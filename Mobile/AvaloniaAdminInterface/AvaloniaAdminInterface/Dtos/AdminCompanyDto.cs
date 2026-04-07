namespace JobHiringAPI.Dtos
{
    public class AdminCompanyDto
    {
        public int ID { get; set; }
        public int OwnerID { get; set; }
        public string Name { get; set; }
        public string Description { get; internal set; }
    }
}