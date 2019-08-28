using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BITClientServer.Commands;
using BITClientServer.Model;
using BITClientServer.View;

namespace BITClientServer.ViewModel
{
    class StatusViewModel : NotificationClass
    {
        private Status _status = new Status();
        public Status SelectedStatus
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("SelectedStatus");
            }

        }

        public ObservableCollection<Status> StatusCollection { get; set; }= new ObservableCollection<Status>();

        public StatusViewModel()
        {
            GetAllStatus();
        }

        public StatusViewModel(Status status)
        {
            GetAllStatus();
            foreach (Status s in StatusCollection)
            {
                if (s.StatusId == status.StatusId)
                {
                    this.SelectedStatus = s;
                }
            }
        }

        private void GetAllStatus()
        {
            DAL dal = new DAL();
            string queryString = "select * from Status";
            DataTable dt = dal.ReadRecords(queryString);
            StatusCollection.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Status status = new Status(Convert.ToInt32(dr["statusid"]));
                StatusCollection.Add(status);
            }
            
        }

    }
}
