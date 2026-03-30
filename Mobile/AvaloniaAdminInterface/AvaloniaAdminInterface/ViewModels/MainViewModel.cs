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

namespace AvaloniaAdminInterface.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly TheModel _model;
    private readonly INavigationService _nav;

    public ObservableCollection<UserViewModel> Users { get; } = new();

    public ReactiveCommand<Unit, Unit> LoadUsersCommand { get; }

    public UserViewModel? SelectedUser { get; set; }

    public string SearchByUserId { get; set; }
    public ReactiveCommand<Unit, Unit> LookUpUserCommand { get; }
    public ObservableCollection<UserViewModel> SelectedUserList { get; }

    public MainViewModel(TheModel model, INavigationService nav)
    {
        _model = model;
        _nav = nav;

        LoadUsersCommand = ReactiveCommand.CreateFromTask(LoadUsersAsync);
        //LoadUsersCommand.Execute().Subscribe();
    }

    public async Task DeleteUserAsync(UserViewModel vm)
    {
        await _model.DeleteUser(vm.Model.UserId);
        Users.Remove(vm);
    }
    public void ExpandUser(UserViewModel vm)
    {
        var detailsVm = new LookUpUserDetailsViewModel(_model, vm.Model.UserId);

        var window = new LookUpUserDetailsWindow
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
                Role = ConvertRole(dto.Role)
            };

            Users.Add(new UserViewModel(user, this));
        }
    }

    static User.TheRoles ConvertRole(string role) => role switch
    {
        "Admin" => User.TheRoles.Admin,
        "Company" => User.TheRoles.Company,
        _ => User.TheRoles.DefaultUser
    };
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