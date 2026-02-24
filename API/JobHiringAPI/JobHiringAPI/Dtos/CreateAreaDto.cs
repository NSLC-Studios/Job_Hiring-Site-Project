namespace JobHiringAPI.Dtos
{
    public class CreateAreaDto
    {
        public int InitiatorID { get; set; }
        public string HolderType { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
    }
}