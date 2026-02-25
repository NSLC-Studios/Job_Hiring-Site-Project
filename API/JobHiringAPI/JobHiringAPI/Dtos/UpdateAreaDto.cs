namespace JobHiringAPI.Dtos
{
    public class UpdateAreaDto
    {
        public int ID { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
    }
}