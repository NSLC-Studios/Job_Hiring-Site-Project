using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaAdminInterface.Model
{
    public class User
    {
        public enum TheRoles { Admin, DefaultUser, Company }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }

        public TheRoles Role { get; set; } //= TheRoles.DefaultUser; not sure i need it



    }

}