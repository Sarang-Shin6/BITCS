using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITClientServer.Commands;
using BITClientServer.View;

namespace BITClientServer.Model
{
    public class Client
    {
        public Company Company { get; set; } = new Company();
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public char Gender { get; set; } = GenderList.GenderCollection[0];
        public string Password { get; set; }

        public Client(int companyid, int clientid, string firstName, string lastName, string phone, string email, char gender)
        {
            this.Company.GetCompany(companyid);
            this.ClientId = clientid;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
            this.Email = email;
            this.Gender = gender;
        }

        public Client(int clientid)
        {
            this.ClientId = clientid;
        }

        public Client()
        {
        }

        public Client(DataRow drClient)
        {
            MapProperties(drClient);
        }

        private void MapProperties(DataRow drClient)
        {
            this.Company.GetCompany(Convert.ToInt32(drClient["companyid"]));
            this.ClientId = Convert.ToInt32(drClient["clientid"]);
            this.FirstName = drClient["firstName"].ToString();
            this.LastName = drClient["lastName"].ToString();
            this.Phone = drClient["phone"].ToString();
            this.Email = drClient["email"].ToString();
            this.Gender = Convert.ToChar(drClient["gender"]);
        }

        public bool Validation()
        {
            // Return true if validation fails
            List<string> list = new List<string>
            {
                this.FirstName, this.LastName, this.Phone, this.Email
            };
            if (ValidationClass.CheckIfNull(list))
            {
                MessageBox.Show("Please make sure all fields are filled out.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            if (ValidationClass.CheckPhone(this.Phone))
            {
                MessageBox.Show("Please enter a valid phone number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            if (ValidationClass.CheckEmail(this.Email))
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                string queryString = "Insert into client(FirstName, LastName, phone, email, gender, Active) values ('" + this.FirstName + "', '" + this.LastName + "', '" + this.Phone +
                                     "', '" + this.Email + "', '" + this.Gender + "', 1); SELECT LAST_INSERT_ID();";
                DataTable dt = dal.ReadRecords(queryString);

                if (dt.Rows.Count == 1)
                {
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

        public int Update()
        {
            if (Validation())
            {
                return 0;
            }
            try
            {
                DAL dal = new DAL();
                string queryString = "Update client set firstname = '" + this.FirstName + "', lastname = '" +
                                     this.LastName + "', phone = '" + this.Phone + "', email = '" + this.Email +
                                     "', gender = '" + this.Gender + "' where clientID = '" + this.ClientId + "';";



                if (dal.Query(queryString) >= 1)
                {
                    return 1;
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
            if (this.ClientId == 0)
            {
                MessageBox.Show("Could not find client to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            try
            {
                DAL dal = new DAL();
                string queryString = "update client set Active = 0 where clientID = " + this.ClientId + ";" +
                                     "update job set Active = 0 where clientID = " + this.ClientId + ";";


                if (dal.Query(queryString) >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong. Please try again later. " + e.Message, "Unsuccessful", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }
        }

        public void GetClient(int clientId)
        {
            try
            {
                DAL dal = new DAL();
                string queryString = "select * from client where clientid = " + clientId + " and active = 1;";
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
                    MessageBoxImage.Error);
                throw;
            }
        }

        public void GetClient(int clientId, bool FromJob)
        {
            try
            {
                DAL dal = new DAL();
                string queryString = "select * from client where clientid = " + clientId + " and active = 1;";
                DataTable dt = dal.ReadRecords(queryString);
                DataRow drClient = dt.Rows[0];
                if (dt.Rows.Count == 1)
                {
                    this.ClientId = Convert.ToInt32(drClient["clientid"]);
                    this.FirstName = drClient["firstName"].ToString();
                    this.LastName = drClient["lastName"].ToString();
                    this.Phone = drClient["phone"].ToString();
                    this.Email = drClient["email"].ToString();
                    this.Gender = Convert.ToChar(drClient["gender"]);
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong. Please try again later. " + e.Message, "Unsuccessful",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }
        }

    }
}
