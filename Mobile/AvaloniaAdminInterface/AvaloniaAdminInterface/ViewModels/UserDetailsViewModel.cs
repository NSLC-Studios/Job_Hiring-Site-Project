using Avalonia.Controls.Documents;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.Model.Services;
using AvaloniaAdminInterface.ViewModels;
using DynamicData;
using JobHiringAPI.Dtos;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

public class UserDetailsViewModel : ViewModelBase
{
    private readonly TheModel _model;

    public int UserId { get; }

    public ObservableCollection<Company> Companies { get; } = new();
    public ObservableCollection<Request> Requests { get; } = new();

 
    public ReactiveCommand<Request, Unit> ChangeStatusCommand { get; }//put 1 reqest under review or approve it 
    public ReactiveCommand<Request, Unit> DeleteRequestCommand { get; }//to delete 1 reqest
    public ReactiveCommand<Company, Unit> DeleteCompanyCommand { get; }//to delete 1 company

    public ReactiveCommand<Unit, Unit> LoadCommand { get; } // load reqests and stuff 
    public ReactiveCommand<int, Unit> RequestsUnderReviewCommand { get; }//to put under all requests under review
    public ReactiveCommand<int, Unit> DeleteCompaniesCommand { get; } //to delete company and all jobs under it
    //oops forgot to add reset password and demote/promote feature
    public ReactiveCommand<Unit, Unit> ResetPasswordCommand { get; }
    public ReactiveCommand<Unit, Unit> PromoteUserCommand { get; }
    public ReactiveCommand<Unit, Unit> DemoteUserCommand { get; }



    private string? _userName;
    public string? UserName
    {
        get => _userName;
        set
        {
            if (_userName != value)
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
    }

    private string? _email;
    public string? Email
    {
        get => _email;
        set
        {
            if (_email != value)
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }

    private string? _phone;
    public string? Phone
    {
        get => _phone;
        set
        {
            if (_phone != value)
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
    }

    private string? _role;
    public string? Role
    {
        get => _role;
        set
        {
            if (_role != value)
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }
    }


    private string? _newPassword;
    public string? NewPassword
    {
        get => _newPassword;
        set
        {
            if (_newPassword != value)
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }
    }

    public Company? selectedCompany { get; set; }


    public UserDetailsViewModel(TheModel model, int userId)
    {
        _model = model;
        UserId = userId;

        ChangeStatusCommand = ReactiveCommand.CreateFromTask<Request>(ChangeStatusAsync);
        DeleteRequestCommand = ReactiveCommand.CreateFromTask<Request>(DeleteRequestAsync);
        DeleteCompanyCommand = ReactiveCommand.CreateFromTask<Company>(DeleteCompanyAsync);

        LoadCommand = ReactiveCommand.CreateFromTask(LoadAsync);
        //RequestsUnderReviewCommand = ReactiveCommand.CreateFromTask<int>(PutCompanyUnderReviewAsync);
        DeleteCompaniesCommand = ReactiveCommand.CreateFromTask<int>(DeleteCompanyByIdAsync);

        PromoteUserCommand = ReactiveCommand.CreateFromTask(PromoteUserAsync);
        DemoteUserCommand = ReactiveCommand.CreateFromTask(DemoteUserAsync);
        ResetPasswordCommand = ReactiveCommand.CreateFromTask(ResetPasswordAsync);


    }
    // private async Task LoadAsync()
    //changed visibility
    public async Task LoadAsync()
    {
        Companies.Clear();
        Requests.Clear();

        // Load user Details (the dto ones no first/lastnames)
        var user = await _model.GetUserExpandedInfo(UserId);
        if (user != null)
        {
            UserName = user.UserName;
            Email = user.Email;
            Phone = user.Phone;
            Role = user.Role;
        }
        

        // Load companies

        foreach (var c in await _model.GetCompaniesExtended(UserId))
        {
            Companies.Add(new Company
            {
                ID = c.ID,
                CompanyName = c.CompanyName,
                OwnerID = c.OwnerID,
                Description = c.Description
            });
        }

        // Load requests
        foreach (var r in await _model.GetRequestsByUserId(UserId))
        {
            Requests.Add(new Request(
                r.ID,
                r.JobID,
                r.Status,
                r.Description,
                r.Response,
                r.CompanyName,
                req => DeleteRequestCommand.Execute(req),
                req => ChangeStatusCommand.Execute(req)
            ));
        }
    }
    
    //requests
    private async Task ChangeStatusAsync(Request req)
    {
        await _model.PutUnderReview(req.ID);
        await LoadAsync();
    }
    
    private async Task DeleteRequestAsync(Request req)
    {
        await _model.DeleteRequest(req.ID);
        await LoadAsync();
    }
    // companies
    private async Task DeleteCompanyAsync(Company company)
    {
        await _model.DeleteCompany(selectedCompany.ID);
        await LoadAsync();
    }

    private async Task DeleteCompanyByIdAsync(int companyId)
    {
        await _model.DeleteCompany(companyId);
        await LoadAsync();
    }
    //User
    private async Task PromoteUserAsync()
    {
        if (Role.Contains("User"))
        {
            await _model.PromoteUser(UserId);
            await LoadAsync();

            OnPropertyChanged(nameof(Role));
        }
    }
    private async Task DemoteUserAsync()
    {
        if (!Role.Contains("User"))
        {
            await _model.DemoteUser(UserId);
            await LoadAsync();

            OnPropertyChanged(nameof(Role));
        }
            
    }

    private async Task ResetPasswordAsync()
    {
        await _model.ResetPassword(UserId);
        NewPassword = await _model.ResetPassword(UserId);
    }
 
}