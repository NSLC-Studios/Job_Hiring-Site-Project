using JobHiringAPI.Persistence;
using System.ComponentModel.DataAnnotations;

namespace JobHiringAPI.Dtos
{
    public class CreateCompanyDto
    {
        public int OwnerID { get; set; }
        public string CompanyName { get; set; }
    }
}
