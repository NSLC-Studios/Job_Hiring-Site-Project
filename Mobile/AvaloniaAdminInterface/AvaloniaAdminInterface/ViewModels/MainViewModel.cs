using Avalonia.Interactivity;
using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.Model.Services;
using AvaloniaAdminInterface.Views;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using JobHiringAPI.Dtos;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using static AvaloniaAdminInterface.Model.User;

namespace AvaloniaAdminInterface.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly TheModel _model;
    private readonly INavigationService _nav;

    public ObservableCollection<UserViewModel> Users { get; } = new();
    public ReactiveCommand<Unit, Unit> LoadUsersCommand { get; }
    public ReactiveCommand<Unit, Unit> LookUpUserCommand { get; }
    public ObservableCollection<UserViewModel> SelectedUserList { get; }



    public ReactiveCommand<Unit, Unit> LoadCompaniesCommand { get; }
    public ObservableCollection<CompanyViewModel> Companies { get; } = new();
    public ReactiveCommand<Unit, Unit> LookUpCompanyCommand { get; }
    public ObservableCollection<CompanyViewModel> SelectedCompanyList { get; }

    public ReactiveCommand<Unit, Unit> IncreaseRange { get; }
    public ReactiveCommand<Unit, Unit> DecreaseRange { get; }


    private string? _searchByUserId;
    public string? SearchByUserId
    {
        get => _searchByUserId;
        set
        {
            if (_searchByUserId != value)
            {
                _searchByUserId = value;
                OnPropertyChanged(nameof(SearchByUserId));
            }
        }
    }

    private string? _searchByCompanyId;
    public string? SearchByCompanyId
    {
        get => _searchByCompanyId;
        set
        {
            if (_searchByCompanyId != value)
            {
                _searchByCompanyId = value;
                OnPropertyChanged(nameof(SearchByCompanyId));
            }
        }
    }
    private UserViewModel? _selectedUser;
    public UserViewModel? SelectedUser
    {
        get => _selectedUser;
        set => this.RaiseAndSetIfChanged(ref _selectedUser, value);
    }

    public int Start { get; set; }
    public int End { get; set; }

    public MainViewModel(TheModel model, INavigationService nav)
    {
        _model = model;
        _nav = nav;
        Users = new ObservableCollection<UserViewModel>();

        SelectedUserList = new ObservableCollection<UserViewModel>();
        SelectedCompanyList = new ObservableCollection<CompanyViewModel>();


        LookUpUserCommand = ReactiveCommand.Create(LookUpUser);
        LoadUsersCommand = ReactiveCommand.CreateFromTask(LoadUsersAsync);

        //LookUpCompanyCommand = ReactiveCommand.Create(LookUpCompany);
        LoadCompaniesCommand = ReactiveCommand.CreateFromTask(LoadCompaniesAsync);

        LoadCompaniesCommand.Execute().Subscribe();


        IncreaseRange = ReactiveCommand.Create(() =>
        {
            Start += 100;
            End += 100;
        });

        DecreaseRange = ReactiveCommand.Create(() =>
        {
            if (Start >= 100)
            {
                Start -= 100;
                End -= 100;
            }
        });
    }

    public async Task DeleteUserAsync(UserViewModel vm)
    {
        await _model.DeleteUser(vm.Model.UserId);
        Users.Remove(vm);
    }
 
    public async Task DeleteCompanyAsync(CompanyViewModel compvm)
    {
        if (compvm == null)
            return;

        await _model.DeleteCompany(compvm.Model.ID);
        Companies.Remove(compvm);
    }

    public async Task ExpandUser(UserViewModel vm)
    {
        var detailsVm = new UserDetailsViewModel(_model, vm.Model.UserId);
        await detailsVm.LoadAsync();

        var window = new UserDetailsWindow
        {
            DataContext = detailsVm
        };

        window.Show();
    }
 


    async Task LoadUsersAsync()
    {
        var dtos = await _model.GetUsers();
        Users.Clear();

        foreach (var dto in dtos)
        {
            var user = new User
            {
                UserId = dto.ID,
                UserName = dto.UserName,
                //Role = ConvertRole(dto.Role)
                Role = dto.Role
            };

            Users.Add(new UserViewModel(user, this));
        }
    }
    //async Task LoadCompaniesAsync(int start,int end)
    async Task LoadCompaniesAsync()
    {
        try
        {
            int id = Convert.ToInt32(_searchByCompanyId);
            var dtos = await _model.GetCompaniesExtended(id);
            SelectedCompanyList.Clear();
            Companies.Clear();
            foreach (var dto in dtos)
            {
                Companies.Add(new CompanyViewModel(new Company
                {
                    ID = dto.ID,
                    OwnerID = dto.OwnerID,
                    OwnerName = dto.OwnerName,
                    CompanyName = dto.CompanyName,
                    Description = dto.Description
                }, this));
               SelectedCompanyList.Add(new CompanyViewModel(new Company
               {
                   ID = dto.ID,
                   OwnerID = dto.OwnerID,
                   OwnerName = dto.OwnerName,
                   CompanyName = dto.CompanyName,
                   Description = dto.Description
               }, this));
            }
            
        }
        catch { 
        }
        
    }



    private void LookUpUser()
    {

        SelectedUserList.Clear();

        if (int.TryParse(SearchByUserId, out int id))
        {
            var match = Users.Where(x => x.UserId == id).FirstOrDefault();
            if (match != null)
                SelectedUserList.Add(match);
        }
    }


    
}
