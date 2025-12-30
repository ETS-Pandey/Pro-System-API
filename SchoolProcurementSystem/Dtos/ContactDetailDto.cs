using System.ComponentModel.DataAnnotations;

namespace SchoolProcurement.Api.Dtos
{
    public class ContactDetailDto
    {
        public int ID { get; set; }
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public string Name { get; set; } = default!;
        public string? Address { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailAddress { get; set; }
        public string? UniqueString { get; set; }
    }

    public class CreateContactDto
    {
        //public int BranchID { get; set; }     // optional: backend can auto-fill from current user

        [Required, MaxLength(250)]
        public string Name { get; set; } = default!;
        public string? Address { get; set; }
        public string? Postcode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailAddress { get; set; }
    }

    public class UpdateContactDto
    {
        public int ID { get; set; }
        //public int BranchID { get; set; }       // enforce branch consistency
        public string Name { get; set; } = default!;
        public string? Address { get; set; }
        public string? Postcode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailAddress { get; set; }
    }
}
