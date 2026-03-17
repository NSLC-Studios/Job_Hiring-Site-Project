using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.Model.Services;
using AvaloniaAdminInterface.ViewModels;
using DynamicData;
using JobHiringAPI.Dtos;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

internal class LookUpUserDetailsViewModel : ViewModelBase
{
    private readonly TheModel _model;

    public int UserId { get; }

    public ObservableCollection<BaseCompanyDto> Companies { get; } = new();
    public ObservableCollection<BaseRequestDto> Requests { get; } = new();

    public ReactiveCommand<Unit, Unit> LoadCommand { get; }
    public ReactiveCommand<int, Unit> PutCompanyUnderReviewCommand { get; }
    public ReactiveCommand<int, Unit> DeleteCompanyCommand { get; }
    public ReactiveCommand<int, Unit> PutJobUnderReviewCommand { get; }

    public LookUpUserDetailsViewModel(TheModel model, int userId)
    {
        _model = model;
        UserId = userId;

        LoadCommand = ReactiveCommand.CreateFromTask(LoadAsync);
        PutCompanyUnderReviewCommand = ReactiveCommand.CreateFromTask<int>(PutCompanyUnderReviewAsync);
        DeleteCompanyCommand = ReactiveCommand.CreateFromTask<int>(DeleteCompanyAsync);
        PutJobUnderReviewCommand = ReactiveCommand.CreateFromTask<int>(PutJobUnderReviewAsync);
    }

    private async Task LoadAsync()
    {
        Companies.Clear();
        Requests.Clear();

        foreach (var c in await _model.GetCompanies(UserId))
            Companies.Add(new BaseCompanyDto { ID = c.ID,CompanyName = c.Name,OwnerID = c.OwnerID});
 
        foreach (var r in await _model.GetRequestsByUserId(UserId))
            Requests.Add(new BaseRequestDto { ID = r.ID,JobID = r.OwnerID});
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
