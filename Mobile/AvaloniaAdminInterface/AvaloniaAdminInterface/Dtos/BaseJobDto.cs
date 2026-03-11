namespace JobHiringAPI.Dtos
{
    public class BaseJobDto
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int? Pay { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string Language { get; set; }
        public string WorkTime { get; set; }
        public string Description { get; set; }
    }
}