using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using static AvaloniaAdminInterface.ViewModels.User;

namespace AvaloniaAdminInterface.ViewModels;

public class MainViewModel : ViewModelBase
{
  //  public string Greeting => "Welcome to Hell,it took 2 hourst to set up!";

    public ObservableCollection<User> Users { get; set; }
    Random rnd = new Random();//temporary
    private List<int> _UserId;
    private List<string>_UserName;
    private List<string> _UserEmail;//test List for deleteion
    private List<TheRoles> _UserRole;

    public int RemoveByUserId {  get; set; }
    public RelayCommand DeleteUser {  get; set; }
    public MainViewModel()
    {
        
        _UserId = [];
        _UserName = ["Jóska_Gyerek", "Róka_Rudi", "Bőzsi_Néni"];
        _UserEmail = [];
        _UserRole = [TheRoles.Admin,TheRoles.DefaultUser,TheRoles.Company];

        Users = new ObservableCollection<User>();
        DeleteUser = new RelayCommand(UpdateFunction);//kills entrys
        

        for (int i = 0; i < 10; i++)
        {
            int tempid = rnd.Next(10, 50);
            int tempname = rnd.Next(0, _UserName.Count);
            int phonenumber = rnd.Next(10000000,20999999);// 10 000 000 - 20 999 999

            TheRoles tempjob_title = _UserRole[rnd.Next(0, _UserRole.Count)];
            string funnyemailfiller = "";
            

            if (tempjob_title == TheRoles.DefaultUser) { funnyemailfiller = "Lost"; }
            else if (tempjob_title == TheRoles.Company) { funnyemailfiller = "BankruptCeo"; }
            else if (tempjob_title == TheRoles.Admin) { funnyemailfiller = "TheOnlyEmploye"; }
            else { funnyemailfiller = "Homless??? How!?"; }

            string fullandomemail = $"{_UserName[tempname]}-{funnyemailfiller}@gmail.com";

            User victims = new User(tempid, _UserName[tempname], _UserName[tempname].Split("_")[0], _UserName[tempname].Split("_")[1],fullandomemail,Convert.ToString( phonenumber),Convert.ToString(rnd.Next(100, 999)), tempjob_title);
            Users.Add(victims);
        }
    }

    private void UpdateFunction()
    {
        //OnPropertyChanged(nameof(RemoveByUserId)); //Apperently ObservableCollection already notifies the UI when items are added or removed.
        //Users.Remove(Users.Where(x => x.UserId == RemoveByUserId)); //aka i only need it when i replace the whole Users ...
        //OnPropertyChanged(nameof(Users));//save it 

      
        var user = Users.FirstOrDefault(x => x.UserId == RemoveByUserId);
        if (user != null)
            Users.Remove(user);
    }


}
/*
*  Oh God Its Almost 4am  
*  
*  Here Have this Trust whorty fox it will definetly fix the code :
*                       []
* [~>         <~]   []  []
* [  >-=====-<  ]   []  O
* [ <         > ]   O 
* [< -O ___ O-  >]
* <«~~«< T >»~~»>
*  <« <     > »> 
*   <« <   > »> 
*      
*/
