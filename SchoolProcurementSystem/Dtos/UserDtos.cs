using System.ComponentModel.DataAnnotations;

namespace SchoolProcurement.Api.Dtos
{
    public class UserDto
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public int? BranchID { get; set; }
        public string FirstName { get; set; } = default!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool IsDelete { get; set; }
    }

    public class CreateUserDto
    {
        [Required][StringLength(150)] public string Email { get; set; } = default!;
        [Required][StringLength(150)] public string FirstName { get; set; } = default!;
        [StringLength(150)] public string? MiddleName { get; set; }
        [Required][StringLength(150)] public string LastName { get; set; } = default!;
        [Required][StringLength(100)] public string Password { get; set; } = default!;
        public int RoleID { get; set; }
        public int BranchID { get; set; }
    }

    public class UpdateUserDto
    {
        [Required] public int ID { get; set; }
        [Required][StringLength(150)] public string FirstName { get; set; } = default!;
        [StringLength(150)] public string? MiddleName { get; set; }
        [Required][StringLength(150)] public string LastName { get; set; } = default!;
        public int RoleID { get; set; }
        public int BranchID { get; set; }
    }
}
