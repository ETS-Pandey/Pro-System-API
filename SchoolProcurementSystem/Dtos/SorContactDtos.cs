using System.ComponentModel.DataAnnotations;

namespace SchoolProcurement.Api.Dtos
{
    public class SorContactMappingDto
    {
        public int ID { get; set; }
        public int BranchID { get; set; }
        public int SORID { get; set; }
        public int ContactID { get; set; }
        public string? Description { get; set; }
        public string? UniqueString { get; set; }
        public decimal? Rate { get; set; }
        public string? OtherDescription { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public List<SorContactAttachmentDto> Attachments { get; set; } = new();
    }

    public class CreateSorContactMappingDto
    {
        [Required] public int SORID { get; set; }
        [Required] public int ContactID { get; set; }
        public string? Description { get; set; }
        public string? UniqueString { get; set; }
        public decimal? Rate { get; set; }
        public string? OtherDescription { get; set; }
    }

    public class UpdateSorContactMappingDto
    {
        [Required] public int ID { get; set; }
        [Required] public int SORID { get; set; }
        [Required] public int ContactID { get; set; }
        public string? Description { get; set; }
        public string? UniqueString { get; set; }
        public decimal? Rate { get; set; }
        public string? OtherDescription { get; set; }
    }

    public class ApproveQuotationItemRowDto
    {
        public int SorContactMappingItemID { get; set; }
        public bool Approve { get; set; }
    }

    public class ApprovedQuotationItemResultDto
    {
        public int ItemID { get; set; }
        public string Status { get; set; } = null!;
    }

    public class InviteQuotationDto
    {
        public int SORID { get; set; }
        public List<int> ContactIDs { get; set; } = new();
    }

    public class SubmitQuotationDto
    {
        public string Token { get; set; } = null!;
        public List<SubmitQuotationItemDto> Items { get; set; } = new();
    }

    public class SubmitQuotationItemDto
    {
        public int ProductID { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuotedRate { get; set; }
    }


    public class ApproveQuotationItemsDto
    {
        public int SorContactMappingID { get; set; }
        public List<ApproveQuotationItemDto> Items { get; set; } = new();
    }

    public class ApproveQuotationItemDto
    {
        public int SorContactMappingItemID { get; set; }
        public bool Approve { get; set; }
    }


}
