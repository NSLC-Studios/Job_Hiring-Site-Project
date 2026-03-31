using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.ViewModels;
using JobHiringAPI.Dtos;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaAdminInterface.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private readonly MainViewModel _parent;

        public User Model { get; }

        public int UserId => Model.UserId;
        public string UserName => Model.UserName;

        // UI-friendly string
        public string Role => Model.Role.ToString();

        // Logic-friendly enum
        public User.TheRoles RoleEnum => Model.Role;
        //public User.TheRoles Role => Model.Role;

        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }
        public ReactiveCommand<Unit, Unit> ExpandCommand { get; }

        public UserViewModel(User model, MainViewModel parent)
        {
            Model = model;
            _parent = parent;



            DeleteCommand = ReactiveCommand.CreateFromTask(
                () => _parent.DeleteUserAsync(this)
            );

            ExpandCommand = ReactiveCommand.Create(
                () => _parent.ExpandUser(this)
            );
        }
    }

}


