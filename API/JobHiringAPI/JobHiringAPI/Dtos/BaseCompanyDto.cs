namespace JobHiringAPI.Dtos
{
    public class BaseCompanyDto
    {
        public int ID { get; set; }
        public int OwnerID { get; set; }
        public string OwnerName { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
    }
}