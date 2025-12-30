namespace SchoolProcurement.Domain.Entities
{
    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation
        public ICollection<User> Users { get; set; }
    }
}
