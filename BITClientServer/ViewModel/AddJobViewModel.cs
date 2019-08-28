using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BITClientServer.Commands;
using BITClientServer.Model;
using BITClientServer.View;

namespace BITClientServer.ViewModel
{
    class AddJobViewModel : NotificationClass
    {
        public JobCategoryViewModel JobCategory { get; set; } = new JobCategoryViewModel();

        private string _endDate = DateTime.Now.ToShortDateString();
        public string EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
            }
        }


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

        public AddJobViewModel(int companyid, int clientid, int coordinatorid)
        {
            SelectedJob = new Job(companyid, clientid, coordinatorid);
        }

        public RelayCommand<Window> AddJobCommand => new RelayCommand<Window>(AddJob, true);

        public void AddJob(Window window)
        {
            EndDate = DateConverter.ToSQL(EndDate);
            SelectedJob.Add(JobCategory.SelectedJobCategory, EndDate);
        }

    }
}
