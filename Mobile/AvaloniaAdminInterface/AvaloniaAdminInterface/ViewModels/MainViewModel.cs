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



    public ReactiveCommand<Unit, Unit> LaoadCompaniesCommand { get; }
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

        LookUpCompanyCommand = ReactiveCommand.Create(LookUpCompany);
        LaoadCompaniesCommand = ReactiveCommand.CreateFromTask(LoadCompaniesAsync);

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
    /*
    public async Task DeleteCompanyAsync(CompanyViewModel cvm)
    {
        await _model.DeleteCompany(cvm.Model.ID);
        Companies.Remove(cvm);
    }*/
    public async Task DeleteCompanyAsync(CompanyViewModel compvm)
    {
        if (compvm == null)
            return;

        await _model.DeleteCompany(compvm.Model.ID);
        Companies.Remove(compvm);
    }

    public void ExpandUser(UserViewModel vm)
    {
        var detailsVm = new UserDetailsViewModel(_model, vm.Model.UserId);

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
        var dtos = await _model.GetAllCompanies(Start, End);

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

    private void LookUpCompany() 
    {
        SelectedCompanyList.Clear();
        if (int.TryParse(SearchByCompanyId, out int id))
        {
            var found = Companies.Where(x => x.ID == id).FirstOrDefault();
            if (found != null)
            {
                SelectedCompanyList.Add(found);
            }

        }
    }


    /*static User.TheRoles ConvertRole(string role) => role switch
    {
        "Admin" => User.TheRoles.Admin,
        //"Company" => User.TheRoles.Company,
        _ => User.TheRoles.DefaultUser
    };*/
}





/* Old one 

//  public string Greeting => "Welcome to Hell,it took 2 hourst to set up!";
TheModel _model;
public ObservableCollection<User> Users { get; set; }
Random rnd = new Random();//temporary
private List<int> _UserId;
private List<string> _UserName;
private List<string> _UserEmail;//test List for deleteion
private List<string> _comapny_names;// test list for next tab
private List<TheRoles> _UserRole;

private int _searchByUserId { get; set; }
public int SearchByUserId
{
    get { return _searchByUserId; }
    set

    {
        if (_searchByUserId != value)
        {
            _searchByUserId = value;
            OnPropertyChanged(nameof(SearchByUserId));
        }
    }

}
public ICommand ExpandCommand { get; }



public ObservableCollection<User> SelectedUserList { get; } = new();
public ICommand DeleteUser { get; set; }
public ICommand LookUpUserCommand { get; set; }


public MainViewModel(TheModel model)
{
    _model = model;

    _UserId = [];
    _UserName = ["Jóska_Gyerek", "Róka_Rudi", "Bőzsi_Néni"];
    _UserEmail = [];
    _UserRole = [TheRoles.Admin, TheRoles.DefaultUser, TheRoles.Company];
    _comapny_names = ["Google", "Apple", "Microsoft", "Amazon", "Facebook"];
    Users = new ObservableCollection<User>();
    //DeleteUser = new RelayCommand(UpdateFunction);//kills entrys
    DeleteUser = new RelayCommand(DeleteUserById);
    LookUpUserCommand = new RelayCommand(GetUserById);


    for (int i = 0; i < 10; i++)
    {
        int phonenumtype = rnd.Next(0, 3);
        int tempid = rnd.Next(10, 50);
        if (_UserId.Contains(tempid))
        {
            tempid = rnd.Next(10, 50);
        }
        int tempname = rnd.Next(0, _UserName.Count);
        int phonenumber = 0;
        if (phonenumtype == 1)
        {
            phonenumber = rnd.Next(200000000, 210000000);// 20 000 0000 - 20 999 9999
        }
        else if (phonenumtype == 0)
        {
            phonenumber = rnd.Next(300000000, 310000000);// 30 000 0000 - 30 999 9999
        }
        else
        {
            phonenumber = rnd.Next(700000000, 710000000);// 70 000 0000 - 70 999 9999 
        }


        TheRoles tempjob_title = _UserRole[rnd.Next(0, _UserRole.Count)];
        string funnyemailfiller = "";


        if (tempjob_title == TheRoles.DefaultUser) { funnyemailfiller = "Lost"; }
        else if (tempjob_title == TheRoles.Company) { funnyemailfiller = "BankruptCeo"; }
        else if (tempjob_title == TheRoles.Admin) { funnyemailfiller = "TheOnlyEmploye"; }
        else { funnyemailfiller = "Homless??? How!?"; }

        string fullandomemail = $"{_UserName[tempname]}-{funnyemailfiller}@gmail.com";

        User victims = new User(tempid, _UserName[tempname], _UserName[tempname].Split("_")[0], _UserName[tempname].Split("_")[1], fullandomemail, Convert.ToString(phonenumber), Convert.ToString(rnd.Next(100, 999)), tempjob_title,"",1,
        DeleteUserInstance,ExpandUserInstance);
        Users.Add(victims);
    }
}
// private void OpenDetailsWindow() { var vm = new UserDetailsViewModel(User); var window = new UserDetailsWindow { DataContext = vm }; window.Show(); }

//private void DeleteUserInstance(User user)
//{
//    Users.Remove(user);
//}

private async void DeleteUserInstance(User user)
{
    try
    {
        await _model.DeleteUser(user.UserId); // API call
        Users.Remove(user);                   // UI update
    }
    catch
    {
        ErrorMessage = "Failed to delete user";
        this.RaisePropertyChanged(nameof(ErrorMessage));
    }
}

private void ExpandUserInstance(User user)
{ 
    Console.WriteLine($"Expanding user {user.UserId}");
}

private void GetUserById()
{
    SelectedUserList.Clear();
    var user = Users.FirstOrDefault(x => x.UserId == SearchByUserId);
    if (user != null)
        SelectedUserList.Add(user);
}
private void DeleteUserById()
{
    var user = Users.FirstOrDefault(x => x.UserId == _searchByUserId);
    if (user != null)
        Users.Remove(user);
}
private void ResetPassword()
{
    var user = Users.FirstOrDefault(x => x.UserId == _searchByUserId);
    if (user != null)
        Users.Replace(user, new User(user.UserId, user.UserName, user.FirstName, user.LastName, user.Email, user.PhoneNumber, "Pass", user.Role,"",1, DeleteUserInstance, ExpandUserInstance));
}
}*/