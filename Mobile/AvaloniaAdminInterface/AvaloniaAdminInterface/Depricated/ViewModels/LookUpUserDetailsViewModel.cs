using AvaloniaAdminInterface.Model;
using JobHiringAPI.Dtos;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaAdminInterface.ViewModels
{
    internal class LookUpUserDetailsViewModel : ViewModelBase
    {
        readonly TheModel _model;
        readonly DialogServiceInterface _dialog;
        public int UserId { get; }
        public ObservableCollection<BaseCompanyDto> Companies { get; } = new();
        public ObservableCollection<BaseJobDto> Jobs { get; } = new();
        public ObservableCollection<BaseRequestDto> Requests { get; } = new();

        public ReactiveCommand<Unit, Unit> LoadCommand { get; }
        public ReactiveCommand<int, Unit> PutCompanyUnderReviewCommand { get; }
        public ReactiveCommand<int, Unit> DeleteCompanyCommand { get; }
        public ReactiveCommand<int, Unit> PutJobUnderReviewCommand { get; }
        public ReactiveCommand<Unit, Unit> PutAllCompaniesUnderReviewCommand { get; }
        public ReactiveCommand<Unit, Unit> PutAllJobsUnderReviewCommand { get; }

        public LookUpUserDetailsViewModel(TheModel model, DialogServiceInterface dialog, int userId)
        {
            _model = model; 
            _dialog = dialog; 
            UserId = userId;
            LoadCommand = ReactiveCommand.CreateFromTask(LoadAsync);
            PutCompanyUnderReviewCommand = ReactiveCommand.CreateFromTask<int>(PutCompanyUnderReviewAsync);
            DeleteCompanyCommand = ReactiveCommand.CreateFromTask<int>(DeleteCompanyAsync);
            PutJobUnderReviewCommand = ReactiveCommand.CreateFromTask<int>(PutJobUnderReviewAsync);
            PutAllCompaniesUnderReviewCommand = ReactiveCommand.CreateFromTask(PutAllCompaniesUnderReviewAsync);
            PutAllJobsUnderReviewCommand = ReactiveCommand.CreateFromTask(PutAllJobsUnderReviewAsync);
        }

        async Task LoadAsync()
        {
            Companies.Clear(); Jobs.Clear(); Requests.Clear();
            var comps = await _model.GetCompanies(UserId);
            foreach (var c in comps) Companies.Add(c);
            var jobs = await _model.GetJobsByUser(UserId);
            foreach (var j in jobs) Jobs.Add(j);
            var reqs = await _model.GetRequestsByUser(UserId);
            foreach (var r in reqs) Requests.Add(r);
        }

        async Task PutCompanyUnderReviewAsync(int companyId)
        {
            if (!await _dialog.Confirm("Confirm", "Put this company under review?")) return;
            try { await _model.PutCompanyUnderReview(companyId); await _dialog.ShowMessage("Success", "Done"); await LoadAsync(); }
            catch { await _dialog.ShowMessage("Error", "Failed"); }
        }

        async Task DeleteCompanyAsync(int companyId)
        {
            if (!await _dialog.Confirm("Delete company", "Permanently delete this company?")) return;
            try { await _model.DeleteCompany(companyId); await _dialog.ShowMessage("Success", "Deleted"); await LoadAsync(); }
            catch { await _dialog.ShowMessage("Error", "Failed"); }
        }

        async Task PutAllCompaniesUnderReviewAsync()
        {
            if (!await _dialog.Confirm("Confirm", "Put all companies under review?")) return;
            try { await _model.PutAllCompaniesUnderReviewForUser(UserId); await _dialog.ShowMessage("Success", "Done"); await LoadAsync(); }
            catch { await _dialog.ShowMessage("Error", "Failed"); }
        }

        async Task PutAllJobsUnderReviewAsync()
        {
            if (!await _dialog.Confirm("Confirm", "Put all jobs under review?")) return;
            try { await _model.PutAllJobsUnderReviewForUser(UserId); await _dialog.ShowMessage("Success", "Done"); await LoadAsync(); }
            catch { await _dialog.ShowMessage("Error", "Failed"); }
        }
    }

}
