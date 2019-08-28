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
    class UpdateClientViewModel : NotificationClass
    {
        // Update and Delete Client

        private Client _client = new Client();
        public Client SelectedClient
        {
            get => _client;
            set
            {
                _client = value;
                OnPropertyChanged("SelectedClient");
            }
        }
       
        public ObservableCollection<char> GenderCollection { get; set; } = GenderList.GenderCollection;

        public LocationViewModel Location { get; set; }

        public RelayCommand<Window> UpdateCommand => new RelayCommand<Window>(UpdateClient, true);
        public RelayCommand<Window> DeleteCommand => new RelayCommand<Window>(DeleteClient, true);
        public RelayCommand AddJobCommand => new RelayCommand(AddJob, true);

        public UpdateClientViewModel(Client client)
        {
            this.SelectedClient = client;
            Location = new LocationViewModel(SelectedClient.Company.Location);
        }

        private void UpdateClient(Window window)
        {
            SelectedClient.Company.SetLocation(Location.SelectedLocation);
            SelectedClient.Update();
            if (SelectedClient.Company.Update())
                window?.Close();
        }

        private void DeleteClient(Window window)
        {
            if (!SelectedClient.Delete())
                return;
            if (SelectedClient.Company.Delete())
                window?.Close();
        }

        private void AddJob()
        {
            AddJob aj = new AddJob(SelectedClient.Company.CompanyId, SelectedClient.ClientId, Convert.ToInt32(Session.CoordinatorId));
            aj.ShowDialog();
        }

    }
}
