namespace SchoolProcurement.Domain.Entities
{
    public class Branch
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public decimal Budget { get; set; } = 0m;
        public bool IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Navigation
        //public ICollection<User> Users { get; set; }
    }
}
