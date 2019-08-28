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
using BITClientServer.View.Coordinator;

namespace BITClientServer.ViewModel
{
    class CoordinatorViewModel : NotificationClass
    {
        public ObservableCollection<Coordinator> CoordinatorCollection { get; } = new ObservableCollection<Coordinator>();

        public Dictionary<string, string> SearchFields { get; set; } = new Dictionary<string, string>()
        {
            {"Coordinator ID", "coordinator.coordinatorid"},
            {"First Name", "staff.firstname"},
            {"Last Name", "staff.lastname"},
            {"Email", "staff.email"},
            {"Phone", "staff.phone"}
        };

        public string SelectedField { get; set; } = "coordinator.coordinatorid";

        public string SearchString { get; set; } = "Enter a search term here";

        private Coordinator _coordinator;
        public Coordinator SelectedCoordinator
        {
            get => _coordinator;
            set
            {
                _coordinator = value;
                OnPropertyChanged("SelectedCoordinator");
            }
        }

        public CoordinatorViewModel()
        {
            ReadAllCoordinators();
        }

        public RelayCommand ShowCoordinatorCommand => new RelayCommand(ShowCoordinator, true);
        public RelayCommand AddCoordinatorCommand => new RelayCommand(AddCoordinator, true);
        public RelayCommand SearchCommand => new RelayCommand(Search, true);


        public void ReadAllCoordinators()
        {
            DAL dal = new DAL();
            string queryString = "select coordinator.coordinatorid, staff.firstname, staff.lastname, staff.street, staff.suburb, staff.state, staff.postcode, staff.phone, staff.email, staff.gender, staff.accname, staff.accbsb, staff.accnumber from coordinator, staff where coordinator.coordinatorid = staff.staffid and staff.active = 1;";
            DataTable dt = dal.ReadRecords(queryString);
            CoordinatorCollection.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Coordinator coordinator = new Coordinator(dr);
                CoordinatorCollection.Add(coordinator);
            }
        }

        private void Search()
        {
            DAL dal = new DAL();
            string queryString;
            if (SelectedField == "coordinator.coordinatorid")
            {
                queryString =
                    "select coordinator.coordinatorid, staff.firstname, staff.lastname, staff.street, staff.suburb, staff.state, staff.postcode, staff.phone, staff.email, staff.gender, staff.accname, staff.accbsb, staff.accnumber from coordinator, staff where coordinator.coordinatorid = staff.staffid and staff.active = 1 and " +
                    SelectedField + " = '" + SearchString + "';";
            }
            else
            {
                queryString =
                    "select coordinator.coordinatorid, staff.firstname, staff.lastname, staff.street, staff.suburb, staff.state, staff.postcode, staff.phone, staff.email, staff.gender, staff.accname, staff.accbsb, staff.accnumber from coordinator, staff where coordinator.coordinatorid = staff.staffid and staff.active = 1 and " +
                    SelectedField + " LIKE '%" + SearchString + "%';";
            }

            DataTable dt = dal.ReadRecords(queryString);
            CoordinatorCollection.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Coordinator coordinator = new Coordinator(dr);
                CoordinatorCollection.Add(coordinator);
            }
        }

        private void ShowCoordinator()
        {
            if (SelectedCoordinator == null)
            {
                return;
            }
            UpdateCoordinator uc = new UpdateCoordinator(SelectedCoordinator);
            uc.ShowDialog();
            ReadAllCoordinators();
        }

        private void AddCoordinator()
        {
            AddCoordinator ac = new AddCoordinator();
            ac.ShowDialog();
            ReadAllCoordinators();
        }
    }
}
