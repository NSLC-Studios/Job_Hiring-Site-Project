using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaAdminInterface.Model
{


    public class Request
    {
        public int ID { get; }
        public int JobID { get; }
        public string Status { get; }
        public string Description { get; }
        public string Response { get; }
        public string CompanyName { get; }

        public ICommand DeleteThis { get; }
        public ICommand ChangeStatus { get; }
        public Func<object, IObservable<Unit>> Value1 { get; }
        public Action<Request> Value2 { get; }

        public Request(
            int id,
            int jobId,
            string status,
            string description,
            string response,
            string companyName,
            Action<Request> deleteAction,
            Action<Request> changeStatusAction)
        {
            ID = id;
            JobID = jobId;
            Status = status;
            Description = description;
            Response = response;
            CompanyName = companyName;

            DeleteThis = new RelayCommand(() => deleteAction(this));
            ChangeStatus = new RelayCommand(() => changeStatusAction(this));
        }
    }
}