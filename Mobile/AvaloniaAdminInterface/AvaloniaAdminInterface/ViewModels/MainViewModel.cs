using static AvaloniaAdminInterface.ViewModels.User;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

namespace AvaloniaAdminInterface.ViewModels;

public class MainViewModel : ViewModelBase
{
    //  public string Greeting => "Welcome to Hell,it took 2 hourst to set up!";

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

   
    public MainViewModel()
    {

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

            User victims = new User(tempid, _UserName[tempname], _UserName[tempname].Split("_")[0], _UserName[tempname].Split("_")[1], fullandomemail, Convert.ToString(phonenumber), Convert.ToString(rnd.Next(100, 999)), tempjob_title,"",1 );
            Users.Add(victims);
        }
    }
   // private void OpenDetailsWindow() { var vm = new UserDetailsViewModel(User); var window = new UserDetailsWindow { DataContext = vm }; window.Show(); }


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
            Users.Replace(user, new User(user.UserId, user.UserName, user.FirstName, user.LastName, user.Email, user.PhoneNumber, "Pass", user.Role,"",1));
    }
}
//Deprecated
/*
    private ObservableCollection<User> _selectedByUserId { get; set; }
    public ObservableCollection<User> SelectedByUserId
    {
        get { return _selectedByUserId; }
        set

        {
            if (_selectedByUserId != value)
            {
                _selectedByUserId = value;
                OnPropertyChanged(nameof(SelectedByUserId));
            }
        }

    }*/
/*
  private int _removeByUserId { get; set; }
 public int RemoveByUserId { get { return _removeByUserId; } set 

     {
         if (_removeByUserId != value)
         {
             _removeByUserId = value;
             OnPropertyChanged(nameof(_removeByUserId));
         }
     } 

 }
 private void UpdateFunction()
 {
     //OnPropertyChanged(nameof(RemoveByUserId)); //Apperently ObservableCollection already notifies the UI when items are added or removed.
     //Users.Remove(Users.Where(x => x.UserId == RemoveByUserId)); //aka i only need it when i replace the whole Users ...
     //OnPropertyChanged(nameof(Users));//save it 
     var user = Users.FirstOrDefault(x => x.UserId == _removeByUserId);
     if (user != null)
         Users.Remove(user);
 }*/