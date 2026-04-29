namespace JobHiringAPI.Dtos
{
    public class DetailedRequestDto
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int JobID { get; set; }
        public int CVID { get; set; }
        public int ApplicantID { get; set; }
        public string Applicant { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string Response { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string WorkTime { get; set; }
        public int Pay { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public string Address { get; set; }
    }
}