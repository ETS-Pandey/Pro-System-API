using System.ComponentModel.DataAnnotations;

namespace SchoolProcurement.Api.Dtos
{
    public class ProductStockDto
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int BranchID { get; set; }
        public string? BranchName { get; set; }
        public decimal Quantity { get; set; }
        public decimal ReservedQty { get; set; }
        public decimal? ReorderLevel { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class ProductStockAdjustmentDto
    {
        [Required] public int ProductID { get; set; }
        [Required] public decimal QuantityDelta { get; set; } // +ve to add, -ve to subtract
        public string? Reason { get; set; }
    }

    public class ProductStockTransferDto
    {
        [Required] public int ProductID { get; set; }
        [Required] public int FromBranchID { get; set; }
        [Required] public int ToBranchID { get; set; }
        [Required] public decimal Quantity { get; set; }
        public string? Reason { get; set; }
    }

    public class PagedRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }

    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    }
}
