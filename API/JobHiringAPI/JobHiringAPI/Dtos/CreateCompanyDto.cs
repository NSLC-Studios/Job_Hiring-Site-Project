using JobHiringAPI.Persistence;
using System.ComponentModel.DataAnnotations;

namespace JobHiringAPI.Dtos
{
    public class CreateCompanyDto
    {
        public string CompanyName { get; set; }
        public string? CompanyEmail { get; set; }
        public string? CompanyPhone { get; set; }
        public string OwnerName { get; set; }
    }
}
