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
    public class ProductService : BaseService, IProductService
    {
        private readonly SchoolDbContext _db;
        private readonly ICurrentUserService _currentUser;

        public ProductService(
            SchoolDbContext db,
            ICurrentUserService currentUser,
            ILogger<ProductService> logger)
            : base(logger)
        {
            _db = db;
            _currentUser = currentUser;
        }

        // ----------------------------------------------------
        // Get paged + filtered list
        // ----------------------------------------------------
        public async Task<GeneraicResponse> GetAllPagedAsync(
            int page,
            int pageSize,
            int? categoryId,
            int? unitTypeId,
            string? search,
            CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 200);

                var q = _db.Products
                    .Include(p => p.Category)
                    .Include(p => p.UnitType)
                    .Where(p => !p.IsDeleted)
                    .AsQueryable();

                if (categoryId.HasValue)
                    q = q.Where(p => p.CategoryID == categoryId.Value);

                if (unitTypeId.HasValue)
                    q = q.Where(p => p.UnitTypeID == unitTypeId.Value);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var s = search.Trim();
                    q = q.Where(p => p.Name.Contains(s));
                }

                var totalCount = await q.CountAsync(ct);

                var entities = await q
                    .OrderBy(p => p.Name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(ct);

                return new
                {
                    totalCount,
                    items = entities.Select(MapToDto).ToList()
                };
            },
            "Products fetched successfully",
            "Failed to fetch products");
        }

        // ----------------------------------------------------
        // Get product by ID
        // ----------------------------------------------------
        public async Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _db.Products
                    .Include(p => p.Category)
                    .Include(p => p.UnitType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.ID == id && !p.IsDeleted, ct);

                if (entity == null)
                    throw new KeyNotFoundException("Product not found");

                return MapToDto(entity);
            },
            "Product fetched successfully",
            "Failed to fetch product");
        }

        // ----------------------------------------------------
        // Create product
        // ----------------------------------------------------
        public async Task<GeneraicResponse> CreateAsync(Product product, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                product.IsDeleted = false;
                product.CreatedBy = _currentUser.UserId;
                product.CreatedDate = DateTime.UtcNow;
                product.UpdatedBy = _currentUser.UserId;
                product.UpdatedDate = DateTime.UtcNow;

                _db.Products.Add(product);
                await _db.SaveChangesAsync(ct);

                await trx.CommitAsync(ct);

                return product.ID;
            },
            "Product created successfully",
            "Failed to create product");
        }

        // ----------------------------------------------------
        // Update product
        // ----------------------------------------------------
        public async Task<GeneraicResponse> UpdateAsync(Product product, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var existing = await _db.Products
                    .FirstOrDefaultAsync(x => x.ID == product.ID && !x.IsDeleted, ct);

                if (existing == null)
                    throw new KeyNotFoundException("Product not found");

                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.CategoryID = product.CategoryID;
                existing.UnitTypeID = product.UnitTypeID;
                existing.SalesPrice = product.SalesPrice;
                existing.PurchasePrice = product.PurchasePrice;
                existing.UpdatedBy = _currentUser.UserId;
                existing.UpdatedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);
                await trx.CommitAsync(ct);

                return existing.ID;
            },
            "Product updated successfully",
            "Failed to update product");
        }

        // ----------------------------------------------------
        // Soft delete product
        // ----------------------------------------------------
        public async Task<GeneraicResponse> DeleteAsync(int id, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var existing = await _db.Products
                    .FirstOrDefaultAsync(x => x.ID == id && !x.IsDeleted, ct);

                if (existing == null)
                    throw new KeyNotFoundException("Product not found");

                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                existing.IsDeleted = true;
                existing.UpdatedBy = _currentUser.UserId;
                existing.UpdatedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);
                await trx.CommitAsync(ct);

                return true;
            },
            "Product deleted successfully",
            "Failed to delete product");
        }

        // ----------------------------------------------------
        // Mapper
        // ----------------------------------------------------
        private static ProductDto MapToDto(Product p)
        {
            return new ProductDto
            {
                ID = p.ID,
                Name = p.Name,
                Description = p.Description,
                CategoryID = p.CategoryID,
                CategoryName = p.Category?.Name,
                UnitTypeID = p.UnitTypeID,
                UnitTypeName = p.UnitType?.Name,
                SalesPrice = p.SalesPrice,
                PurchasePrice = p.PurchasePrice,
                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                UpdatedBy = p.UpdatedBy,
                UpdatedDate = p.UpdatedDate
            };
        }
    }
}
