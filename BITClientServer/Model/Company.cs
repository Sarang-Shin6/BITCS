using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITClientServer.View;

namespace BITClientServer.Model
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; } = StateList.StateCollection[0];
        public string Postcode { get; set; }
        public string Branch { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }
        public Location Location { get; set; }

        public Company() { }

        public Company(int companyId)
        {
            GetCompany(companyId);
        }

        public void GetCompany(int companyId)
        {
            DAL dal = new DAL();
            string queryString = "select * from company where companyid = " + companyId + " and active = 1;";
            DataTable dt = dal.ReadRecords(queryString);
            try
            {
                if (dt.Rows.Count == 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        MapProperties(dr);
                    }

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

        private void MapProperties(DataRow dr)
        {
            this.CompanyId = Convert.ToInt32(dr["CompanyID"]);
            this.Name = dr["Name"].ToString();
            this.Street = dr["Street"].ToString();
            this.Suburb = dr["Suburb"].ToString();
            this.State = dr["State"].ToString();
            this.Postcode = dr["Postcode"].ToString();
            this.Branch = dr["Branch"].ToString();
            this.Description = dr["Description"].ToString();
            this.ClientId = Convert.ToInt32(dr["ClientID"]);
            this.Location = new Location();
            this.Location.GetLocation(Convert.ToInt32(dr["LocationID"]));

        }

        private bool Validation()
        {
            List<string> list = new List<string>
            {
                this.Name,
                this.Street,
                this.Suburb,
                this.State,
                this.Postcode,
                this.Branch,
                this.Description,
                this.Location.Description
            };
            if (ValidationClass.CheckIfNull(list))
            {
                MessageBox.Show("Please make sure all fields are filled out.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            if (ValidationClass.CheckPostcode(this.Postcode))
            {
                MessageBox.Show("Please enter a valid postcode.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            return false;
        }

        public bool Add()
        {
            if (Validation())
            {
                return false;
            }
            DAL dal = new DAL();
            string queryString = "Insert into company(name, street, suburb, state, postcode, branch, description, clientid, locationid, Active) values ('" + this.Name + "', '" + this.Street + "', '" + this.Suburb +
                                 "', '" + this.State + "', '" + this.Postcode + "', '" + this.Branch + "', '" + this.Description + "', " + this.ClientId + ", " + this.Location.LocationId + ", 1);";

            try
            {
                if (dal.Query(queryString) == 1)
                {
                    MessageBox.Show("The Client has been successfully added to the database.", "Success!", MessageBoxButton.OK, MessageBoxImage.Hand);
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
            if (Validation())
            {
                return false;
            }
            if (MessageBox.Show("Would you like to update this client? This cannot be undone.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return false;
            }
            DAL dal = new DAL();
            string queryString = "Update company set name = '" + this.Name + "', Street = '" +
                                 this.Street + "', suburb = '" + this.Suburb + "', postcode = '" + this.Postcode +
                                 "', Branch = '" + this.Branch + "', description = '" + this.Description + "', ClientId = " + this.ClientId + ", locationid = " + this.Location.LocationId + " where companyid = " + this.CompanyId + ";";


            try
            {
                if (dal.Query(queryString) >= 1)
                {
                    MessageBox.Show("The Client has been successfully updated in the database.",
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
            if (this.CompanyId == 0)
            {
                MessageBox.Show("Could not find Company to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (MessageBox.Show("Would you like to delete this client?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return false;
            }
            DAL dal = new DAL();
            string queryString = "update company set Active = 0 where companyid = " + this.CompanyId + ";";

            try
            {
                if (dal.Query(queryString) >= 1)
                {
                    MessageBox.Show("The Client has been successfully deleted from the database.",
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

        public void SetLocation(Location location)
        {
            this.Location = location;
        }

        public void SetClient(int clientId)
        {
            this.ClientId = clientId;
        }


    }
}
