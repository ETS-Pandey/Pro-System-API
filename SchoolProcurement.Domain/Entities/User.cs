namespace SchoolProcurement.Domain.Entities
{
    public class User
    {
        public int ID { get; set; }

        public int RoleID { get; set; }
        public Role Role { get; set; } = null!;

        // ✅ SINGLE OPTIONAL FK
        public int? BranchID { get; set; }
        public Branch? Branch { get; set; }

        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Saltkey { get; set; } = null!;
        public string UniqueKey { get; set; } = null!;

        public bool IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

}
