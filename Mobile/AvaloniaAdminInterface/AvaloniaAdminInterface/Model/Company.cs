using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaAdminInterface.Model
{
    public class Company
    {
        public int ID { get; set; }
        public int OwnerID { get; set; }
        public string OwnerName { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        /*
        public ICommand DeleteThis { get; private set; }

        public Company() { }

        public void AttachDeleteAction(Action<Company> deleteAction)
        {
            DeleteThis = new RelayCommand(() => deleteAction(this));
        }*/
    }
}
