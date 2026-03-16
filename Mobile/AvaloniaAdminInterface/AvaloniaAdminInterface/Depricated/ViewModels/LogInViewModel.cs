using Avalonia.Markup.Xaml.Templates;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.Model.Services;
using JobHiringAPI.Dtos;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;


namespace AvaloniaAdminInterface.ViewModels
{
    public class LogInViewModel : ViewModelBase
    {
        private readonly TheModel _model;
        private readonly INavigationService _nav;

        public LogInViewModel(TheModel model, INavigationService nav)
        {
            _model = model;
            _nav = nav;

            LoginCommand = ReactiveCommand.CreateFromTask(ExecuteLoginAsync);
        }

        private async Task ExecuteLoginAsync()
        {
            IsBusy = true;
            RaisePropertyChanged(nameof(IsBusy));

            try
            {
                var user = await _model.Log_in(Username, Password);

                if (user == null)
                {
                    SetError("Login failed.");
                    return;
                }

                if (!string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    SetError("Access denied. Admin role required.");
                    return;
                }

                // SUCCESS → go to MainViewModel
                _nav.NavigateTo(new MainViewModel(_model, new DialogService(), _nav));
            }
            catch
            {
                SetError("Server error.");
            }
            finally
            {
                IsBusy = false;
                RaisePropertyChanged(nameof(IsBusy));
            }
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

