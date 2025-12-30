using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Infrastructure;
using SchoolProcurement.Infrastructure.Persistence;
using SchoolProcurement.Infrastructure.Security;
using SchoolProcurement.Infrastructure.Services;
using System.Security.Cryptography;
using System.Text;

namespace SchoolProcurement.Api.Service
{
    public class UserService : BaseService, IUserService
    {
        private readonly SchoolDbContext _db;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ICurrentUserService _currentUser;

        public UserService(
            SchoolDbContext db,
            IJwtTokenService jwtTokenService,
            ICurrentUserService currentUser,
            ILogger<UserService> logger)
            : base(logger)
        {
            _db = db;
            _jwtTokenService = jwtTokenService;
            _currentUser = currentUser;
        }

        protected int RequireBranch()
        {
            if (_currentUser.IsAdmin)
                throw new UnauthorizedAccessException("Admin user cannot perform branch-scoped operation.");

            return _currentUser.UserBranchId
                ?? throw new InvalidOperationException("Branch context required.");
        }

        private int BranchId => RequireBranch();

        #region User CRUD

        public async Task<GeneraicResponse> GetAllAsync(int page, int pageSize, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 200);

                var query = _db.Users
                    .AsNoTracking()
                    .Where(u => !u.IsDelete && u.BranchID == BranchId);

                var total = await query.CountAsync(ct);

                var entities = await query
                    .OrderBy(u => u.ID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(ct);

                var items = entities.Select(MapToDto).ToList();

                return new PagedResult<UserDto>
                {
                    TotalCount = total,
                    Items = items
                };
            },
            "Users fetched successfully",
            "Failed to fetch users");
        }

        public async Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _db.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u =>
                        u.ID == id &&
                        u.BranchID == BranchId &&
                        !u.IsDelete, ct);

                if (user == null)
                    throw new KeyNotFoundException("User not found");

                return MapToDto(user);
            },
            "User found successfully",
            "Failed to fetch user");
        }

        public async Task<GeneraicResponse> CreateAsync(CreateUserDto dto, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(dto.Password))
                    throw new ArgumentException("Password is required");

                var exists = await _db.Users.AnyAsync(u =>
                    !u.IsDelete &&
                    u.Email == dto.Email &&
                    u.BranchID == dto.BranchID, ct);

                if (exists)
                    throw new InvalidOperationException("User already exists with this email");

                var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
                var hash = HashPassword(dto.Password, salt);

                var user = new Domain.Entities.User
                {
                    Email = dto.Email.Trim(),
                    FirstName = dto.FirstName.Trim(),
                    MiddleName = dto.MiddleName?.Trim(),
                    LastName = dto.LastName.Trim(),
                    Password = hash,
                    Saltkey = salt,
                    UniqueKey = Guid.NewGuid().ToString(),
                    BranchID = dto.BranchID,
                    RoleID = dto.RoleID,
                    CreatedBy = _currentUser.UserId,
                    UpdatedBy = _currentUser.UserId,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    IsDelete = false
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync(ct);

                return MapToDto(user);
            },
            "User created successfully",
            "Failed to create user");
        }

        public async Task<GeneraicResponse> UpdateAsync(UpdateUserDto dto, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _db.Users.FirstOrDefaultAsync(u =>
                    u.ID == dto.ID &&
                    !u.IsDelete &&
                    u.BranchID == BranchId, ct);

                if (user == null)
                    throw new KeyNotFoundException("User not found");

                user.FirstName = dto.FirstName.Trim();
                user.MiddleName = dto.MiddleName?.Trim();
                user.LastName = dto.LastName.Trim();
                user.RoleID = dto.RoleID;
                user.UpdatedBy = _currentUser.UserId;
                user.UpdatedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);

                return MapToDto(user);
            },
            "User updated successfully",
            "Failed to update user");
        }

        public async Task<GeneraicResponse> DeleteAsync(int id, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _db.Users.FirstOrDefaultAsync(u =>
                    u.ID == id &&
                    !u.IsDelete &&
                    u.BranchID == BranchId, ct);

                if (user == null)
                    throw new KeyNotFoundException("User not found");

                user.IsDelete = true;
                user.UpdatedBy = _currentUser.UserId;
                user.UpdatedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);
            },
            "User deleted successfully",
            "Failed to delete user");
        }

        #endregion

        #region Authentication

        public async Task<GeneraicResponse> LoginAsync(LoginRequest req, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _db.Users.Include(m => m.Role)
                    .FirstOrDefaultAsync(u =>
                        u.Email == req.Email &&
                        !u.IsDelete, ct);

                if (user == null)
                    throw new UnauthorizedAccessException("Invalid email or password");

                var hash = HashPassword(req.Password, user.Saltkey);
                if (hash != user.Password)
                    throw new UnauthorizedAccessException("Invalid email or password");

                var token = _jwtTokenService.GenerateToken(user);

                return new LoginResponse
                {
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(8)
                };
            },
            "Login successful",
            "Invalid email or password");
        }

        #endregion

        #region Helpers

        private static UserDto MapToDto(Domain.Entities.User u) => new()
        {
            ID = u.ID,
            RoleID = u.RoleID,
            BranchID = u.BranchID,
            FirstName = u.FirstName,
            MiddleName = u.MiddleName,
            LastName = u.LastName,
            Email = u.Email,
            IsDelete = u.IsDelete
        };

        private static string HashPassword(string password, string salt)
        {
            using var sha = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            return Convert.ToBase64String(sha.ComputeHash(combined));
        }

        #endregion
    }
}
