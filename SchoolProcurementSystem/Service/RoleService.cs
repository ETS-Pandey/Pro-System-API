using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure;
using SchoolProcurement.Infrastructure.Persistence;
using SchoolProcurement.Infrastructure.Services;

namespace SchoolProcurement.Api.Service
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly SchoolDbContext _db;

        public RoleService(
            SchoolDbContext db,
            ILogger<RoleService> logger)
            : base(logger)
        {
            _db = db;
        }

        // ----------------------------------------------------
        // Get all roles
        // ----------------------------------------------------
        public async Task<GeneraicResponse> GetAllAsync(CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                var list = await _db.Roles
                    .AsNoTracking()
                    .OrderBy(r => r.Name)
                    .Select(r => new RoleDto
                    {
                        ID = r.ID,
                        Name = r.Name,
                        CreatedDate = r.CreatedDate
                    })
                    .ToListAsync(ct);

                return list;
            },
            "Roles fetched successfully",
            "Failed to fetch roles");
        }

        // ----------------------------------------------------
        // Get role by ID
        // ----------------------------------------------------
        public async Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                var role = await _db.Roles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.ID == id, ct);

                if (role == null)
                    throw new KeyNotFoundException("Role not found");

                return new RoleDto
                {
                    ID = role.ID,
                    Name = role.Name,
                    CreatedDate = role.CreatedDate
                };
            },
            "Role fetched successfully",
            "Failed to fetch role");
        }

        // ----------------------------------------------------
        // Create role
        // ----------------------------------------------------
        public async Task<GeneraicResponse> CreateAsync(CreateRoleDto dto, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("Role name is required");

                var exists = await _db.Roles
                    .AnyAsync(r => r.Name.ToLower() == dto.Name.Trim().ToLower(), ct);

                if (exists)
                    throw new InvalidOperationException("Role already exists");

                var entity = new Role
                {
                    Name = dto.Name.Trim(),
                    CreatedDate = DateTime.UtcNow
                };

                _db.Roles.Add(entity);
                await _db.SaveChangesAsync(ct);

                return new RoleDto
                {
                    ID = entity.ID,
                    Name = entity.Name,
                    CreatedDate = entity.CreatedDate
                };
            },
            "Role created successfully",
            "Failed to create role");
        }

        // ----------------------------------------------------
        // Update role
        // ----------------------------------------------------
        public async Task<GeneraicResponse> UpdateAsync(UpdateRoleDto dto, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _db.Roles
                    .FirstOrDefaultAsync(r => r.ID == dto.ID, ct);

                if (entity == null)
                    throw new KeyNotFoundException("Role not found");

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("Role name is required");

                var exists = await _db.Roles
                    .AnyAsync(r =>
                        r.ID != dto.ID &&
                        r.Name.ToLower() == dto.Name.Trim().ToLower(), ct);

                if (exists)
                    throw new InvalidOperationException("Another role with the same name already exists");

                entity.Name = dto.Name.Trim();
                await _db.SaveChangesAsync(ct);

                return true;
            },
            "Role updated successfully",
            "Failed to update role");
        }

        // ----------------------------------------------------
        // Delete role (hard delete – change to soft if needed)
        // ----------------------------------------------------
        public async Task<GeneraicResponse> DeleteAsync(int id, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _db.Roles
                    .FirstOrDefaultAsync(r => r.ID == id, ct);

                if (entity == null)
                    throw new KeyNotFoundException("Role not found");

                _db.Roles.Remove(entity);
                await _db.SaveChangesAsync(ct);

                return true;
            },
            "Role deleted successfully",
            "Failed to delete role");
        }
    }
}
