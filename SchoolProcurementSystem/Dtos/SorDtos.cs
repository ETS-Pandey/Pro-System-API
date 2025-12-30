using System.ComponentModel.DataAnnotations;

namespace SchoolProcurement.Api.Dtos
{
    public class CreateSorWithFilesDto
    {
        [Required]
        public string Model { get; set; } = default!; // JSON string of CreateSorDto

        public List<IFormFile>? Files { get; set; } = new();
    }

    public class SorItemCreateDto
    {
        [Required] public int ProductID { get; set; }
        public int? UnitTypeID { get; set; }
        [Required] public decimal Quantity { get; set; }
        public decimal? EstimatedCost { get; set; }
        public string? TechnicalSpecifications { get; set; }
    }

    public class SorItemDto
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int? UnitTypeID { get; set; }
        public string? UnitTypeName { get; set; }
        public decimal Quantity { get; set; }
        public decimal? EstimatedCost { get; set; }
        public string? TechnicalSpecifications { get; set; }
    }

    // Attachment DTOs
    public class SorAttachmentCreateDto
    {
        [Required] public string FileName { get; set; } = default!;
        [Required] public string FilePath { get; set; } = default!; // caller stores file and sends path/url
    }

    public class SorAttachmentDto
    {
        public int ID { get; set; }
        public string FileName { get; set; } = default!;
        public string FilePath { get; set; } = default!;
    }

    // Assignment DTOs
    public class SorAssignDto
    {
        [Required] public int SORID { get; set; }
        [Required] public int ToUserID { get; set; }
        public string? Note { get; set; }
    }

    public class SorApproveDto
    {
        [Required] public int SORID { get; set; }
        public bool Approve { get; set; } = true;
        public string? Note { get; set; }
    }

    // Create SOR
    public class CreateSorDto
    {
        //[Required] public int BranchID { get; set; }
        public int? DepartmentID { get; set; }
        public string? PurposeDescription { get; set; }
        public string? AdditionalJustification { get; set; }
        public DateTime? RequiredByDate { get; set; }
        public int? UrgencyLevelID { get; set; }

        // optional initial assignee
        public int? InitialAssignedUserID { get; set; }

        public List<SorItemCreateDto> Items { get; set; } = new();
        public List<SorAttachmentCreateDto> Attachments { get; set; } = new();
    }

    // SOR response DTO
    public class SorDto
    {
        public int ID { get; set; }
        public string UniqueString { get; set; } = default!;
        public int BranchID { get; set; }
        public string? BranchName { get; set; }
        public int? DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
        public string? PurposeDescription { get; set; }
        public string? AdditionalJustification { get; set; }
        public DateTime? RequiredByDate { get; set; }
        public int? UrgencyLevelID { get; set; }
        public string? UrgencyLevelName { get; set; }
        public string Status { get; set; } = default!;
        public int? CurrentAssignedUserID { get; set; }
        public string? CurrentAssignedUserName { get; set; }

        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }

        public List<SorItemDto> Items { get; set; } = new();
        public List<SorAttachmentDto> Attachments { get; set; } = new();
        public List<(int AssignmentId, int UserId, string? Note, DateTime CreatedDate)> Assignments { get; set; } = new();
    }
}
