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
    public class BranchService : BaseService, IBranchService
    {
        private readonly SchoolDbContext _db;
        private readonly ICurrentUserService _currentUser;

        public BranchService(
            SchoolDbContext db,
            ICurrentUserService currentUser,
            ILogger<BranchService> logger)
            : base(logger)
        {
            _db = db;
            _currentUser = currentUser;
        }

        #region Read

        public async Task<GeneraicResponse> GetAllAsync(int page, int pageSize, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                page = Math.Max(page, 1);
                pageSize = Math.Clamp(pageSize, 1, 100);

                var query = _db.Branches.Where(b => !b.IsDelete);

                var totalCount = await query.CountAsync(ct);

                var items = await query
                    .OrderBy(b => b.ID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(b => new BranchDto
                    {
                        ID = b.ID,
                        Name = b.Name,
                        MobileNo = b.MobileNo,
                        Website = b.Website,
                        Address = b.Address,
                        IsDelete = b.IsDelete,
                        CreatedBy = b.CreatedBy,
                        UpdatedBy = b.UpdatedBy,
                        CreatedDate = b.CreatedDate,
                        UpdatedDate = b.UpdatedDate
                    })
                    .ToListAsync(ct);

                return new
                {
                    totalCount,
                    items
                };
            },
            "Branches found successfully",
            "Failed to fetch branches");
        }

        public async Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _db.Branches
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ID == id && !x.IsDelete, ct);

                if (entity == null)
                    throw new KeyNotFoundException("Branch not found");

                return new BranchDto
                {
                    ID = entity.ID,
                    Name = entity.Name,
                    MobileNo = entity.MobileNo,
                    Website = entity.Website,
                    Address = entity.Address,
                    IsDelete = entity.IsDelete,
                    CreatedBy = entity.CreatedBy,
                    UpdatedBy = entity.UpdatedBy,
                    CreatedDate = entity.CreatedDate,
                    UpdatedDate = entity.UpdatedDate
                };
            },
            "Branch details found successfully",
            "Failed to fetch branch details");
        }

        #endregion

        #region Write

        public async Task<GeneraicResponse> CreateAsync(CreateBranchDto dto, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                var entity = new Branch
                {
                    Name = dto.Name.Trim(),
                    MobileNo = dto.MobileNo,
                    Website = dto.Website,
                    Address = dto.Address,
                    IsDelete = false,
                    CreatedBy = _currentUser.UserId,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = _currentUser.UserId,
                    UpdatedDate = DateTime.UtcNow
                };

                _db.Branches.Add(entity);
                await _db.SaveChangesAsync(ct);
                await trx.CommitAsync(ct);

                return new BranchDto
                {
                    ID = entity.ID,
                    Name = entity.Name,
                    MobileNo = entity.MobileNo,
                    Website = entity.Website,
                    Address = entity.Address,
                    IsDelete = entity.IsDelete,
                    CreatedBy = entity.CreatedBy,
                    CreatedDate = entity.CreatedDate
                };
            },
            "Branch created successfully",
            "Failed to create branch");
        }

        public async Task<GeneraicResponse> UpdateAsync(UpdateBranchDto dto, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                var entity = await _db.Branches
                    .FirstOrDefaultAsync(x => x.ID == dto.ID && !x.IsDelete, ct);

                if (entity == null)
                    throw new KeyNotFoundException("Branch not found");

                entity.Name = dto.Name.Trim();
                entity.MobileNo = dto.MobileNo;
                entity.Website = dto.Website;
                entity.Address = dto.Address;
                entity.UpdatedBy = _currentUser.UserId;
                entity.UpdatedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);
                await trx.CommitAsync(ct);

                return dto;
            },
            "Branch updated successfully",
            "Failed to update branch");
        }

        public async Task<GeneraicResponse> DeleteAsync(int id, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                var entity = await _db.Branches
                    .FirstOrDefaultAsync(x => x.ID == id && !x.IsDelete, ct);

                if (entity == null)
                    throw new KeyNotFoundException("Branch not found");

                entity.IsDelete = true;
                entity.UpdatedBy = _currentUser.UserId;
                entity.UpdatedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);
                await trx.CommitAsync(ct);
            },
            "Branch deleted successfully",
            "Failed to delete branch");
        }

        #endregion
    }
}
