namespace SchoolProcurement.Api.Dtos
{
    public class CreatePurchaseOrderDto
    {
        public int SupplierContactID { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
    }

    public class CreatePurchaseOrderItemDto
    {
        public int ProductID { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class ReceivePurchaseOrderDto
    {
        public int PurchaseOrderID { get; set; }
        public string? Remarks { get; set; }
        public List<ReceivePurchaseOrderItemDto> Items { get; set; } = new();
    }

    public class ReceivePurchaseOrderItemDto
    {
        public int PurchaseOrderItemID { get; set; }
        public decimal ReceivedQty { get; set; }
    }

    public class AddPurchaseOrderPaymentDto
    {
        public int PurchaseOrderID { get; set; }
        public decimal Amount { get; set; }
        public string? ReferenceNo { get; set; }
    }


    public class PurchaseOrderDto
    {
        public int ID { get; set; }
        public string PONumber { get; set; } = null!;

        public int BranchID { get; set; }
        public string? BranchName { get; set; }

        public string SupplierName { get; set; } = null!;
        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount => TotalAmount - PaidAmount;

        public string Status { get; set; } = null!;
        public DateTime CreatedDate { get; set; }

        public List<PurchaseOrderItemDto> Items { get; set; } = new();
    }


    public class PurchaseOrderItemDto
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }

        public decimal OrderedQty { get; set; }
        public decimal ReceivedQty { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
