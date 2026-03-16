using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml.Templates;
using AvaloniaAdminInterface.Model;
using ReactiveUI;
using System.Net.Http;
using JobHiringAPI.Dtos;


namespace AvaloniaAdminInterface.ViewModels
{
    
    public class LogInViewModel : ViewModelBase
    {
        private readonly TheModel _model;

        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; private set; }
        public bool IsBusy { get; private set; }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        public event Action<UserLoginDto> LoginSucceeded;

        public LogInViewModel(AuthApi auth, TheModel model)
        {
            _model = model;

            LoginCommand = ReactiveCommand.CreateFromTask(ExecuteLoginAsync);
        }

        private async Task ExecuteLoginAsync()
        {
            IsBusy = true;
            this.RaisePropertyChanged(nameof(IsBusy));
            ErrorMessage = string.Empty;
            this.RaisePropertyChanged(nameof(ErrorMessage));

            try
            {
                var user = await _model.Log_in(Username, Password);

                // 1) API returned non-success or null
                if (user == null)
                {
                    SetError("Login failed. Server returned no data.");
                    return;
                }

                // 2) Role check
                if (!string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    SetError("Access denied. Admin role required.");
                    return;
                }

                // 3) Success → event to open MainViewModel
                LoginSucceeded?.Invoke(user);
            }
            catch (HttpRequestException)
            {
                SetError("Could not reach server. Check your connection or try again later.");
            }
            catch (Exception ex)
            {
                SetError($"Unexpected error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                this.RaisePropertyChanged(nameof(IsBusy));
            }
        }

        private void SetError(string message)
        {
            ErrorMessage = message;
            this.RaisePropertyChanged(nameof(ErrorMessage));
        }
    }
}
    /*
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

