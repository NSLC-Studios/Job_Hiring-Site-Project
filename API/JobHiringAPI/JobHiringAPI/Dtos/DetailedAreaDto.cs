namespace JobHiringAPI.Dtos
{
    public class DetailedAreaDto
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string Postal { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}