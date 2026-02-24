using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaAdminInterface.ViewModels
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

        public User(int id,
            string username,
            string firstname,
             string lastname,
             string email,
             string phone,
             string pass,
             TheRoles role
            )
        {
            UserId = id;
            UserName = username;
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            PhoneNumber = phone;
            Password = pass;
            Role = role;

        }

    }
}
