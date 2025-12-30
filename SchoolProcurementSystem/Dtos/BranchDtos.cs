using System.ComponentModel.DataAnnotations;

namespace SchoolProcurement.Api.Dtos
{
    public class BranchDto { public int ID { get; set; } public string Name { get; set; } = default!; public string? MobileNo { get; set; } public string? Website { get; set; } public string? Address { get; set; } public bool IsDelete { get; set; } public int? CreatedBy { get; set; } public int? UpdatedBy { get; set; } public DateTime CreatedDate { get; set; } public DateTime? UpdatedDate { get; set; } }

    public class CreateBranchDto
    {
        [Required][StringLength(200)] public string Name { get; set; } = default!;
        [StringLength(20)] public string? MobileNo { get; set; }
        [StringLength(200)] public string? Website { get; set; }
        [StringLength(500)] public string? Address { get; set; }
        public int? CreatedBy { get; set; }
    }

    public class UpdateBranchDto
    {
        [Required] public int ID { get; set; }
        [Required][StringLength(200)] public string Name { get; set; } = default!;
        [StringLength(20)] public string? MobileNo { get; set; }
        [StringLength(200)] public string? Website { get; set; }
        [StringLength(500)] public string? Address { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
