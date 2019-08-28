using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using BITClientServer.View;

namespace BITClientServer.Model
{
    class ContractorAvailability
    {

        public int AvailabilityId { get; set; }
        public int ContractorId { get; set; }

        public string Date { get; set; } = DateTime.Today.ToShortDateString();

        public string StartTime { get; set; } = "09:00 AM";

        public int Hours { get; set; } = 5;

        public string StringHours
        {
            get => Hours.ToString();
            set => Hours = Convert.ToInt32(value);
        }

        public ContractorAvailability() { }

        public ContractorAvailability(DataRow dr)
        {
            MapProperties(dr);
        }
       
        private void MapProperties(DataRow dr)
        {
            this.AvailabilityId = Convert.ToInt32(dr["contractoravailabilityid"]);
            this.ContractorId = Convert.ToInt32(dr["contractorid"]);
            this.Date = Convert.ToDateTime(dr["date"]).ToShortDateString();
            this.StartTime = dr["starttime"].ToString();
            this.Hours = Convert.ToInt32(dr["hours"]);
        }

        private bool Validation()
        {
            List<string> list = new List<string>
            {
                this.ContractorId.ToString(),
                this.StartTime,
                this.Hours.ToString()
            };
            if (ValidationClass.CheckIfNull(list) || ValidationClass.CheckDate(DateTime.Parse(Date)) || ValidationClass.CheckNumber(StringHours))
            {
                return true;
            }


            //if (!Regex.IsMatch(this.Hours.ToString(), "^[0-9]+$"))
            //{
            //    return true;
            //}

            return false;
        }

        public bool Add()
        {
            if (Validation())
            {
                MessageBox.Show("Invalid Data Entry.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            DAL dal = new DAL();

            string queryString = "Insert into ContractorAvailability(contractorid, date, starttime, hours, active) values (" + this.ContractorId + ", '" + DateConverter.ToSQL(this.Date) + "', '" + this.StartTime + "', " + this.Hours + ", 1);";
            

            try
            {
                if (dal.Query(queryString) == 1)
                {
                    MessageBox.Show("The record has been successfully added to the database.", "Success!", MessageBoxButton.OK, MessageBoxImage.Hand);
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
                MessageBox.Show("Invalid Data Entry.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Validation())
            {
                MessageBox.Show("Invalid Data Entry.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (MessageBox.Show("Would you like to update this record? This cannot be undone.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return false;
            }
            DAL dal = new DAL();

            string queryString = "Update ContractorAvailability set contractorid = " + this.ContractorId + ", date = '" +
                                 DateConverter.ToSQL(this.Date) + "', starttime = '" + this.StartTime + "', Hours = " +
                                 this.Hours + ", active = 1 where ContractorAvailabilityid = " + this.AvailabilityId + ";";


            try
            {
                if (dal.Query(queryString) >= 1)
                {
                    MessageBox.Show("The record has been successfully updated in the database.",
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
            if (this.AvailabilityId == 0)
            {
                MessageBox.Show("Could not find record to delete.", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            if (MessageBox.Show("Would you like to delete this record?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return false;
            }
            DAL dal = new DAL();
            string queryString = "update ContractorAvailability set active = 0 where ContractorAvailabilityid = " + this.AvailabilityId + ";";

            try
            {
                if (dal.Query(queryString) >= 1)
                {
                    MessageBox.Show("The Record has been successfully deleted from the database.",
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

        public void SetContractor(Contractor contractor)
        {
            this.ContractorId = contractor.ContractorId;
        }

        public void SetContractorAvailability(ContractorAvailability contractorAvailability)
        {
            //this.AvailabilityId = contractorAvailability.AvailabilityId;
            this.ContractorId = contractorAvailability.ContractorId;
            this.Date = contractorAvailability.Date;
            this.StartTime = contractorAvailability.StartTime;
            this.Hours = contractorAvailability.Hours;
        }

        
    }
}
