using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml.Templates;
using AvaloniaAdminInterface.Model;
using ReactiveUI;

namespace AvaloniaAdminInterface.ViewModels
{
    public class LogInViewModel : ViewModelBase
    {
        readonly TheModel _model;
       
        private readonly IAuthService _authService;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        public event Action LoginSucceeded;

        public LogInViewModel(IAuthService authService)
        {
            _authService = authService;

            LoginCommand = ReactiveCommand.Create(ExecuteLogin);
        }

        private void ExecuteLogin()
        {
            if (_authService.Login(Username, Password))
            {
                LoginSucceeded?.Invoke();
            }
            else
            {
                ErrorMessage = "Invalid username or password";
                this.RaisePropertyChanged(nameof(ErrorMessage));
            }
        }
    }
    public interface IAuthService
    {
        bool Login(string username, string password);
    }

    public class AuthService : IAuthService
    {
        public bool Login(string username, string password)
        {
            //Template pass make it use [HttpPost("login")] and reconfigure everithing to run with database

            return username == "admin" && password == "0000";
        }
    }


}


