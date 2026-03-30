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

public class LookUpUserDetailsViewModel : ViewModelBase
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
        set => this.RaiseAndSetIfChanged(ref _userName, value);
    }

    private string? _email;
    public string? Email
    {
        get => _email;
        set => this.RaiseAndSetIfChanged(ref _email, value);
    }

    private string? _phone;
    public string? Phone
    {
        get => _phone;
        set => this.RaiseAndSetIfChanged(ref _phone, value);
    }

    private string? _role;
    public string? Role
    {
        get => _role;
        set => this.RaiseAndSetIfChanged(ref _role, value);
    }

    private string? _newPassword;
    public string? NewPassword
    {
        get => _newPassword;
        set => this.RaiseAndSetIfChanged(ref _newPassword, value);
    }


    public LookUpUserDetailsViewModel(TheModel model, int userId)
    {
        _model = model;
        UserId = userId;

        ChangeStatusCommand = ReactiveCommand.CreateFromTask<Request>(ChangeStatusAsync);
        DeleteRequestCommand = ReactiveCommand.CreateFromTask<Request>(DeleteRequestAsync);
        DeleteCompanyCommand = ReactiveCommand.CreateFromTask<Company>(DeleteCompanyAsync);

        LoadCommand = ReactiveCommand.CreateFromTask(LoadAsync);
        RequestsUnderReviewCommand = ReactiveCommand.CreateFromTask<int>(PutCompanyUnderReviewAsync);
        DeleteCompaniesCommand = ReactiveCommand.CreateFromTask<int>(DeleteCompanyByIdAsync);

        PromoteUserCommand = ReactiveCommand.CreateFromTask(PromoteUserAsync);
        DemoteUserCommand = ReactiveCommand.CreateFromTask(DemoteUserAsync);
        ResetPasswordCommand = ReactiveCommand.CreateFromTask(ResetPasswordAsync);


    }

    private async Task LoadAsync()
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
    
    private async Task GetMoreDetails(int id)
    {
        await _model.GetUserExpandedInfo(id);
    }

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

    private async Task DeleteCompanyAsync(Company company)
    {
        await _model.DeleteCompany(company.ID);
        await LoadAsync();
    }

    private async Task PutCompanyUnderReviewAsync(int companyId)
    {
        await _model.PutUnderReview(companyId);
        await LoadAsync();
    }

    private async Task DeleteCompanyByIdAsync(int companyId)
    {
        await _model.DeleteCompany(companyId);
        await LoadAsync();
    }
    private async Task PromoteUserAsync()
    {
        await _model.PromoteUser(UserId);
        await LoadAsync();
    }
    private async Task DemoteUserAsync()
    {
        await _model.DemoteUser(UserId);
        await LoadAsync();
    }
    private async Task ResetPasswordAsync()
    {
        await _model.ResetPassword(UserId);
        NewPassword = await _model.ResetPassword(UserId);
    }
 
}