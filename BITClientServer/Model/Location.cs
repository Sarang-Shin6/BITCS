using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITClientServer.View;

namespace BITClientServer.Model
{
    public class Location
    {
        #region Variables

        public int LocationId { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public bool Active { get; set; }

        #endregion Variables

        #region Constructor

        public Location(int locationid, string description, string city, bool active)
        {
            this.LocationId = locationid;
            this.Description = description;
            this.City = city;
            this.Active = active;
        }
        public Location()
        {

        }

        public Location(DataRow drContractor)
        {
            MapProperties(drContractor);
        }

        public void GetLocation(int locationId)
        {

            try
            {
                DAL dal = new DAL();
                string queryString = "select * from location where locationid = " + locationId + " and active = 1;";
                DataTable dt = dal.ReadRecords(queryString);
                try
                {
                    if (dt.Rows.Count == 1)
                    {
                        MapProperties(dt.Rows[0]);
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                catch (Exception)
                {

                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong whilst obtaining status. Please try again later." + e.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        #endregion Constructor

        #region Validation

        private bool Validation()
        {
            List<string> list = new List<string> {this.Description, this.City};
            if (ValidationClass.CheckIfNull(list))
            {
                return true;
            }

            return false;
        }

        private bool Validation(int ContractorId)
        {
            List<int> list = new List<int> { this.LocationId, ContractorId};
            if (ValidationClass.CheckIfNull(list))
            {
                return true;
            }

            return false;
        }

        #endregion Validation

        #region Methods

        

        private void MapProperties(DataRow dr)
        {
            this.LocationId = Convert.ToInt32(dr["locationid"]);
            this.Description = dr["description"].ToString();
            this.City = dr["city"].ToString();
            this.Active = Convert.ToBoolean(dr["Active"]);
        }

        public void AddContractorLocation(int contractorId)
        {
            try
            {
                if (Validation(contractorId))
                {
                    MessageBox.Show("Invalid Data Entry.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DAL dal = new DAL();
                string queryString = "Insert into ContractorLocation(locationid, contractorid, Active) values (" +
                                         this.LocationId + ", " + contractorId + ", 1);";
                dal.Query(queryString);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong. Please try again later. " + e.Message, "Unsuccessful", MessageBoxButton.OK,MessageBoxImage.Error);
                throw;
            }
        }

        public void DeleteContractorLocation(int contractorId)
        {
            try
            {
                if (this.LocationId == 0)
                {
                    MessageBox.Show("Invalid Data Entry.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DAL dal = new DAL();
                string queryString = "update contractorlocation set active = 0 where contractorid = " + contractorId +
                                     " and locationid = " + this.LocationId + ";";
                dal.Query(queryString);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong. Please try again later. " + e.Message, "Unsuccessful", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }
        }

        public static List<Location> getLocationsFromPref(int ContractorId)
        {
            // Get All Contractor's Location from database

            try
            {
                DAL dal = new DAL();
                string queryString = "Select * from contractorlocation where contractorid = " + ContractorId + " and active = 1;";
                DataTable dtConLoc = dal.ReadRecords(queryString);
                DataTable dt;
                List<Location> tempLocCollection = new List<Location>();
                foreach (DataRow drConLoc in dtConLoc.Rows)
                {
                    queryString = "Select * from location where locationid = " + Convert.ToInt32(drConLoc["locationid"]) + " and active = 1;";
                    dt = dal.ReadRecords(queryString);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Location newLoc = new Location(dr);
                        tempLocCollection.Add(newLoc);
                    }
                }

                return tempLocCollection;
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong. Please try again later. " + e.Message, "Unsuccessful", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }
        }
        #region CRUD

        public bool Add()
        {
            if (Validation())
            {
                MessageBox.Show("Invalid Data Entry.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            DAL dal = new DAL();
            var activeBit = 1;
            if (Active == false)
            {
                activeBit = 0;
            }
            string queryString = "Insert into Location(description, city, Active) values ('" + this.Description + "', '" + this.City + "', "+ activeBit +");";


            try
            {
                if (dal.Query(queryString) >= 1)
                {
                    MessageBox.Show("The Location has been successfully added to the database.", "Success!", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return true;
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong. Please try again later. " + e.Message, "Unsuccessful", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }
        }

        public bool Update()
        {
            if (Validation() || this.LocationId == 0)
                if (Validation())
                {
                    MessageBox.Show("Invalid Data Entry.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            if (MessageBox.Show("Would you like to update this location? This cannot be undone.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return false;
            }
            DAL dal = new DAL();
            var activeBit = 1;
            if (Active == false)
            {
                activeBit = 0;
            }
            string queryString = "Update Location set Description = '" + this.Description + "', City = '" +
                                 this.City + "', active = " + activeBit + " where Locationid = " + this.LocationId + ";";


            try
            {
                if (dal.Query(queryString) >= 1)
                {
                    MessageBox.Show("The Location has been successfully updated in the database.",
                        "Success!", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return true;
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong. Please try again later. " + e.Message, "Unsuccessful", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }
        }

        public bool Delete()
        {
            if (this.LocationId == 0)
                return false;
            if (MessageBox.Show("Would you like to delete this location?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
            {
                MessageBox.Show("Could not find contractor to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            DAL dal = new DAL();
            string queryString = "delete from Location where locationid = " + this.LocationId + ";";

            try
            {
                if (dal.Query(queryString) == 1)
                {
                    MessageBox.Show("The Location has been successfully deleted from the database.",
                        "Success!", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return true;
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong. Please try again later. " + e.Message, "Unsuccessful", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }
        }

        #endregion CRUD

        public void NullLocationId()
        {
            this.LocationId = -1;
        }

        public void SetLocation(Location changeLocation)
        {
            this.Description = changeLocation.Description;
            this.City = changeLocation.City;
            this.Active = changeLocation.Active;
        }
#endregion Methods
    }
}
