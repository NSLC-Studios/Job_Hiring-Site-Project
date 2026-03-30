using AvaloniaAdminInterface.Model;
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
        public User.TheRoles Role => Model.Role;

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

