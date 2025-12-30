using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure;
using SchoolProcurement.Infrastructure.Persistence;
using SchoolProcurement.Infrastructure.Security;
using SchoolProcurement.Infrastructure.Services;

namespace SchoolProcurement.Api.Service
{
    public class SorChatService : BaseService, ISorChatService
    {
        private readonly SchoolDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly ICurrentUserService _currentUser;

        public SorChatService(
            SchoolDbContext db,
            ICurrentUserService currentUser,
            IWebHostEnvironment env,
            ILogger<SorChatService> logger) : base(logger)
        {
            _db = db;
            _currentUser = currentUser;
            _env = env;
        }

        protected int RequireBranch()
        {
            if (_currentUser.IsAdmin)
                throw new UnauthorizedAccessException("Admin user cannot perform branch-scoped operation.");

            return _currentUser.UserBranchId
                ?? throw new InvalidOperationException("Branch context required.");
        }

        private int BranchId => RequireBranch();

        public async Task<GeneraicResponse> CreateAsync(
            SorChatCreateDto dto,
            IEnumerable<IFormFile>? files,
            CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                if (dto.ParentChatID.HasValue)
                {
                    var parentExists = await _db.SorChats.AnyAsync(x =>
                        x.ID == dto.ParentChatID &&
                        x.SORID == dto.SORID &&
                        !x.IsDeleted, ct);

                    if (!parentExists)
                        throw new InvalidOperationException("Quoted message not found");
                }

                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                var chat = new SorChat
                {
                    SORID = dto.SORID,
                    BranchID = BranchId,
                    SenderUserID = _currentUser.UserId!.Value,
                    Message = dto.Message,
                    ParentChatID = dto.ParentChatID,
                    CreatedBy = _currentUser.UserId,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                };

                _db.SorChats.Add(chat);
                await _db.SaveChangesAsync(ct);

                // Attachments
                if (files != null && files.Count() > 0)
                {
                    var path = Path.Combine(_env.WebRootPath, "uploads", "sor-chat", chat.ID.ToString());
                    Directory.CreateDirectory(path);

                    foreach (var file in files)
                    {
                        var name = $"{Guid.NewGuid()}_{file.FileName}";
                        var fullPath = Path.Combine(path, name);

                        await using var fs = new FileStream(fullPath, FileMode.Create);
                        await file.CopyToAsync(fs, ct);

                        _db.SorChatAttachments.Add(new SorChatAttachment
                        {
                            SORChatID = chat.ID,
                            FileName = file.FileName,
                            FilePath = $"/uploads/sor-chat/{chat.ID}/{name}",
                            ContentType = file.ContentType,
                            CreatedBy = _currentUser.UserId,
                            CreatedDate = DateTime.UtcNow
                        });
                    }

                    await _db.SaveChangesAsync(ct);
                    await trx.CommitAsync(ct);
                }
            },
            "Message sent successfully",
            "Failed to send message");
        }

        public async Task<GeneraicResponse> GetBySorAsync(int sorId, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var chats = await _db.SorChats
                    .Include(x => x.SenderUser)
                    .Include(x => x.ParentChat).ThenInclude(p => p!.SenderUser)
                    .Include(x => x.Attachments)
                    .Where(x => x.SORID == sorId && x.BranchID == BranchId && !x.IsDeleted)
                    .OrderBy(x => x.CreatedDate)
                    .ToListAsync(ct);

                return chats.Select(c => new SorChatDto
                {
                    ID = c.ID,
                    SenderUserID = c.SenderUserID,
                    SenderName = $"{c.SenderUser.FirstName} {c.SenderUser.LastName}",
                    Message = c.Message,
                    ParentChatID = c.ParentChatID,
                    ParentMessage = c.ParentChat?.Message,
                    ParentSenderName = c.ParentChat != null
                        ? $"{c.ParentChat.SenderUser.FirstName} {c.ParentChat.SenderUser.LastName}"
                        : null,
                    CreatedDate = c.CreatedDate,
                    Attachments = c.Attachments.Select(a => new SorChatAttachmentDto
                    {
                        ID = a.ID,
                        FileName = a.FileName,
                        FilePath = a.FilePath
                    }).ToList()
                });
            },
            "Chat loaded successfully",
            "Failed to load chat");
        }

        public async Task<GeneraicResponse> DeleteAsync(int chatId, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                await using var trx = await _db.Database.BeginTransactionAsync(ct);
                var chat = await _db.SorChats
                    .FirstOrDefaultAsync(c => c.ID == chatId && !c.IsDeleted, ct);

                if (chat == null)
                    throw new KeyNotFoundException("Chat message not found");

                chat.IsDeleted = true;
                chat.UpdatedBy = _currentUser.UserId;
                chat.UpdatedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);
                await trx.CommitAsync(ct);
            },
            "Message deleted successfully",
            "Failed to delete message");
        }
    }
}
