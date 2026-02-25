using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AvaloniaAdminInterface.ViewModels.User;

namespace AvaloniaAdminInterface.ViewModels
{
    public class UserDetailsViewModel : ViewModelBase
    {
        public User User { get; }

        public UserDetailsViewModel(User user)
        {
            User = user;
        }

        public string UserName => User.UserName;
        public string Email => User.Email;
        public string PhoneNumber => User.PhoneNumber;
        public TheRoles Role => User.Role;
    }

}
