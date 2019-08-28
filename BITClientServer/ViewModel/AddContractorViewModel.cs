using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITClientServer.Commands;
using BITClientServer.Model;
using BITClientServer.View;
using Org.BouncyCastle.Asn1;

namespace BITClientServer.ViewModel
{
    class AddContractorViewModel : NotificationClass
    {
        #region Variables

        public LocationViewModel LocationViewModel { get; set; } = new LocationViewModel();

        public JobCategoryViewModel JobCatViewModel { get; set; } = new JobCategoryViewModel();

        public ObservableCollection<char> GenderCollection { get; set; } = GenderList.GenderCollection;

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

        public RelayCommand<Window> AddCommand => new RelayCommand<Window>(AddContractor, true);

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

        public AddContractorViewModel()
        {
            SelectedContractor = new Contractor();

        }

        #endregion

        #region Methods

        private void AddContractor(Window window)
        {

            if (Validation())
            {
                return;
            }
            int contractorId = SelectedContractor.Add();
            if (contractorId == 0)
            {
                return;
            }
            if (!LocationViewModel.AddContractorLocation(contractorId) || !JobCatViewModel.AddContractorSkill(contractorId))
            {
                MessageBox.Show("Something went wrong. Please try again later. Could not add contractor's location or skill.", "Unsuccessful",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (contractorId != 0)
                window?.Close();

        }

        #endregion

    }


}
