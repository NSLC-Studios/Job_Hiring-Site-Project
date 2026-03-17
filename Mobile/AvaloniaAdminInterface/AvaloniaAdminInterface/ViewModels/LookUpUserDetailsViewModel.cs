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

internal class LookUpUserDetailsViewModel : ViewModelBase
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


    public Func<Request, Task> DeleteThis { get; }
    public Func<Request, Task> ChangeStatus { get; }
    public Func<Company, Task> DeleteThisCompany { get; }

    public LookUpUserDetailsViewModel(TheModel model, int userId)
    {
        _model = model;
        UserId = userId;


        ChangeStatusCommand = ReactiveCommand.CreateFromTask<Request>(ChangeStatus);
        DeleteRequestCommand = ReactiveCommand.CreateFromTask<Request>(DeleteThis);
        DeleteCompanyCommand = ReactiveCommand.CreateFromTask<Company>(DeleteThisCompany);

        LoadCommand = ReactiveCommand.CreateFromTask(LoadAsync);
        RequestsUnderReviewCommand = ReactiveCommand.CreateFromTask<int>(PutCompanyUnderReviewAsync);
        DeleteCompaniesCommand = ReactiveCommand.CreateFromTask<int>(DeleteCompanyAsync);

    }
    async Task GetUserReqests(int id)
    {
        var requestList = await _model.GetRequestsByUserId(id);
        foreach (var dto in requestList)
        {
            Requests.Add(new Request(
               dto.ID,
               dto.JobID,
               dto.Status,
               dto.Description,
               dto.Response,
               dto.CompanyName,
               R => DeleteRequestCommand.Execute(R),
               R => ChangeStatusCommand.Execute(R)
            ));
        }

    }
    async Task GetUserownedCompanies(int id)
    {
    }
    private async Task LoadAsync()
    {
        Companies.Clear();
        Requests.Clear();

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

    private async Task PutCompanyUnderReviewAsync(int companyId)
    {
        // if (!await _dialog.Confirm("Confirm", "Put this company under review?"))
        // return;

        await _model.PutUnderReview(companyId);
        await LoadAsync();
    }

    private async Task DeleteCompanyAsync(int companyId)
    {
        // if (!await _dialog.Confirm("Delete", "Delete this company?"))
        //  return;

        await _model.DeleteCompany(companyId);
        await LoadAsync();
    }

    private async Task PutJobUnderReviewAsync(int jobId)
    {
        //  if (!await _dialog.Confirm("Confirm", "Put this job under review?"))
        //   return;

        await _model.PutUnderReview(jobId);
        await LoadAsync();
    }
   
}
