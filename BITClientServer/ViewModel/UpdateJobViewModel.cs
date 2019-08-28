using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITClientServer.Commands;
using BITClientServer.Model;
using BITClientServer.View.Job;

namespace BITClientServer.ViewModel
{
    class UpdateJobViewModel : NotificationClass
    {
        // Update and Delete Job

        public JobCategoryViewModel JobCat { get; set; }

        public StatusViewModel Status { get; set; }

        private string _endDateSet;

        public string EndDateSet
        {
            get => _endDateSet;
            set
            {
                _endDateSet = value;
                OnPropertyChanged("EndDateSet");
            }
        }

        private string _startDateSet;

        public string StartDateSet
        {
            get => _startDateSet;
            set
            {
                _startDateSet = value;
                OnPropertyChanged(StartDateSet);
            }
        }

        public string SelectedStartDate
        {
            get
            {
                if (SelectedJob.StringStartDate == "N/A")
                {
                    StartDateSet = "Start date is not set!";
                    return DateTime.Today.ToShortDateString();
                }
                StartDateSet = "Start date is set!";
                return SelectedJob.StartDate.ToShortDateString();


            }
            set
            {
                if (value.Length >= 9)
                {
                    SelectedJob.StartDate = DateTime.Parse(value);
                }
            }
        }

        public string SelectedEndDate
        {
            get
            {
                if (SelectedJob.StringPreferredEndDate == "N/A")
                {
                    EndDateSet = "Due date is not set!";
                    return DateTime.Today.ToShortDateString();
                }
                EndDateSet = "Due date is set!";
                return SelectedJob.PreferredEndDate.ToShortDateString();

            }
            set
            {
                if (value.Length >= 9)
                {
                    SelectedJob.PreferredEndDate = DateTime.Parse(value);
                }

            }
        }

        private Job _job = new Job();
        public Job SelectedJob
        {
            get => _job;
            set
            {
                _job = value;
                OnPropertyChanged("SelectedJob");
            }
        }

        public UpdateJobViewModel(Job job)
        {
            SelectedJob = job;
            JobCat = new JobCategoryViewModel(SelectedJob.JobCategory);
            Status = new StatusViewModel(SelectedJob.Status);

        }

        public RelayCommand<Window> UpdateJobCommand => new RelayCommand<Window>(UpdateJob, true);

        public RelayCommand UpdateStartDateCommand => new RelayCommand(UpdateStartDate, true);

        public RelayCommand UpdateDueDateCommand => new RelayCommand(UpdateDueDate, true);

        public RelayCommand AssignJobCommand => new RelayCommand(AssignJob, true);

        public RelayCommand<Window> DeleteJobCommand => new RelayCommand<Window>(DeleteJob, true);

        private void UpdateJob(Window window)
        {
            if (SelectedJob.Coordinator.CoordinatorId == 0)
            {
                SelectedJob.Coordinator = new Coordinator(Convert.ToInt32(Session.CoordinatorId));
            }
            SelectedJob.SetJobCategory(JobCat.SelectedJobCategory);
            SelectedJob.SetStatus(Status.SelectedStatus);
            SelectedJob.SetDates(SelectedStartDate, SelectedEndDate);
            if (SelectedJob.Update())
                window?.Close();
        }

        private void DeleteJob(Window window)
        {
            if (SelectedJob.Delete())
                window?.Close();
        }

        private void AssignJob()
        {
            if (SelectedJob.Status.StatusId < 2)
            {
                MessageBox.Show("Please assess the job before assigning a contractor.", "Job not assessed!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (SelectedJob.Status.StatusId > 5)
            {
                MessageBox.Show("The Job has been completed.", "Job completed!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            FindContractor fc = new FindContractor(SelectedJob);
            fc.ShowDialog();
        }

        private void UpdateStartDate()
        {
            if (SelectedJob.Update(SelectedStartDate, "startdate"))
                StartDateSet = "Start date is set!";
        }

        private void UpdateDueDate()
        {
            if (ValidationClass.CheckDate(DateTime.Parse(SelectedEndDate)))
            {
                MessageBox.Show("Something went wrong. Please try again later. You have selected an invalid date.", "Unsuccessful",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (SelectedJob.Update(SelectedEndDate, "preferredenddate"))
                EndDateSet = "Due date is set!";
        }
    }
}
