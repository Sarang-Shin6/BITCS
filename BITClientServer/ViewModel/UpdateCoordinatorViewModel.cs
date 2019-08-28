using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITClientServer.Commands;
using BITClientServer.Model;
using BITClientServer.View;

namespace BITClientServer.ViewModel
{
    class UpdateCoordinatorViewModel : NotificationClass
    {
        // Update and Delete Coordinator

        public ObservableCollection<char> GenderCollection { get; set; } = GenderList.GenderCollection;

        public ObservableCollection<string> StateCollection { get; set; } = Model.StateList.StateCollection;

        public UpdateCoordinatorViewModel(Coordinator coordinator)
        {
            SelectedCoordinator = coordinator;
        }

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

        public RelayCommand<Window> UpdateCommand => new RelayCommand<Window>(UpdateCoordinator, true);

        public RelayCommand<Window> DeleteCommand => new RelayCommand<Window>(DeleteCoordinator, true);

        private void UpdateCoordinator(Window window)
        {
            if (SelectedCoordinator.Update())
                window?.Close();
        }

        private void DeleteCoordinator(Window window)
        {
            if (SelectedCoordinator.Delete())
                window?.Close();
        }
    }
}
