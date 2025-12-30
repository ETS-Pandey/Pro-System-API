namespace SchoolProcurement.Api.Dtos
{
    public class SorAssignedUserDto
    {
        public int AssignmentID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? Note { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}
