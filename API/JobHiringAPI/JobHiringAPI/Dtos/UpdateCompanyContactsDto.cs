namespace JobHiringAPI.Dtos
{
    public class UpdateCompanyContactsDto
    {
        public int OwnerId { get; set; }
        public int CompanyId { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
