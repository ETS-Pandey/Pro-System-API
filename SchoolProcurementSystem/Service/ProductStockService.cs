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
    public class ProductStockService : BaseService, IProductStockService
    {
        private readonly SchoolDbContext _db;
        private readonly ICurrentUserService _currentUser;

        public ProductStockService(
            SchoolDbContext db,
            ICurrentUserService currentUser,
            ILogger<ProductStockService> logger)
            : base(logger)
        {
            _db = db;
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

        // ----------------------------------------------------
        // Get stock by product (current branch)
        // ----------------------------------------------------
        public async Task<GeneraicResponse> GetByProductAsync(int productId, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                var rows = await _db.ProductStocks
                    .Include(s => s.Product)
                    .Include(s => s.Branch)
                    .Where(s =>
                        !s.IsDeleted &&
                        s.ProductID == productId &&
                        s.BranchID == BranchId)
                    .ToListAsync(ct);

                return rows.Select(MapToDto).ToList();
            },
            "Product stock fetched successfully",
            "Failed to fetch product stock");
        }

        // ----------------------------------------------------
        // Get stock by branch
        // ----------------------------------------------------
        public async Task<GeneraicResponse> GetByBranchAsync(int branchId, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                var rows = await _db.ProductStocks
                    .Include(s => s.Product)
                    .Include(s => s.Branch)
                    .Where(s =>
                        !s.IsDeleted &&
                        s.BranchID == branchId)
                    .ToListAsync(ct);

                return rows.Select(MapToDto).ToList();
            },
            "Branch stock fetched successfully",
            "Failed to fetch branch stock");
        }

        // ----------------------------------------------------
        // Get stock for product in current branch
        // ----------------------------------------------------
        public async Task<GeneraicResponse> GetAsync(int productId, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                var row = await _db.ProductStocks
                    .Include(s => s.Product)
                    .Include(s => s.Branch)
                    .FirstOrDefaultAsync(s =>
                        !s.IsDeleted &&
                        s.ProductID == productId &&
                        s.BranchID == BranchId, ct);

                if (row == null)
                    throw new KeyNotFoundException("Stock record not found");

                return MapToDto(row);
            },
            "Product stock fetched successfully",
            "Failed to fetch product stock");
        }

        // ----------------------------------------------------
        // Get paged stock list
        // ----------------------------------------------------
        public async Task<GeneraicResponse> GetPagedAsync(
            int page,
            int pageSize,
            int? productId,
            int? branchId,
            string? search,
            CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 200);

                var q = _db.ProductStocks
                    .Include(s => s.Product)
                    .Include(s => s.Branch)
                    .Where(s => !s.IsDeleted && s.BranchID == BranchId)
                    .AsQueryable();

                if (productId.HasValue)
                    q = q.Where(s => s.ProductID == productId.Value);

                if (branchId.HasValue)
                    q = q.Where(s => s.BranchID == branchId.Value);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var s = search.Trim();
                    q = q.Where(x =>
                        x.Product!.Name.Contains(s) ||
                        x.Branch!.Name.Contains(s));
                }

                var total = await q.CountAsync(ct);

                var items = await q
                    .OrderBy(s => s.Product!.Name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(ct);

                return new PagedResult<ProductStockDto>
                {
                    TotalCount = total,
                    Items = items.Select(MapToDto).ToList()
                };
            },
            "Product stock list fetched successfully",
            "Failed to fetch product stock list");
        }

        // ----------------------------------------------------
        // Adjust stock (increase / decrease)
        // ----------------------------------------------------
        public async Task<GeneraicResponse> AdjustStockAsync(ProductStockAdjustmentDto dto, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                if (dto.QuantityDelta == 0)
                    return true;

                var productExists = await _db.Products
                    .AnyAsync(p => p.ID == dto.ProductID, ct);

                if (!productExists)
                    throw new ArgumentException("Product not found");

                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                var stock = await _db.ProductStocks
                    .FirstOrDefaultAsync(s =>
                        s.ProductID == dto.ProductID &&
                        s.BranchID == BranchId, ct);

                if (stock == null)
                {
                    if (dto.QuantityDelta < 0)
                        throw new InvalidOperationException("Insufficient stock");

                    stock = new ProductStock
                    {
                        ProductID = dto.ProductID,
                        BranchID = BranchId,
                        Quantity = dto.QuantityDelta,
                        ReservedQty = 0,
                        ReorderLevel = 0,
                        CreatedBy = _currentUser.UserId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedBy = _currentUser.UserId,
                        UpdatedDate = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    _db.ProductStocks.Add(stock);
                }
                else
                {
                    var newQty = stock.Quantity + dto.QuantityDelta;
                    if (newQty < 0)
                        throw new InvalidOperationException("Insufficient stock");

                    stock.Quantity = newQty;
                    stock.UpdatedBy = _currentUser.UserId;
                    stock.UpdatedDate = DateTime.UtcNow;
                }

                await _db.SaveChangesAsync(ct);
                await trx.CommitAsync(ct);

                return true;
            },
            "Stock adjusted successfully",
            "Failed to adjust stock");
        }

        // ----------------------------------------------------
        // Get low stock items
        // ----------------------------------------------------
        public async Task<GeneraicResponse> GetLowStockAsync(CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                var rows = await _db.ProductStocks
                    .Include(s => s.Product)
                    .Include(s => s.Branch)
                    .Where(s =>
                        !s.IsDeleted &&
                        s.BranchID == BranchId &&
                        s.ReorderLevel != null &&
                        s.Quantity <= s.ReorderLevel)
                    .ToListAsync(ct);

                return rows.Select(MapToDto).ToList();
            },
            "Low stock items fetched successfully",
            "Failed to fetch low stock items");
        }

        // ----------------------------------------------------
        // Mapper
        // ----------------------------------------------------
        private static ProductStockDto MapToDto(ProductStock s)
        {
            return new ProductStockDto
            {
                ID = s.ID,
                ProductID = s.ProductID,
                ProductName = s.Product?.Name,
                BranchID = s.BranchID,
                BranchName = s.Branch?.Name,
                Quantity = s.Quantity,
                ReservedQty = s.ReservedQty,
                ReorderLevel = s.ReorderLevel,
                IsDeleted = s.IsDeleted,
                CreatedBy = s.CreatedBy,
                CreatedDate = s.CreatedDate,
                UpdatedBy = s.UpdatedBy,
                UpdatedDate = s.UpdatedDate
            };
        }
    }
}
