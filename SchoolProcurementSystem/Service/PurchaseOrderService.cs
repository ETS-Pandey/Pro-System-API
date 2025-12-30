using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Helper;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Domain.Constants;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure;
using SchoolProcurement.Infrastructure.Constants;
using SchoolProcurement.Infrastructure.Persistence;
using SchoolProcurement.Infrastructure.Security;
using SchoolProcurement.Infrastructure.Services;

namespace SchoolProcurement.Api.Service
{
    public class PurchaseOrderService : BaseService, IPurchaseOrderService
    {
        private readonly SchoolDbContext _db;
        private readonly ICurrentUserService _currentUser;
        private readonly IProductStockService _productStockService;
        private readonly ISmtpEmailService _smtpEmailService;
        private readonly INotificationService _notificationService;

        public PurchaseOrderService(
            SchoolDbContext db,
            ICurrentUserService currentUser,
            ISmtpEmailService smtpEmailService,
            IProductStockService productStockService,
            INotificationService notificationService,
            ILogger<ProductService> logger)
            : base(logger)
        {
            _db = db;
            _currentUser = currentUser;
            _smtpEmailService = smtpEmailService;
            _productStockService = productStockService;
            _notificationService = notificationService;
        }

        protected int RequireBranch()
        {
            if (_currentUser.IsAdmin)
                throw new UnauthorizedAccessException("Admin user cannot perform branch-scoped operation.");

            return _currentUser.UserBranchId
                ?? throw new InvalidOperationException("Branch context required.");
        }

        private int BranchId => RequireBranch();

        // ---------------- GET BY ID ----------------
        public async Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var po = await _db.PurchaseOrders
                    .Include(p => p.Items).ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(p => p.ID == id && p.BranchID == BranchId, ct);

                if (po == null)
                    throw new KeyNotFoundException("Purchase order not found");

