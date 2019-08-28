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
using BITClientServer.View.Contractor;

namespace BITClientServer.ViewModel
{
    class ContractorViewModel : NotificationClass
    {
        public ObservableCollection<Contractor> ContractorCollection { get; } = new ObservableCollection<Contractor>();

        public Dictionary<string, string> SearchFields { get; set; } = new Dictionary<string, string>()
        {
            {"Contractor ID", "contractor.contractorid"},
            {"First Name", "staff.firstname"},
            {"Last Name", "staff.lastname"},
            {"Email", "staff.email"},
            {"Phone", "staff.phone"}
        };

        public string SelectedField { get; set; } = "contractor.contractorid";

        public string SearchString { get; set; } = "Enter a search term here";

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

        public ContractorViewModel()
        {
            ReadAllContractors();
        }

        public RelayCommand ShowContractorCommand => new RelayCommand(ShowContractor, true);
        public RelayCommand AddContractorCommand => new RelayCommand(AddContractor, true);
        public RelayCommand SearchCommand => new RelayCommand(Search, true);

        public void ReadAllContractors()
        {
            DAL dal = new DAL();
            string queryString = "select contractor.contractorid, staff.firstname, staff.lastname, staff.street, staff.suburb, staff.state, staff.postcode, staff.phone, staff.email, staff.gender, staff.accname, staff.accbsb, staff.accnumber from contractor, staff where contractor.contractorid = staff.staffid and contractor.active = 1 and staff.active = 1;";
            DataTable dt = dal.ReadRecords(queryString);
            ContractorCollection.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Contractor contractor = new Contractor(dr);
                ContractorCollection.Add(contractor);
            }
        }

        private void Search()
        {
            DAL dal = new DAL();
            string queryString;
            if (SelectedField == "contractor.contractorid")
            {
                queryString = "select contractor.contractorid, staff.firstname, staff.lastname, staff.street, staff.suburb, staff.state, staff.postcode, staff.phone, staff.email, staff.gender, staff.accname, staff.accbsb, staff.accnumber from contractor, staff where contractor.contractorid = staff.staffid and contractor.active = 1 and staff.active = 1 and " +
                    SelectedField + " = '" + SearchString + "';";
            }
            else
            {
                queryString = "select contractor.contractorid, staff.firstname, staff.lastname, staff.street, staff.suburb, staff.state, staff.postcode, staff.phone, staff.email, staff.gender, staff.accname, staff.accbsb, staff.accnumber from contractor, staff where contractor.contractorid = staff.staffid and contractor.active = 1 and staff.active = 1 and " +
                    SelectedField + " LIKE '%" + SearchString + "%';";
            }

            DataTable dt = dal.ReadRecords(queryString);
            ContractorCollection.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Contractor contractor = new Contractor(dr);
                ContractorCollection.Add(contractor);
            }
        }

        private void ShowContractor()
        {
            if (SelectedContractor == null)
            {
                return;
            }
            UpdateContractor uc = new UpdateContractor(SelectedContractor);
                uc.ShowDialog();
            ReadAllContractors();
        }

        private void AddContractor()
        {
            AddContractor ac = new AddContractor();
            ac.ShowDialog();
            ReadAllContractors();
        }

        
    }
}
