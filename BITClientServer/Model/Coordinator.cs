using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITClientServer.View;

namespace BITClientServer.Model
{
    public class Coordinator : Staff
    {
        public int CoordinatorId
        {
            get { return StaffId; }
            set { StaffId = value; }
        }

        public Coordinator(int staffid, string firstname, string lastname, string street, string suburb, string state, string postcode, string email, string phone, string accname, string accbsb, string accnumber, string dob, char gender) : base(staffid, firstname, lastname, street, suburb, state, postcode, email, phone, accname, accbsb, accnumber,
            gender)
        { }

        public Coordinator(int coordinatorId)
        {
            this.GetCoordinator(coordinatorId);
        }

        public Coordinator() { }

        public Coordinator(DataRow drCoordinator)
        {
            MapProperties(drCoordinator);
        }

        private void MapProperties(DataRow drCoordinator)
        {
            this.CoordinatorId = Convert.ToInt32(drCoordinator["coordinatorid"]);
            this.FirstName = drCoordinator["firstname"].ToString();
            this.LastName = drCoordinator["lastname"].ToString();
            this.Street = drCoordinator["street"].ToString();
            this.Suburb = drCoordinator["suburb"].ToString();
            this.State = drCoordinator["state"].ToString();
            this.Postcode = drCoordinator["postcode"].ToString();
            this.Email = drCoordinator["email"].ToString();
            this.Phone = drCoordinator["phone"].ToString();
            this.AccName = drCoordinator["accname"].ToString();
            this.AccBSB = drCoordinator["accbsb"].ToString();
            this.AccNumber = drCoordinator["accnumber"].ToString();
            this.Gender = Convert.ToChar(drCoordinator["gender"].ToString());


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

        public bool Add()
        {
            try
            {
                if (Validation())
                {
                    return false;
                }

                DAL dal = new DAL();
                string queryString = "Insert into staff(FirstName, LastName, street, suburb, state, postcode, email, phone, accname, accbsb, accnumber, gender, Active) values ('" + this.FirstName + "', '" + this.LastName + "', '" + this.Street + "', '" + this.Suburb + "', '" + this.State + "', '" + this.Postcode + "', '" + this.Email + "', '" + this.Phone +
                                     "', '" + this.AccName + "', '" + this.AccBSB + "', '" + this.AccNumber + "', '" + this.Gender + "', 1);" +
                "SELECT LAST_INSERT_ID();";

                DataTable dt = dal.ReadRecords(queryString);
                queryString = "Insert into coordinator(coordinatorid, active) values (" + dt.Rows[0]["LAST_INSERT_ID()"] + ", 1);";

                if (dal.Query(queryString) == 1)
                {
                    MessageBox.Show(
                        this.FirstName + " " + this.LastName + " has been successfully added to the database.",
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

        public bool Update()
        {
            try
            {
                if (Validation())
                {

                    return false;
                }

                if (MessageBox.Show("Would you like to update " + this.FirstName + " " + this.LastName + "? This cannot be undone.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
                {
                    return false;
                }
                DAL dal = new DAL();
                string queryString = "Update staff set firstname = '" + this.FirstName + "', lastname = '" +
                                     this.LastName + "', street = '" + this.Street + "', suburb = '" + this.Suburb +
                                     "', state = '" + this.State + "', postcode = '" + this.Postcode + "', phone = '" +
                                     this.Phone + "', email = '" + this.Email +
                                     "', gender = '" + this.Gender + "', AccName = '" + this.AccName +
                                     "', AccNumber = '" + this.AccNumber + "', accbsb = '" + this.AccBSB +
                                     "' where staffid = " + this.StaffId + ";";

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
            if (this.CoordinatorId == 0)
            {

                MessageBox.Show("Could not find contractor to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (MessageBox.Show("Would you like to delete this coordinator?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return false;
            }
            try
            {
                DAL dal = new DAL();
                string queryString = "update coordinator set Active = 0 where coordinatorID = " + this.CoordinatorId + ";" +
                                     "update job set Active = 0 where coordinatorID = " + this.CoordinatorId + ";" +
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

        public void GetCoordinator(int coordinatorId)
        {
            try
            {
                DAL dal = new DAL();
                string queryString = "select coordinator.coordinatorid, staff.firstname, staff.lastname, staff.street, staff.suburb, staff.state, staff.postcode, staff.phone, staff.email, staff.gender, staff.accname, staff.accbsb, staff.accnumber from coordinator, staff where coordinator.coordinatorid = staff.staffid and coordinatorid = " + coordinatorId + "  and coordinator.active = 1;";
                DataTable dt = dal.ReadRecords(queryString);

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
                    MessageBoxImage.Error); throw;
            }


        }
        
    }


}
