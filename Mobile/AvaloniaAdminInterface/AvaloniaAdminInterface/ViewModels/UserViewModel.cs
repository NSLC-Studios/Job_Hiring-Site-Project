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

        public User Model { get; }

        public string UserName
        {
            get => Model.UserName;
            set
            {
                Model.UserName = value;
                this.RaisePropertyChanged(nameof(UserName)); // or RaiseAndSetIfChanged with a backing field if you want
            }
        }

        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }
        public ReactiveCommand<Unit, Unit> ExpandCommand { get; }

        public UserViewModel(User model, MainViewModel parent)
        {
            Model = model;

            DeleteCommand = ReactiveCommand.CreateFromTask(
                () => parent.DeleteUserAsync(this)
            );

            ExpandCommand = ReactiveCommand.Create(
                () => parent.ExpandUser(this)
            );
        }



    }
}
