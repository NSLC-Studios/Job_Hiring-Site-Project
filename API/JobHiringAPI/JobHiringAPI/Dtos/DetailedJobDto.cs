namespace JobHiringAPI.Dtos
{
    public class DetailedJobDto
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int? Pay { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string Postal { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Language { get; set; }
        public string WorkTime { get; set; }
        public string Description { get; set; }
    }
}