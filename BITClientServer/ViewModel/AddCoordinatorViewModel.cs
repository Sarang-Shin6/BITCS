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
    class AddCoordinatorViewModel : NotificationClass
    {
        public ObservableCollection<char> GenderCollection { get; set; } = GenderList.GenderCollection;
        public ObservableCollection<string> StateCollection { get; set; } = Model.StateList.StateCollection;

        private Coordinator _coordinator = new Coordinator();
        public Coordinator SelectedCoordinator
        {
            get => _coordinator;
            set
            {
                _coordinator = value;
                OnPropertyChanged("SelectedCoordinator");
            }
        }

        public RelayCommand<Window> AddCommand => new RelayCommand<Window>(AddCoordinator, true);

        private void AddCoordinator(Window window)
        {
            if (SelectedCoordinator.Add())
            window?.Close();
        }
    }
}
