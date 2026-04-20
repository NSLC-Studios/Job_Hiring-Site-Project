using AvaloniaAdminInterface.Model;
using AvaloniaAdminInterface.ViewModels;
using JobHiringAPI.Dtos;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaAdminInterface.ViewModels
{
    public class CompanyViewModel : ViewModelBase
    {
        private readonly MainViewModel _parent;

        public Company Model { get; }

        public int ID => Model.ID;
        public int OwnerId => Model.OwnerID;
        public string CompanyName => Model.CompanyName;
        public string Description => Model.Description;

        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }

        public CompanyViewModel(Company model, MainViewModel parent)
        {
            Model = model;
            _parent = parent;

            DeleteCommand = ReactiveCommand.CreateFromTask(DeleteAsync);
        }

        private Task DeleteAsync()
        {
            return _parent.DeleteCompanyAsync(this);
        }


    }

}


