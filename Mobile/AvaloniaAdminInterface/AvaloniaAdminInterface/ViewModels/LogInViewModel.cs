using Avalonia.Markup.Xaml.Templates;
using AvaloniaAdminInterface.Model;
using CommunityToolkit.Mvvm.Input;
using JobHiringAPI.Dtos;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace AvaloniaAdminInterface.ViewModels
{

    public class LogInViewModel : ViewModelBase
    {
        private readonly TheModel _model;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }
        private string _username;

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }
        private string _password;

        public string ErrorMessage
        {
            get => _errorMessage;
            private set { _errorMessage = value; OnPropertyChanged(); }
        }
        private string _errorMessage;

        public bool IsBusy
        {
            get => _isBusy;
            private set { _isBusy = value; OnPropertyChanged(); }
        }
        private bool _isBusy;

        public ICommand LoginCommand { get; }

        public event Action<UserLoginDto> LoginSucceeded;

        public LogInViewModel(TheModel model)
        {
            _model = model;
            LoginCommand = new RelayCommand(async () => await ExecuteLoginAsync());
        }

        private async Task ExecuteLoginAsync()
        {
            IsBusy = true;
            ErrorMessage = "";

            try
            {
                // DEVELOPMENT BYPASS
                if (Username == "admin" && Password == "0000")
                {
                    LoginSucceeded?.Invoke(new UserLoginDto
                    {
                        UserID = 99,
                        UserName = "DeveloperAdmin",
                        Role = "Admin"
                    });
                    return;
                }

                // REAL API LOGIN
                var user = await _model.Log_in(Username, Password);

                if (user == null)
                {
                    ErrorMessage = "Login failed.";
                    return;
                }

                if (!string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    ErrorMessage = "Access denied.";
                    return;
                }

                LoginSucceeded?.Invoke(user);
            }
            catch
            {
                ErrorMessage = "Server error.";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

    /*
     //before api connection
    public class LogInViewModel : ViewModelBase
    {
        readonly TheModel _model;
       
        private readonly IAuthService _authService;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        public event Action LoginSucceeded;

        public LogInViewModel(IAuthService authService,TheModel model)
        {
            _authService = authService;
            _model = model;

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
    */

