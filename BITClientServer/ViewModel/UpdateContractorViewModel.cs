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
using BITClientServer.View.Contractor;

namespace BITClientServer.ViewModel
{
    class UpdateContractorViewModel : NotificationClass
    {
        // Update and Delete Contractor
        
        #region Variables

        public LocationViewModel LocationViewModel { get; set; } = new LocationViewModel();

        public ObservableCollection<char> GenderCollection { get; set; } = GenderList.GenderCollection;

        public JobCategoryViewModel JobCatViewModel { get; set; } = new JobCategoryViewModel();

        private Contractor _contractor;
        public Contractor SelectedContractor
        {
            get => _contractor;
            set
            {
                _contractor = value;
                OnPropertyChanged("SelectedContractor");
            }
        }

        #endregion

        #region Commands

        public RelayCommand<Window> UpdateCommand => new RelayCommand<Window>(UpdateContractor, true);

        public RelayCommand<Window> DeleteCommand => new RelayCommand<Window>(DeleteContractor, true);

        public RelayCommand AvailabilityCommand => new RelayCommand(ShowAvailability, true);

        #endregion

        #region Validation

        private bool Validation()
        {
            if (JobCatViewModel.PrefJobCategoryCollection.Count == 0)
            {
                MessageBox.Show("Please enter atleast one preferred job category.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            if (LocationViewModel.PrefLocationCollection.Count == 0)
            {
                MessageBox.Show("Please enter atleast one preferred location.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            return false;
        }

        #endregion Validation

        #region Constructor

        public UpdateContractorViewModel(Contractor contractor)
        {
            
            SelectedContractor = contractor;
            LocationViewModel.SetContractor(contractor);
            JobCatViewModel.SetContractor(contractor);

        }

        #endregion

        #region Methods

        private void UpdateContractor(Window window)
        {
            if (Validation())
            {
                return;
            }

            if (!LocationViewModel.UpdateContractorLocation() || !JobCatViewModel.UpdateContractorSkill())
            {
                MessageBox.Show("Something went wrong. Please try again later. Could not update contractor's location or skill." , "Unsuccessful",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (SelectedContractor.Update())
                window?.Close();
        }

        private void DeleteContractor(Window window)
        {
            if (SelectedContractor.Delete())
                window?.Close();
        }

        private void ShowAvailability()
        {
            AddAvailability aa = new AddAvailability(SelectedContractor);
            aa.ShowDialog();
        }

        #endregion
    }
}
