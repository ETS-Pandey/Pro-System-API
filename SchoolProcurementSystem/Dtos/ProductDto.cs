using System.ComponentModel.DataAnnotations;

namespace SchoolProcurement.Api.Dtos
{
    public class ProductDto
    {
        public int ID { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public int UnitTypeID { get; set; }
        public string? UnitTypeName { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateProductDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [Required]
        public int UnitTypeID { get; set; }

        [Range(0, 9999999999999999.99)]
        public decimal SalesPrice { get; set; }

        [Range(0, 9999999999999999.99)]
        public decimal PurchasePrice { get; set; }

        public int? CreatedBy { get; set; }
    }

    public class UpdateProductDto
    {
        [Required]
        public int ID { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [Required]
        public int UnitTypeID { get; set; }

        [Range(0, 9999999999999999.99)]
        public decimal SalesPrice { get; set; }

        [Range(0, 9999999999999999.99)]
        public decimal PurchasePrice { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
