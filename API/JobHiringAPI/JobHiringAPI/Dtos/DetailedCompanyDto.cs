namespace JobHiringAPI.Dtos
{
    public class DetailedCompanyDto
    {
        public int ID { get; set; }
        public int OwnerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string Postal { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}