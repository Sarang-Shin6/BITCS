using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using BITClientServer.View;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace BITClientServer.Model
{
    public class Job
    {
        #region Variables

        public int JobId { get; set; }
        public Company Company { get; set; } = new Company();
        public Client Client { get; set; } = new Client();
        public Status Status { get; set; } = new Status();
        public JobCategory JobCategory { get; set; } = new JobCategory();
        public DateTime StartDate { get; set; }

        public string StringStartDate
        {
            get
            {
                if (StartDate == DateTime.Parse("0001/01/01"))
                {
                    return "N/A";
                }
                return DateConverter.ToDDMMYYY(StartDate.ToShortDateString());
            }
        }

        public int Duration { get; set; }

        public string StringDuration
        {
            get => Duration.ToString();
            set
            {
                try
                {
                    Duration = Convert.ToInt32(value);
                }
                catch (Exception e)
                {
                    Duration = 0;
                }
                
            }
        }

        public DateTime PreferredEndDate { get; set; }

        public string StringPreferredEndDate
        {
            get
            {
                if (PreferredEndDate == DateTime.Parse("0001/01/01"))
                {
                    return "N/A";
                }
                return DateConverter.ToDDMMYYY(PreferredEndDate.ToShortDateString());
            }
        }

        public int EstimatedDuration { get; set; }
        public string StringEstimatedDuration
        {
            get => EstimatedDuration.ToString();
            set
            {
                try
                {
                    EstimatedDuration = Convert.ToInt32(value);
                }
                catch (Exception e)
                {
                    EstimatedDuration = 0;
                }

            }
        }

        public string Description { get; set; }

        public string Feedback { get; set; }


        public int LoggedKm { get; set; }
        public string StringLoggedKm
        {
            get => LoggedKm.ToString();
            set
            {
                try
                {
                    LoggedKm = Convert.ToInt32(value);
                }
                catch (Exception e)
                {
                    LoggedKm = 0;
                }

            }
        }

        public short JobRating { get; set; }
        public string StringJobRating
        {
            get => JobRating.ToString();
            set
            {
                try
                {
                    JobRating = Convert.ToInt16(value);
                }
                catch (Exception e)
                {
                    JobRating = 0;
                }

            }
        }

        public Contractor Contractor { get; set; } = new Contractor();

        public string StringContractor
        {
            get
            {
                if (Contractor.ContractorId == 0)
                {
                    return "N/A";
                }
                return Contractor.ContractorId.ToString();

            }
            set
            {
                try
                {
                    Contractor.ContractorId = Convert.ToInt32(value);
                }
                catch (Exception e)
                {
                    Contractor.ContractorId = 0;
                }

            }

            
        }

        public Coordinator Coordinator { get; set; } = new Coordinator();

        #endregion Variables

        #region Constructor

        public Job(int companyid, int clientid, int statusid, int jobcatid, DateTime startdate, int duration,
            DateTime preferredenddate, int estimatedduration, string description, string feedback, int loggedkm,
            short jobrating, int contractorid, int coordinatorid)
        {
            this.Company.GetCompany(companyid);
            this.Client.GetClient(clientid, true);
            this.Status.GetStatus(statusid);
            this.JobCategory.GetJobCat(jobcatid);
            this.StartDate = startdate;
            this.Duration = duration;
            this.PreferredEndDate = preferredenddate;
            this.EstimatedDuration = estimatedduration;
            this.Description = description;
            this.Feedback = feedback;
            this.LoggedKm = loggedkm;
            this.JobRating = jobrating;
            this.Contractor.GetContractor(contractorid);
            this.Coordinator.GetCoordinator(coordinatorid);
        }

        public Job(int jobId)
        {
            GetJob(jobId);
        }

        public Job() { }

        public Job(int companyid, int clientid, int coordinatorid)
        {
            this.Company.GetCompany(companyid);
            this.Client.GetClient(clientid, true);
            this.Coordinator.GetCoordinator(coordinatorid);
        }

        public Job(DataRow drJob)
        {
            MapProperties(drJob);
        }

        #endregion Constructor

        #region Validation

        private bool ValidationUpdate()
        {
            List<string> list = new List<string>
            {
                this.EstimatedDuration.ToString(),
                this.Description
            };
            if (ValidationClass.CheckIfNull(list))
            {
                MessageBox.Show("Please make sure Estimated Duration and Description are not empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            if (StringDuration != "" && ValidationClass.CheckNumber(StringDuration))
            {
                MessageBox.Show("Invalid Data Entry, Duration input must be a number.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            if (ValidationClass.CheckNumber(StringEstimatedDuration))
            {
                MessageBox.Show("Invalid Data Entry, Duration input must be a number.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            if (StringJobRating != "" && ValidationClass.CheckNumber(StringJobRating))
            {
                MessageBox.Show("Invalid Data Entry, Job Rating input must be a number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            if (StringLoggedKm != "" && ValidationClass.CheckNumber(StringLoggedKm))
            {
                MessageBox.Show("Invalid Data Entry, Logged KM's input must be a number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            return false;
        }

        private bool ValidationAdd(string date)
        {
            List<string> list = new List<string>
            {
                this.Description
            };
            if (ValidationClass.CheckIfNull(list))
            {
                MessageBox.Show("There must be a description for the job request.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            if (ValidationClass.CheckDate(DateTime.Parse(date)))
            {
                MessageBox.Show("Due date cannot be in the past.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            return false;
        }

        #endregion Validation

        #region Methods

        private void MapProperties(DataRow drJob)
        {
            this.JobId = Convert.ToInt32(drJob["JobID"]);
            this.Company.GetCompany(Convert.ToInt32(drJob["CompanyId"]));
            this.Client.GetClient(Convert.ToInt32(drJob["ClientID"]), true);

            this.Status = new Status(Convert.ToInt32(drJob["StatusID"]));
            this.JobCategory = new JobCategory(Convert.ToInt32(drJob["JobCategoryID"]));
            if (drJob["StartDate"] != DBNull.Value)
            {
                this.StartDate = Convert.ToDateTime(drJob["StartDate"].ToString());
            }

            if (drJob["Duration"] != DBNull.Value)
            {
                this.Duration = Convert.ToInt32(drJob["Duration"]);
            }

            this.PreferredEndDate = Convert.ToDateTime(drJob["PreferredEndDate"].ToString());
            if (drJob["EstimatedDuration"] != DBNull.Value)
            {
                this.EstimatedDuration = Convert.ToInt32(drJob["EstimatedDuration"]);
            }


            this.Description = drJob["Description"].ToString();
            if (drJob["Feedback"] != DBNull.Value)
            {
                this.Feedback = drJob["Feedback"].ToString();
            }

            if (drJob["LoggedKm"] != DBNull.Value)
            {
                this.LoggedKm = Convert.ToInt32(drJob["LoggedKm"]);
            }

            if (drJob["JobRating"] != DBNull.Value)
            {
                this.JobRating = Convert.ToInt16(drJob["JobRating"]);
            }

            if (drJob["ContractorID"] != DBNull.Value)
            {
                this.Contractor.GetContractor(Convert.ToInt32(drJob["ContractorID"]));
            }

            if (drJob["CoordinatorID"] != DBNull.Value)
            {
                this.Coordinator.GetCoordinator(Convert.ToInt32(drJob["CoordinatorID"]));
            }
        }

        private void GetJob(int jobId)
        {
            DAL dal = new DAL();
            string queryString = "select * from job where jobid = " + jobId + " and active = 1;";
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

        //public bool Add()
        //{
        //    if (ValidationAdd())
        //    {
        //        MessageBox.Show("Invalid Data Entry.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }

        //    DAL dal = new DAL();
        //    string queryString = "insert into job(companyid, clientid, statusid, jobcategoryid, startdate, duration, preferredenddate, estimatedduration, description, feedback, loggedkm, jobrating, contractorid, coordinatorid, Active) values ('" + this.Company.CompanyId + "', '" + this.Client.ClientId + "', '" + this.Status.StatusId + "', '" + this.JobCategory.JobCatId + "', '" + this.StringStartDate + "', '" + this.Duration + "', '" + this.StringPreferredEndDate + "', '" + this.EstimatedDuration + "', '" + this.Description + "', '" + this.Feedback + "', '" + this.StringLoggedKm + "', '" + this.StringJobRating + "', '" + this.Contractor.ContractorId + "', '" + this.Coordinator.CoordinatorId + "', 1);";

        //    try
        //    {
        //        if (dal.Query(queryString) == 1)
        //        {
        //            MessageBox.Show("The job has been successfully added to the database.", "Success!", MessageBoxButton.OK, MessageBoxImage.Hand);
        //            return true;
        //        }
        //        else
        //        {
        //            throw new Exception();
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show("Something went wrong. Please try again later. " + e.Message, "Unsuccessful", MessageBoxButton.OK,
        //            MessageBoxImage.Error);
        //        throw;
        //    }
        //}

        public bool Add(JobCategory jobCat, string endDate)
        {
            if (ValidationAdd(endDate))
            {
                return false;
            }
            DAL dal = new DAL();
            string queryString = "insert into job(companyid, clientid, statusid, jobcategoryid,  preferredenddate, description, coordinatorid, Active) values ('" + this.Company.CompanyId + "', '" + this.Client.ClientId + "', 1, '" + jobCat.JobCatId + "', '" + endDate + "', '" + this.Description + "', " + Session.CoordinatorId + ", 1);";

            try
            {
                if (dal.Query(queryString) == 1)
                {
                    MessageBox.Show("The job has been successfully added to the database.", "Success!", MessageBoxButton.OK, MessageBoxImage.Hand);
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
            if (ValidationUpdate())
            {
                return false;
            }

            if (MessageBox.Show("Would you like to update this job? This cannot be undone.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return false;
            }
            DAL dal = new DAL();
            string queryString = "update job set statusid = '" + this.Status.StatusId + "', duration = " + this.Duration + ", EstimatedDuration = " + this.EstimatedDuration + ", description = '" + this.Description + "', feedback = '" + this.Feedback + "', LoggedKm = " + this.LoggedKm + ", JobRating = " + this.JobRating + ", Coordinatorid = " + this.Coordinator.CoordinatorId + " where jobid = " + this.JobId + ";";

            try
            {
                if (dal.Query(queryString) >= 1)
                {
                    MessageBox.Show("The job has been successfully updated in the database.",
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
            if (this.JobId == 0)
            {

                MessageBox.Show("Could not find job to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (MessageBox.Show("Would you like to delete this job?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return false;
            }
            DAL dal = new DAL();
            string queryString = "update job set active = 0 where jobid = " + this.JobId + ";";

            try
            {
                if (dal.Query(queryString) >= 1)
                {
                    MessageBox.Show("The job has been successfully deleted from the database.",
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


        public void SetJobCategory(JobCategory jobcategory)
        {
            this.JobCategory.JobCatId = jobcategory.JobCatId;
        }

        public void SetStatus(Status status)
        {
            this.Status.StatusId = status.StatusId;
        }

        public void SetDates(string selectedStartDate, string selectedEndDate)
        {
            this.StartDate = Convert.ToDateTime(selectedStartDate);
            this.PreferredEndDate = Convert.ToDateTime(selectedEndDate);
        }

        public bool Update(string date, string field)
        {
            // Used for StartDate and DueDate Updates

            DAL dal = new DAL();

            string fieldText = "preferred end date";
            if (field == "startdate")
                fieldText = "start date";

            string queryString = "update job set " + field + " = '" + DateConverter.ToSQL(date) + "' where jobid = " + this.JobId + ";";
            try
            {
                if (dal.Query(queryString) == 1)
                {
                    MessageBox.Show("The " + fieldText + " has been successfully updated in the database.",
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
                return false;
            }
        }
        #endregion Methods
    }
}
