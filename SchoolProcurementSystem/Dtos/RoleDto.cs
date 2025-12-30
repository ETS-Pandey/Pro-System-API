using System.ComponentModel.DataAnnotations;

namespace SchoolProcurement.Api.Dtos
{
    public class RoleDto { public int ID { get; set; } public string Name { get; set; } = default!; public DateTime CreatedDate { get; set; } }
    public class CreateRoleDto { [Required][StringLength(100)] public string Name { get; set; } = default!; }
    public class UpdateRoleDto { [Required] public int ID { get; set; } [Required][StringLength(100)] public string Name { get; set; } = default!; }
}
