using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BITClientServer.Commands;
using BITClientServer.Model;
using BITClientServer.View;

namespace BITClientServer.ViewModel
{
    class LocationViewModel : NotificationClass
    {
        // For Location Selection

        #region Variables

        public ObservableCollection<string> CityCollection { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<Location> LocationCollection { get; set; } = new ObservableCollection<Location>();

        public ObservableCollection<Location> PrefLocationCollection { get; set; } = new ObservableCollection<Location>();

        public Contractor Contractor { get; set; }

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

        private string _selectedCity = "Sydney";

        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                _selectedCity = value;
                OnPropertyChanged("SelectedCity");
                GetLocations();
            }
        }

        private Location _prefLocation;

        public Location SelectedPrefLocation
        {
            get => _prefLocation;
            set
            {
                _prefLocation = value;
                OnPropertyChanged("SelectedPrefLocation");
            }
        }

        public ObservableCollection<string> StateCollection { get; set; } = Model.StateList.StateCollection;

        #endregion Variables

        #region Commands

        public RelayCommand AddLocationToListCommand => new RelayCommand(AddLocationToPref, true);

        public RelayCommand RemoveLocationFromListCommand => new RelayCommand(RemoveLocationFromPref, true);

        #endregion Commands

        #region Constructor

        /// <summary>
        /// Constructor to display the List of cities, Locations list and Preferred Locations list for the corresponding contractor.
        /// </summary>

        public LocationViewModel()
        {
            GetCities();
            InitialiseLocationList();
            
        }

        public LocationViewModel(Location location)
        {
            GetCities();
            this.SelectedCity = location.City;
            InitialiseLocationList();

            foreach (Location loc in LocationCollection)
            {
                if (location.LocationId == loc.LocationId)
                {
                    this.SelectedLocation = loc;
                }
            }
        }

        #endregion Constructor

        #region methods

        /// <summary>
        /// Set Contractor for context for this ViewModel
        /// </summary>
        /// <param name="contractor"></param>

        public void SetContractor(Contractor contractor)
        {
            Contractor = contractor;
            GetPrefLocation();
            GetLocations();
        }
        

        /// <summary>
        /// Initialises the Location List. For use in Constructor
        /// </summary>

        public void InitialiseLocationList()
        {
            DAL dal = new DAL();
            string queryString = "Select * from location where active = 1;";
            DataTable dt = new DataTable();
            dt = dal.ReadRecords(queryString);
            foreach (DataRow dr in dt.Rows)
            {
                Location location = new Location(Convert.ToInt32(dr["LocationID"]), dr["Description"].ToString(),
                    dr["City"].ToString(), true);
                if (location.City == SelectedCity) // Checks Location's City to show only selected City
                {
                    LocationCollection.Add(location);
                }

            }

            SelectedLocation = LocationCollection[0];
        }

        /// <summary>
        /// Gets the Locations from the database checking if it is present in the Preferred Locations Collection
        /// Used to Refresh the two grid views used for selecting locations when updating and adding a contractor.
        /// </summary>

        private void GetLocations()
        {
            LocationCollection.Clear();
            DAL dal = new DAL();
            string queryString = "Select * from location where active = 1;";
            DataTable dt = new DataTable();
            dt = dal.ReadRecords(queryString);
            foreach (DataRow dr in dt.Rows)
            {
                Location location = new Location(Convert.ToInt32(dr["LocationID"]), dr["Description"].ToString(),
                    dr["City"].ToString(), true);
                if (location.City == SelectedCity) // Checks Location's City to show only selected City
                {
                    bool locationNotInPref = true;
                    if (PrefLocationCollection.Count == 0) // If PrefJobCategoryCollection is empty
                    {
                        LocationCollection.Add(location);
                    }
                    else
                    {
                        foreach (Location prefLocation in PrefLocationCollection)
                            // If selected JobCategory is present in PrefJobCategoryCollection don't add to JobCategoryCollection
                        {
                            if (prefLocation.LocationId == location.LocationId)
                            {
                                locationNotInPref = false;
                            }
                        }
                        // Else add Location to LocationCollection
                        if (locationNotInPref)
                        {
                            LocationCollection.Add(location);
                        }

                        List<Location> locationListToCheck = new List<Location>();
                        foreach (Location loc in locationListToCheck)
                        {
                            locationListToCheck.Add(loc);
                        }

                        foreach (Location prefLocation in PrefLocationCollection)
                            // Deals with Location present in both PrefLocationCollection and LocationCollection to remove from LocationCollection.
                            // Compares each location in LocationCollection to each location in PrefLocationCollection
                            // if locations are equal, remove location from LocationCollection
                        {
                            foreach (Location locationToCheck in locationListToCheck)
                            {
                                bool toRemove = prefLocation.LocationId == locationToCheck.LocationId;
                                if (toRemove)
                                    LocationCollection.Remove(locationToCheck);
                            }
                        }
                    }
                }
            }


            
        }

