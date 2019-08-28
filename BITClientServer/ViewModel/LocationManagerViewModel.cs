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

namespace BITClientServer.ViewModel
{
    class LocationManagerViewModel : NotificationClass
    {
        // For Admin Actions Tab - Location CRUD

        private Location _selectedLocation = new Location();

        public Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                OnPropertyChanged("SelectedLocation");
            }
        }

        private Location _changeLocation = new Location();

        public Location ChangeLocation
        {
            get => _changeLocation;
            set
            {
                _changeLocation = value;
                OnPropertyChanged("ChangeLocation");
            }
        }

        public ObservableCollection<Location> LocationCollection { get; set; } = new ObservableCollection<Location>();

        public RelayCommand AddCommand => new RelayCommand(Add, true);
        public RelayCommand UpdateCommand => new RelayCommand(Update, true);
        public RelayCommand DeleteCommand => new RelayCommand(Delete, true);

        public LocationManagerViewModel()
        {
            ReadAllLocations();
        }

        private void Add()
        {
            if (SelectedLocation == null)
            {
                SelectedLocation = new Location();
            }
            SelectedLocation.SetLocation(ChangeLocation);
            SelectedLocation.NullLocationId();
            SelectedLocation.Add();
            ReadAllLocations();
        }

        private void Update()
        {
            SelectedLocation.SetLocation(ChangeLocation);
            SelectedLocation.Update();
            ReadAllLocations();
        }

        private void Delete()
        {
            if (SelectedLocation == null)
            {
                return;
            }
            SelectedLocation.SetLocation(ChangeLocation);
            SelectedLocation.Delete();
            ReadAllLocations();
        }

        private void ReadAllLocations()
        {
            DAL dal = new DAL();
            string queryString = "select * from location";
            DataTable dt = dal.ReadRecords(queryString);
            LocationCollection.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Location location = new Location(dr);
                LocationCollection.Add(location);
            }
        }
    }
}