                return MapToDto(po);
            }, "Purchase order found", "Failed to fetch purchase order");
        }

        // ---------------- PAGED LIST ----------------
        public async Task<GeneraicResponse> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 100);

                var q = _db.PurchaseOrders.Where(p => p.BranchID == BranchId);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var s = search.Trim().ToLower();
                    q = q.Where(c => c.Status != null &&
                        c.Status.ToLower().Contains(s));
                }

                var total = await q.CountAsync(ct);

                var items = await q
                    .OrderByDescending(p => p.CreatedDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => MapToDto(p))
                    .ToListAsync(ct);

                return new PagedResult<PurchaseOrderDto>
                {
                    TotalCount = total,
                    Items = items
                };
            }, "Purchase orders fetched", "Failed to fetch purchase orders");
        }

        // ---------------- ADD PAYMENT ----------------
        public async Task<GeneraicResponse> AddPaymentAsync(AddPurchaseOrderPaymentDto dto, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                var po = await _db.PurchaseOrders.FirstOrDefaultAsync(p =>
                    p.ID == dto.PurchaseOrderID && p.BranchID == BranchId, ct);

                if (po == null)
                    throw new KeyNotFoundException("Purchase order not found");

                var payment = new PurchaseOrderPayment
                {
                    PurchaseOrderID = po.ID,
                    Amount = dto.Amount,
                    ReferenceNo = dto.ReferenceNo,
                    PaymentDate = DateTime.UtcNow,
                    CreatedBy = _currentUser.UserId,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                };

                po.PaidAmount += dto.Amount;
                po.UpdatedBy = _currentUser.UserId;
                po.UpdatedDate = DateTime.UtcNow;

                _db.PurchaseOrderPayments.Add(payment);
                await _db.SaveChangesAsync(ct);
                await trx.CommitAsync(ct);
            }, "Payment added successfully", "Failed to add payment");
        }

        public async Task<GeneraicResponse> CreateAsync(CreatePurchaseOrderDto dto, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                await using var trx = await _db.Database.BeginTransactionAsync(ct);
                var now = DateTime.UtcNow;

                var contact = await _db.ContactDetails
                    .FirstOrDefaultAsync(c =>
                        c.ID == dto.SupplierContactID &&
                        c.BranchID == BranchId &&
                        !c.IsDeleted, ct);

                if (contact == null)
                    throw new InvalidOperationException("Supplier contact not found for this branch");

                var po = new PurchaseOrder
                {
                    PONumber = await GeneratePONumberAsync(BranchId, ct),
                    BranchID = BranchId,
                    SupplierContactID = dto.SupplierContactID,
                    OrderDate = now,
                    Status = PurchaseOrderStatus.Open,
                    ExpectedDeliveryDate = dto.ExpectedDeliveryDate,
                    TotalAmount = dto.Items.Sum(i => i.OrderedQty * i.UnitPrice),
                    PaidAmount = 0,
                    IsDeleted = false,
                    CreatedBy = _currentUser.UserId,
                    CreatedDate = now,
                    UpdatedBy = _currentUser.UserId,
                    UpdatedDate = now
                };

                _db.PurchaseOrders.Add(po);
                await _db.SaveChangesAsync(ct);

                foreach (var item in dto.Items)
                {
                    _db.PurchaseOrderItems.Add(new PurchaseOrderItem
                    {
                        PurchaseOrderID = po.ID,
                        ProductID = item.ProductID,
                        OrderedQty = item.OrderedQty,
                        //TotalPrice = item.UnitPrice * item.OrderedQty,
                        ReceivedQty = 0,
                        UnitPrice = item.UnitPrice,
                        CreatedBy = _currentUser.UserId,
                        CreatedDate = now,
                        UpdatedBy = _currentUser.UserId,
                        UpdatedDate = now,
                        IsDeleted = false
                    });
                }

                await _db.SaveChangesAsync(ct);

                var purchaseOrder = await _db.PurchaseOrders
                        .Include(m => m.SupplierContact)
                        .Include(m => m.Branch)
                        .Include(m => m.Items)
                        .Include("Items.Product")
                        .Where(m => m.ID == po.ID).FirstOrDefaultAsync();

                var branchAdmin = await _db.Users.Include(m => m.Role)
                    .Where(u => u.BranchID == po.BranchID && u.Role.Name == "BranchAdmin")
                    .FirstOrDefaultAsync(ct);

                if (branchAdmin != null)
                    await _notificationService.CreateAsync(
                            branchAdmin.ID,
                            "Purchase Order Created",
                            $"Purchase Order has been created by {_currentUser.UserId ?? 0}",
                            NotificationTypes.PurchaseOrderCreated,
                            "PURCHASEORDER",
                            purchaseOrder.ID,
                            $"/purchaseorder/{purchaseOrder.ID}",
                            ct
                        );

                // Generate PDF
                var pdfBytes = PurchaseOrderPdfGenerator.Generate(purchaseOrder);

                // Email
                var email = new EmailMessage
                {
                    To = new List<string> { purchaseOrder.SupplierContact?.EmailAddress! },
                    Subject = $"Purchase Order - {purchaseOrder.PONumber}",
                    Body = $@"
                        <p>Hello {purchaseOrder.SupplierContact?.Name},</p>
                        <p>Please find attached Purchase Order <b>{purchaseOrder.PONumber}</b>.</p>
                        <p>Regards,<br/>School Procurement System</p>",
                    Attachments =
                    {
                        ( $"{po.PONumber}.pdf", pdfBytes )
                    }
                };

                await _smtpEmailService.SendEmailAsync(email, po.BranchID, ct);
                await trx.CommitAsync(ct);
                return po.ID;
            },
            "Purchase order created successfully",
            "Failed to create purchase order");
        }

        public async Task<GeneraicResponse> ReceiveAsync(ReceivePurchaseOrderDto dto, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var branchId = RequireBranch();
                var now = DateTime.UtcNow;

                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                var receipt = new PurchaseOrderReceipt
                {
                    PurchaseOrderID = dto.PurchaseOrderID,
                    ReceiptDate = now,
                    Remarks = dto.Remarks,
                    IsDeleted = false,
                    CreatedBy = _currentUser.UserId,
                    CreatedDate = now
                };

                _db.PurchaseOrderReceipts.Add(receipt);
                await _db.SaveChangesAsync(ct);

                foreach (var r in dto.Items)
                {
                    var poItem = await _db.PurchaseOrderItems
                        .FirstAsync(i => i.ID == r.PurchaseOrderItemID, ct);

                    poItem.ReceivedQty += r.ReceivedQty;

                    // 🔥 UPDATE PRODUCT STOCK
                    await _productStockService.AdjustStockAsync(new ProductStockAdjustmentDto
                    {
                        ProductID = poItem.ProductID,
                        QuantityDelta = r.ReceivedQty
                    }, ct);

                    _db.PurchaseOrderReceiptItems.Add(new PurchaseOrderReceiptItem
                    {
                        ReceiptID = receipt.ID,
                        PurchaseOrderItemID = poItem.ID,
                        ReceivedQty = r.ReceivedQty,
                        CreatedBy = _currentUser.UserId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedBy = _currentUser.UserId,
                        UpdatedDate = DateTime.UtcNow
                    });
                }

                // Update PO status
                var allReceived = await _db.PurchaseOrderItems
                    .Where(i => i.PurchaseOrderID == dto.PurchaseOrderID)
                    .AllAsync(i => i.ReceivedQty >= i.OrderedQty, ct);

                var po = await _db.PurchaseOrders.FindAsync(dto.PurchaseOrderID);
                po!.Status = allReceived
                    ? PurchaseOrderStatus.Completed
                    : PurchaseOrderStatus.PartiallyReceived;

                await _db.SaveChangesAsync(ct);
                await trx.CommitAsync(ct);
            },
            "Items received successfully",
            "Failed to receive purchase order items");
        }

        private async Task<string> GeneratePONumberAsync(int branchId, CancellationToken ct)
        {
            // 1️⃣ Get branch code (short name)
            var branchCode = await _db.Branches
                .Where(b => b.ID == branchId)
                .Select(b => b.Name.Substring(0, 3))
                .FirstOrDefaultAsync(ct);

            if (string.IsNullOrWhiteSpace(branchCode))
                branchCode = "BR";

            branchCode = branchCode.ToUpper();

            // 2️⃣ Date part
            var today = DateTime.UtcNow.Date;
            var datePart = today.ToString("yyyyMMdd");

            // 3️⃣ Get today's running count (branch specific)
            var countToday = await _db.PurchaseOrders
                .Where(p =>
                    p.BranchID == branchId &&
                    p.CreatedDate >= today &&
                    p.CreatedDate < today.AddDays(1))
                .CountAsync(ct);

            var sequence = countToday + 1;

            // 4️⃣ Build PO number
            return $"PO-{branchCode}-{datePart}-{sequence:0000}";
        }

        // ---------------- HELPERS ----------------
        private static PurchaseOrderDto MapToDto(PurchaseOrder p)
        {
            return new PurchaseOrderDto
            {
                ID = p.ID,
                PONumber = p.PONumber,
                BranchID = p.BranchID,
                SupplierName = p.SupplierContact?.Name ?? "",
                OrderDate = p.OrderDate,
                TotalAmount = p.TotalAmount,
                PaidAmount = p.PaidAmount,
                Status = p.Status,
                CreatedDate = p.CreatedDate,
                Items = p.Items.Select(i => new PurchaseOrderItemDto
                {
                    ID = i.ID,
                    ProductID = i.ProductID,
                    ProductName = i.Product?.Name,
                    OrderedQty = i.OrderedQty,
                    ReceivedQty = i.ReceivedQty,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.UnitPrice * i.OrderedQty,
                }).ToList()
            };
        }
    }
}