        /// <summary>
        /// Gets list of Cities present in Locations table so user can select it to get the list of appropriate locations.
        /// </summary>
        public void GetCities()
        {
            DAL dal = new DAL();
            string queryString = "Select distinct City from location where active = 1;";
            DataTable dt = new DataTable();
            dt = dal.ReadRecords(queryString);
            foreach (DataRow dr in dt.Rows)
            {
                CityCollection.Add(dr["City"].ToString());
            }

            SelectedCity = CityCollection[0];
        }

        #region ContractorLocation CRUD

        /// <summary>
        /// Add the corresponding location from preferred locations list with contractor into ContractorLocation.
        /// </summary>
        /// <param name="contractorId"></param>

        public bool AddContractorLocation(int contractorId)
        {
            if (PrefLocationCollection.Count == 0)
                return false;
            foreach (Location location in PrefLocationCollection)
            {
                location.AddContractorLocation(contractorId);
            }

            return true;
        }

        /// <summary>
        /// Updates ContractorLocation by checking if location in preferred location is present in the database.
        /// Add if present in Preferred Locations Collection and not in database
        /// Delete if present in database and not in Preferred Locations
        /// </summary>

        public bool UpdateContractorLocation()
        {
            if (PrefLocationCollection.Count == 0)
                return false;
            List<Location> DBLocCollection = Location.getLocationsFromPref(Contractor.ContractorId);

            // Add

            foreach (Location location in PrefLocationCollection)
            {
                bool toAdd = true;
                foreach (Location prefLoc in DBLocCollection)
                {
                    if (location.LocationId == prefLoc.LocationId)
                    {
                        toAdd = false;
                    }
                }
                if (toAdd)
                    location.AddContractorLocation(Contractor.ContractorId);
                
            }

            // Delete

            foreach (Location loc in DBLocCollection)
            {
                bool toDelete = true;
                foreach (Location prefLoc in PrefLocationCollection)
                {
                    if (loc.LocationId == prefLoc.LocationId)
                    {
                        toDelete = false;
                    }
                }
                if (toDelete)
                    loc.DeleteContractorLocation(Contractor.ContractorId);
            }
            return true;
        }

        #endregion ContractorLocation CRUD

        #region Preferred Locations Collection Management

        /// <summary>
        /// Reads the ContractorLocation table to fill in contractor's Preferred Locations List
        /// </summary>

        private void GetPrefLocation()
        {
            DAL dal = new DAL();
            string queryString = "select location.locationid, location.description, location.city, contractorlocation.locationid, location.active from Location, contractorlocation where contractorlocation.contractorid =" +
                                 Contractor.ContractorId + " and contractorlocation.locationid = location.locationid;";
            DataTable dt = new DataTable();
            dt = dal.ReadRecords(queryString);
            foreach (DataRow dr in dt.Rows)
            {
                Location location = new Location(dr);
                PrefLocationCollection.Add(location);
            }

        }

        /// <summary>
        /// Add Location to Preferred Locations Collection
        /// </summary>

        private void AddLocationToPref()
        {
            if (SelectedLocation == null)
            {
                return;
            }
            PrefLocationCollection.Add(SelectedLocation);
            GetLocations();
        }

        /// <summary>
        /// Remove Location from Preferred Locations Collection
        /// </summary>

        private void RemoveLocationFromPref()
        {
            if (SelectedPrefLocation == null)
            {
                return;
            }
            PrefLocationCollection.Remove(SelectedPrefLocation);
            GetLocations();
        }

        #endregion Preferred Locations Collection Management

        

        #endregion methods


    }
}
