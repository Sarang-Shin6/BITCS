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
    public class Contractor : Staff
    {
        public int ContractorId { get; set; }

        public double ContractorRating { get; set; }

        public void GetRating()
        {
            DAL dal = new DAL();
            string queryString =
                "select AVG(jobrating) as ConRating from job where contractorid = " + this.ContractorId + ";";
            DataTable dt = dal.ReadRecords(queryString);
            foreach (DataRow dr in dt.Rows)
            {
                this.ContractorRating = Convert.ToDouble(dr["ConRating"]);
            }

        }

        public Contractor(int staffid, string firstname, string lastname, string street, string suburb, string state,
            string postcode, string email, string phone, string accname, string accbsb, string accnumber, char gender) :
            base(staffid, firstname, lastname, street, suburb, state, postcode, email, phone, accname, accbsb,
                accnumber,
                gender)
        {
            GetRating();
            this.ContractorId = staffid;

        }

        public Contractor(int contractorId)
        {
            this.ContractorId = contractorId;
            GetContractor(contractorId);
        }

        public Contractor() { }

        public Contractor(DataRow drContractor)
        {
            MapProperties(drContractor);
        }

        private void MapProperties(DataRow drContractor)
        {
            this.ContractorId = Convert.ToInt32(drContractor["contractorid"]);
            this.FirstName  = drContractor["firstname"].ToString();
            this.LastName  = drContractor["lastname"].ToString();
            this.Street = drContractor["street"].ToString();
            this.Suburb = drContractor["suburb"].ToString();
            this.State = drContractor["state"].ToString();
            this.Postcode = drContractor["postcode"].ToString();
            this.Email = drContractor["email"].ToString();
            this.Phone  = drContractor["phone"].ToString();
            this.AccName = drContractor["accname"].ToString();
            this.AccBSB = drContractor["accbsb"].ToString();
            this.AccNumber = drContractor["accnumber"].ToString();
            this.Gender = Convert.ToChar(drContractor["gender"].ToString());
        }

        private bool Validation()
        {
            List<string> list = new List<string>
            {
                this.FirstName,
                this.LastName,
                this.Street,
                this.Suburb,
                this.State,
                this.Postcode,
                this.Email,
                this.Phone,
                this.AccName,
                this.AccBSB,
                this.AccNumber
            };
            if (ValidationClass.CheckEmail(this.Email))
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            if (ValidationClass.CheckPhone(this.Phone))
            {
                MessageBox.Show("Please enter a valid phone number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
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

        public int Add()
        {
            try
            {
                if (Validation())
                {
                    return 0;
                }
                DAL dal = new DAL();
                string queryString =
                    "Insert into staff(FirstName, LastName, street, suburb, state, postcode, email, phone, accname, accbsb, accnumber, gender, Active) values ('" +
                    this.FirstName + "', '" + this.LastName + "', '" + this.Street + "', '" + this.Suburb + "', '" +
                    this.State + "', '" + this.Postcode + "', '" + this.Email + "', '" + this.Phone +
                    "', '" + this.AccName + "', '" + this.AccBSB + "', '" + this.AccNumber + "', '" + this.Gender +
                    "', 1);" + 
                "SELECT LAST_INSERT_ID();";
                DataTable dt = dal.ReadRecords(queryString);
                queryString = "Insert into contractor(contractorid, active) values (" + dt.Rows[0]["LAST_INSERT_ID()"] + ", 1);";

                if (dal.Query(queryString) == 1)
                {
                    MessageBox.Show(
                        this.FirstName + " " + this.LastName + " has been successfully added to the database.",
                        "Success!", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return Convert.ToInt32(dt.Rows[0][0]);
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
            if (MessageBox.Show("Would you like to update " + this.FirstName + " " + this.LastName +"? This cannot be undone.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return false;
            }
            try
            {
                DAL dal = new DAL();
                string queryString = "Update staff set firstname = '" + this.FirstName + "', lastname = '" +
                                     this.LastName + "', street = '" + this.Street + "', suburb = '" + this.Suburb +
                                     "', state = '" + this.State + "', postcode = '" + this.Postcode + "', phone = '" +
                                     this.Phone + "', email = '" + this.Email +
                                     "', gender = '" + this.Gender + "', AccName = '" + this.AccName +
                                     "', AccNumber = '" + this.AccNumber + "', accbsb = '" + this.AccBSB +
                                     "' where staffid = " + this.ContractorId + ";";
                if (dal.Query(queryString) >= 1)
                {
                    MessageBox.Show(
                        this.FirstName + " " + this.LastName + " has been successfully updated in the database.",
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
            if (this.ContractorId == 0)
            {
                MessageBox.Show("Could not find contractor to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (MessageBox.Show("Would you like to delete this contractor?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return false;
            }
            try
            {
                DAL dal = new DAL();
                string queryString = "update contractorlocation set Active = 0 where contractorid = " + this.ContractorId + ";" +
                                     "update contractorskill set Active = 0 where contractorid = " + this.ContractorId + ";" +
                                     "update contractoravailability set Active = 0 where contractorID = " + this.ContractorId + ";" +
                                     "update contractor set Active = 0 where contractorID = " + this.ContractorId + ";" +
                                     "update job set Active = 0 where contractorID = " + this.ContractorId + ";" +
                                     "update staff set Active = 0 where StaffID = " + this.StaffId + ";";
                
                if (dal.Query(queryString) >= 1)
                {
                    MessageBox.Show(
                        this.FirstName + " " + this.LastName + " has been successfully removed from the database.",
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

        public void GetContractor(int contractorid)
        {
            DAL dal = new DAL();
            string queryString =
                "select contractor.contractorid, staff.firstname, staff.lastname, staff.street, staff.suburb, staff.state, staff.postcode, staff.phone, staff.email, staff.gender, staff.accname, staff.accbsb, staff.accnumber from contractor, staff where contractor.contractorid = staff.staffid and contractor.contractorid = " +
                contractorid + " and contractor.active = 1;";
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
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong. Please try again later. " + e.Message, "Unsuccessful", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }


        }
    }
}
