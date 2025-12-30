using System.ComponentModel.DataAnnotations;

namespace SchoolProcurement.Api.Dtos
{
    public class SorContactAttachmentDto
    {
        public int ID { get; set; }
        public int MappingID { get; set; }
        public string FileName { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public DateTime CreatedDate { get; set; }
    }

    public class UpdateMappingWithAttachmentsDto
    {
        [Required] public int ID { get; set; }                // mapping id
        public string? Description { get; set; }
        public decimal? Rate { get; set; }
        // files will be sent via multipart/form-data; see controller [FromForm] usage
    }
}
