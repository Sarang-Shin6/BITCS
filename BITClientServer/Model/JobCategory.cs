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
    public class JobCategory
    {
        #region Variables
        public int JobCatId { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        #endregion Variables

        #region Contructors

        public JobCategory(int jobCatId, string description, bool active)
        {
            this.JobCatId = jobCatId;
            this.Description = description;
            this.Active = active;
        }

        public JobCategory() { }

        public JobCategory(DataRow drContractor)
        {
            MapProperties(drContractor);
        }

        public JobCategory(int jobCatId)
        {
            GetJobCat(jobCatId);
        }

        #endregion Constructors

        #region Validation
        private bool Validation()
        {
            List<string> list = new List<string> { this.Description };
            if (ValidationClass.CheckIfNull(list))
            {
                return true;
            }

            return false;
        }

        private bool Validation(int ContractorId)
        {
            List<string> list = new List<string> { this.Description };
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
            this.JobCatId = Convert.ToInt32(dr["jobcategoryid"]);
            this.Description = dr["description"].ToString();
            this.Active = Convert.ToBoolean(dr["Active"]);
        }

        public void AddContractorSkill(int contractorId)
        {
            try
            {
                if (Validation(contractorId))
                {
                    MessageBox.Show("Invalid Data Entry.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (contractorId == 0)
                    return;
                DAL dal = new DAL();

                string queryString = "Insert into ContractorSkill(jobcategoryid, contractorid, active) values (" +
                                     this.JobCatId + ", " + contractorId + ", 1);";
                dal.Query(queryString);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong. Please try again later. " + e.Message, "Unsuccessful", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }


        }

        public void DeleteContractorSkill(int contractorId)
        {
            try
            {
                if (this.JobCatId == 0)
                {
                    MessageBox.Show("Invalid Data Entry.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (contractorId == 0)
                    return;
                DAL dal = new DAL();
                string queryString = "Update ContractorSkill set active = 0 where contractorid = " + contractorId +
                                     " and jobcategoryid = " + this.JobCatId + ";";
                dal.Query(queryString);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong. Please try again later. " + e.Message, "Unsuccessful", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }

        }

        public static List<JobCategory> GetJobCatFromPref(int contractorId)
        {
            //Get All of Contractor's Skills from Database

            try
            {
                DAL dal = new DAL();
                string queryString = "Select * from contractorskill where contractorid = " + contractorId + " and active = 1;";
                DataTable dtConJobCatDB = dal.ReadRecords(queryString);
                DataTable dt;
                List<JobCategory> tempJobCatCollection = new List<JobCategory>();
                foreach (DataRow drConJobCat in dtConJobCatDB.Rows)
                {
                    queryString = "Select * from jobcategory where jobcategoryid = " + Convert.ToInt32(drConJobCat["jobcategoryid"])
                    + " and active = 1;";
                    dt = dal.ReadRecords(queryString);
                    foreach (DataRow dr in dt.Rows)
                    {
                        JobCategory newJobCat = new JobCategory(dr);
                        tempJobCatCollection.Add(newJobCat);
                    }
                }


                return tempJobCatCollection;
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong. Please try again later. " + e.Message, "Unsuccessful", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }

        }

        public void GetJobCat(int jobCatId)
        {
            DAL dal = new DAL();
            string queryString = "select * from jobcategory where jobcategoryid = " + jobCatId + " and active = 1;";
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
        #region CRUD

        public bool Add()
        {
            if (Validation() || this.JobCatId == 0)
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

            string queryString = "Insert into JobCategory(description, Active) values ('" + this.Description + "', " + activeBit + ");";

            try
            {
                if (dal.Query(queryString) == 1)
                {
                    MessageBox.Show("The JobCategory has been successfully added to the database.", "Success!", MessageBoxButton.OK, MessageBoxImage.Hand);
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
            if (Validation() || this.JobCatId == 0)
            {
                MessageBox.Show("Invalid Data Entry.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (MessageBox.Show("Would you like to update this job category? This cannot be undone.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return false;
            }
            DAL dal = new DAL();
            var activeBit = 1;
            if (Active == false)
            {
                activeBit = 0;
            }
            string queryString = "Update JobCategory set Description = '" + this.Description + "', Active = " +
                                 activeBit + " where JobCategoryid = " + this.JobCatId + ";";


            try
            {
                if (dal.Query(queryString) >= 1)
                {
                    MessageBox.Show("The JobCategory has been successfully updated in the database.",
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
            if (this.JobCatId == 0)
            {
                MessageBox.Show("Could not find contractor to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (MessageBox.Show("Would you like to delete this job category?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return false;
            }
            DAL dal = new DAL();
            string queryString = "delete from JobCategory where JobCategoryid = " + this.JobCatId + ";";

            try
            {
                if (dal.Query(queryString) == 1)
                {
                    MessageBox.Show("The JobCategory has been successfully deleted from the database.",
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

        public void NullJobCategoryId()
        {
            this.JobCatId = -1;
        }

        public void SetJobCategory(JobCategory changeJobCategory)
        {
            this.Description = changeJobCategory.Description;
            this.Active = changeJobCategory.Active;
        }

#endregion Methods
    }
}
