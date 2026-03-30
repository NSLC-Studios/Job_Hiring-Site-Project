using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaAdminInterface.Dtos
{
    public class ExtendedUserDto
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Role { get; set; }
    }
}
