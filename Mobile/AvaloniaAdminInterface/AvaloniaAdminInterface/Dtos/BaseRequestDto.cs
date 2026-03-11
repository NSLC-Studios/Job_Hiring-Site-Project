namespace JobHiringAPI.Dtos
{
    public class BaseRequestDto
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string Response { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
        public int JobID { get; set; }
    }
}