using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITClientServer.Commands;
using BITClientServer.Model;
using BITClientServer.View.Client;

namespace BITClientServer.ViewModel
{
    class AddClientViewModel : NotificationClass
    {
        private Client _coordinator = new Client();
        public Client SelectedClient
        {
            get => _coordinator;
            set
            {
                _coordinator = value;
                OnPropertyChanged("SelectedClient");
            }
        }

        private Company _company = new Company();

        public Company SelectedCompany
        {
            get => _company;
            set
            {
                _company = value;
                OnPropertyChanged("SelectedCompany");
            }
        }

        public ObservableCollection<char> GenderCollection { get; set; }= GenderList.GenderCollection;

        public LocationViewModel Location { get; set; } = new LocationViewModel();

        public RelayCommand<Window> AddCommand => new RelayCommand<Window>(AddClient, true);

        private void AddClient(Window window)
        {
            int clientId = SelectedClient.Add();
            if (clientId == 0)
            {
                return;
            }
            SelectedCompany.SetLocation(Location.SelectedLocation);
            SelectedCompany.SetClient(clientId);
            if (SelectedCompany.Add())
                window?.Close();
        }
        
    }
}
