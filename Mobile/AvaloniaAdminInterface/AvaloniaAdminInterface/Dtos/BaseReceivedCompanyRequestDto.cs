namespace JobHiringAPI.Dtos
{
    public class BaseReceivedCompanyRequestDto
    {
        public int ID { get; set; }
        public int JobID { get; set; }
        public string Applicant { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
    }
}