using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITClientServer.Commands;
using BITClientServer.Model;
using BITClientServer.View;

namespace BITClientServer.ViewModel
{
    class JobCategoryManagerViewModel : NotificationClass
    {
        // For Admin Actions Tab - Job Category CRUD

        private JobCategory _selectedJobCategory = new JobCategory();

        public JobCategory SelectedJobCategory
        {
            get => _selectedJobCategory;
            set
            {
                _selectedJobCategory = value;
                OnPropertyChanged("SelectedJobCategory");
            }
        }

        private JobCategory _changeJobCategory = new JobCategory();

        public JobCategory ChangeJobCategory
        {
            get => _changeJobCategory;
            set
            {
                _changeJobCategory = value;
                OnPropertyChanged("ChangeJobCategory");
            }
        }

        public ObservableCollection<JobCategory> JobCategoryCollection { get; set; } = new ObservableCollection<JobCategory>();

        public RelayCommand AddCommand => new RelayCommand(Add, true);
        public RelayCommand UpdateCommand => new RelayCommand(Update, true);
        public RelayCommand DeleteCommand => new RelayCommand(Delete, true);


        public JobCategoryManagerViewModel()
        {
            ReadAllJobCategories();
        }

        private void Add()
        {
            if (SelectedJobCategory == null)
            {
                SelectedJobCategory = new JobCategory();
            }
            SelectedJobCategory.SetJobCategory(ChangeJobCategory);
            SelectedJobCategory.NullJobCategoryId();
            SelectedJobCategory.Add();
            ReadAllJobCategories();
        }

        private void Update()
        {
            SelectedJobCategory.SetJobCategory(ChangeJobCategory);
            SelectedJobCategory.Update();
            ReadAllJobCategories();
        }

        private void Delete()
        {
            if (SelectedJobCategory == null)
            {
                return;
            }
            SelectedJobCategory.SetJobCategory(ChangeJobCategory);
            SelectedJobCategory.Delete();
            ReadAllJobCategories();
        }


        private void ReadAllJobCategories()
        {
            DAL dal = new DAL();
            string queryString = "select * from JobCategory";
            DataTable dt = dal.ReadRecords(queryString);
            JobCategoryCollection.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                JobCategory JobCategory = new JobCategory(dr);
                JobCategoryCollection.Add(JobCategory);
            }
        }
    }
}
