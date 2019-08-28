using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITClientServer.Commands;
using BITClientServer.Model;
using BITClientServer.View;
using BITClientServer.View.Client;
using BITClientServer.View.Job;

namespace BITClientServer.ViewModel
{
    class JobViewModel : NotificationClass
    {
        
        public ObservableCollection<Job> JobCollection { get; } = new ObservableCollection<Job>();

        public Dictionary<string, string> SearchFields { get; set; } = new Dictionary<string, string>()
        {
            {"Job ID", "jobid"},
            {"Company ID", "companyid"},
            {"Client Id", "clientid"},
            {"Contractor Id", "contractorid"}
        };

        public string SelectedField { get; set; } = "jobid";

        public string SearchString { get; set; } = "Enter a search term here";

        private Job _job;
        public Job SelectedJob
        {
            get => _job;
            set
            {
                _job = value;
                OnPropertyChanged("SelectedJob");
            }
        }

        public JobViewModel()
        {
            ReadAllJobs();
        }

        public RelayCommand ShowJobCommand => new RelayCommand(ShowJob, true);
        public RelayCommand RefreshCommand => new RelayCommand(ReadAllJobs, true);
        public RelayCommand SearchCommand => new RelayCommand(Search, true);

        private void ReadAllJobs()
        {
            DAL dal = new DAL();
            string queryString = "select * from job where active = 1";
            DataTable dt = dal.ReadRecords(queryString);
            JobCollection.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Job job = new Job(dr);
                JobCollection.Add(job);
            }
        }

        private void ShowJob()
        {
            if (SelectedJob == null)
            {
                return;
            }
            UpdateJob uj = new UpdateJob(SelectedJob);
            uj.ShowDialog();
            ReadAllJobs();
        }

        private void Search()
        {
            DAL dal = new DAL();
            //string queryString = "select * from job where active = 1 and " + SelectedField + " LIKE '%" + SearchString + "%';";
            string queryString = "select * from job where active = 1 and " + SelectedField + " = '" + SearchString + "';";
            DataTable dt = dal.ReadRecords(queryString);
            JobCollection.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Job job = new Job(dr);
                JobCollection.Add(job);
            }
        }
    }
}
