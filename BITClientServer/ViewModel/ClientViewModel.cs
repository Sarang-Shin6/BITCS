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
using BITClientServer.View.Client;

namespace BITClientServer.ViewModel
{
    class ClientViewModel : NotificationClass
    {
        public ObservableCollection<Client> ClientCollection { get; set; } = new ObservableCollection<Client>();

        public Dictionary<string, string> SearchFields { get; set; } = new Dictionary<string, string>()
        {
            {"Client ID", "cl.clientid"},
            {"First Name", "cl.firstname"},
            {"Last Name", "cl.lastname"},
            {"Phone", "cl.phone"},
            {"Company ID", "c.companyid"},
            {"Name", "c.name"},
            {"Postcode", "c.postcode"},
            {"Branch", "c.branch"},
            {"Location ID", "l.locationid"}
        };

        public string SelectedField { get; set; } = "cl.clientid";

        public string SearchString { get; set; } = "Enter a search term here";

        private Client _client;
        public Client SelectedClient
        {
            get => _client;
            set
            {
                _client = value;
                OnPropertyChanged("SelectedClient");
            }
        }

        public RelayCommand ShowClientCommand => new RelayCommand(ShowClient, true);
        public RelayCommand AddClientCommand => new RelayCommand(AddClient, true);
        public RelayCommand SearchCommand => new RelayCommand(Search, true);

        public ClientViewModel()
        {
            ReadAllClients();
        }

        private void ShowClient()
        {
            if (SelectedClient == null)
            {
                return;
            }
            UpdateClient uc = new UpdateClient(SelectedClient);
            uc.ShowDialog();
            ReadAllClients();
        }

        private void AddClient()
        {
            AddClient ac = new AddClient();
            ac.ShowDialog();
            ReadAllClients();
        }

        public void ReadAllClients()
        {
            DAL dal = new DAL();
            string queryString = "select cl.clientid, cl.firstname, cl.lastname, cl.phone, cl.email, cl.gender, c.companyid, c.name, c.street, c.suburb, c.state, c.postcode, c.branch, c.Description as companydescription, l.Description as location from client as cl, company as c, location as l where cl.clientID = c.clientID and l.locationid = c.locationid and cl.active = 1 and c.active = 1 and l.active = 1;";
            DataTable dt = dal.ReadRecords(queryString);
            ClientCollection.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Client client = new Client(dr);
                ClientCollection.Add(client);
            }
        }

        private void Search()
        {
            DAL dal = new DAL();
            string queryString;
            if (SelectedField == "cl.clientid" || SelectedField == "c.companyid")
            {
                queryString = "select cl.clientid, cl.firstname, cl.lastname, cl.phone, cl.email, cl.gender, c.companyid, c.name, c.street, c.suburb, c.state, c.postcode, c.branch, c.Description as companydescription, l.Description as location from client as cl, company as c, location as l where cl.clientID = c.clientID and l.locationid = c.locationid and cl.active = 1 and c.active = 1 and l.active = 1 and " + SelectedField + " = '" + SearchString + "';";
            }
            else
            {
                queryString = "select cl.clientid, cl.firstname, cl.lastname, cl.phone, cl.email, cl.gender, c.companyid, c.name, c.street, c.suburb, c.state, c.postcode, c.branch, c.Description as companydescription, l.Description as location from client as cl, company as c, location as l where cl.clientID = c.clientID and l.locationid = c.locationid and cl.active = 1 and c.active = 1 and l.active = 1 and " + SelectedField + " LIKE '%" + SearchString + "%';";
            }
            DataTable dt = dal.ReadRecords(queryString);
            ClientCollection.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Client client = new Client(dr);
                ClientCollection.Add(client);
            }
        }
    }
}
