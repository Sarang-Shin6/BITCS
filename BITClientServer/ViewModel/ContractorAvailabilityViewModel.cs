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
    class ContractorAvailabilityViewModel : NotificationClass
    {
        public DateTime today = DateTime.Today;

        private ContractorAvailability _conAvail = new ContractorAvailability();

        public ContractorAvailability SelectedContractorAvailability
        {
            get => _conAvail;
            set
            {
                _conAvail = value;
                OnPropertyChanged("SelectedContractorAvailability");
            }
        }

        private ContractorAvailability _conAvailChange = new ContractorAvailability();

        public ContractorAvailability ChangeContractorAvailability
        {
            get => _conAvailChange;
            set
            {
                _conAvail = value;
                OnPropertyChanged("ChangeContractorAvailability");
            }
        }


        public ObservableCollection<ContractorAvailability> ContractorAvailabilityCollection { get; set; } = new ObservableCollection<ContractorAvailability>();

        public RelayCommand AddCommand => new RelayCommand(AddAvailability, true);
        public RelayCommand UpdateCommand => new RelayCommand(UpdateAvailability, true);
        public RelayCommand DeleteCommand => new RelayCommand(DeleteAvailability, true);

        public ContractorAvailabilityViewModel(Contractor contractor)
        {
            ChangeContractorAvailability.SetContractor(contractor);
            SelectedContractorAvailability.SetContractor(contractor);
            ReadAllAvailabilities();
        }

        private void ReadAllAvailabilities()
        {
            DAL dal = new DAL();
            string queryString = "select * from contractoravailability where date >= DATE(NOW() - INTERVAL 1 DAY) and contractorid = " + SelectedContractorAvailability.ContractorId + " and active = 1;";
            DataTable dt = dal.ReadRecords(queryString);
            ContractorAvailabilityCollection.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                ContractorAvailability avail = new ContractorAvailability(dr);
                ContractorAvailabilityCollection.Add(avail);
            }
        }

        private void AddAvailability()
        {
            if (SelectedContractorAvailability == null)
            {
                SelectedContractorAvailability = new ContractorAvailability();
            }
            ChangeContractorAvailability.Add();
            SelectedContractorAvailability.SetContractorAvailability(ChangeContractorAvailability);
            ReadAllAvailabilities();
        }

        private void UpdateAvailability()
        {
            SelectedContractorAvailability.SetContractorAvailability(ChangeContractorAvailability);
            SelectedContractorAvailability.Update();
            ReadAllAvailabilities();
        }

        private void DeleteAvailability()
        {
            SelectedContractorAvailability.SetContractorAvailability(ChangeContractorAvailability);
            SelectedContractorAvailability.Delete();
            ReadAllAvailabilities();
        }
    }
}
