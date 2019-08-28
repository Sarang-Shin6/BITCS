using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BITClientServer.Commands;
using BITClientServer.Model;
using BITClientServer.View;

namespace BITClientServer.ViewModel
{
    class JobCategoryViewModel : NotificationClass
    {
        // For JobCategory Selection

        #region Variables

        public ObservableCollection<JobCategory> JobCategoryCollection { get; } = new ObservableCollection<JobCategory>();

        public ObservableCollection<JobCategory> PrefJobCategoryCollection { get; } = new ObservableCollection<JobCategory>();

        public Contractor Contractor { get; set; }

        private JobCategory _selectedJobCategory = new JobCategory();
        public JobCategory SelectedJobCategory
        {
            get => _selectedJobCategory;
            set
            {
                _selectedJobCategory = value;
                OnPropertyChanged("SelectedJobCategory");
            }
        }

        private JobCategory _prefJobCategory;
        public JobCategory SelectedPrefJobCategory
        {
            get => _prefJobCategory;
            set
            {
                _prefJobCategory = value;
                OnPropertyChanged("SelectedPrefJobCategory");
            }
        }

        #endregion Variables

        #region Commands

        public RelayCommand AddJobCategoryToListCommand => new RelayCommand(AddJobCatToPref, true);

        public RelayCommand RemoveJobCategoryFromListCommand => new RelayCommand(RemoveJobCatFromPref, true);



        #endregion Commands

        #region Constructor

        public JobCategoryViewModel()
        {
            IntialiseJobCatList();

        }

        public JobCategoryViewModel(JobCategory jobcat)
        {
            IntialiseJobCatList();
            foreach (JobCategory j in JobCategoryCollection)
            {
                if (j.JobCatId == jobcat.JobCatId)
                {
                    this.SelectedJobCategory = j;
                }
            }
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Set Contractor for context for this ViewModel
        /// </summary>
        /// <param name="contractor"></param>

        public void SetContractor(Contractor contractor)
        {
            Contractor = contractor;
            GetPrefJobCategory();
            GetJobCategories();
        }

        /// <summary>
        /// Initialises the JobCat List. For use in Constructor
        /// </summary>
        
        public void IntialiseJobCatList()
        {
            DAL dal = new DAL();
            string queryString = "Select * from JobCategory where active = 1;";
            DataTable dt = new DataTable();
            dt = dal.ReadRecords(queryString);
            foreach (DataRow dr in dt.Rows)
            {
                JobCategory jobCategory =
                    new JobCategory(Convert.ToInt32(dr["JobCategoryid"]), dr["Description"].ToString(), Convert.ToBoolean(dr["Active"]));
                if (PrefJobCategoryCollection.Count == 0) // If PrefJobCategoryCollection is empty
                {
                    JobCategoryCollection.Add(jobCategory);
                }
            }

            SelectedJobCategory = JobCategoryCollection[0];
        }

        /// <summary>
        /// Gets the JobCat from the database checking if it is present in the Preferred JobCat Collection
        /// Used to Refresh the two grid views used for selecting JobCats when updating and adding a contractor.
        /// </summary>

        private void GetJobCategories()
        {
            JobCategoryCollection.Clear();
            DAL dal = new DAL();
            string queryString = "Select * from JobCategory where active = 1;";
            DataTable dt = new DataTable();
            dt = dal.ReadRecords(queryString);
            foreach (DataRow dr in dt.Rows)
            {
                JobCategory jobCategory = new JobCategory(Convert.ToInt32(dr["JobCategoryid"]), dr["Description"].ToString(), Convert.ToBoolean(dr["Active"]));
                bool jobCatNotInPref = true;
                if (PrefJobCategoryCollection.Count == 0) // If PrefJobCategoryCollection is empty
                {
                    JobCategoryCollection.Add(jobCategory);
                }
                else
                {
                    foreach (JobCategory prefJobCategory in PrefJobCategoryCollection)
                    // If selected JobCategory is present in PrefJobCategoryCollection don't add to JobCategoryCollection
                    {
                        if (prefJobCategory.JobCatId == jobCategory.JobCatId)
                        {
                            jobCatNotInPref = false;
                        }
                    }
                    // Else add Location to LocationCollection
                    if (jobCatNotInPref)
                    {
                        JobCategoryCollection.Add(jobCategory);
                    }

                    List<JobCategory> jobCatListToCheck = new List<JobCategory>();
                    foreach (JobCategory jobCat in JobCategoryCollection)
                    {
                       jobCatListToCheck.Add(jobCat); 
                    }

                    
                    foreach (JobCategory prefJobCategory in PrefJobCategoryCollection)

                    // Deals with Location present in both PrefLocationCollection and LocationCollection to remove from LocationCollection.
                    // Compares each location in LocationCollection to each location in PrefLocationCollection
                    // if locations are equal, remove location from LocationCollection
                    {
                        foreach (JobCategory jobCategoryToCheck in jobCatListToCheck)
                        {
                            bool toRemove = prefJobCategory.JobCatId == jobCategoryToCheck.JobCatId;
                            if (toRemove)
                            {
                                JobCategoryCollection.Remove(jobCategoryToCheck);
                            }
                        }
                    }
                }
            }
        }

        #region JobCat CRUD

        /// <summary>
        /// Add the corresponding location from preferred locations list with contractor into ContractorLocation.
        /// </summary>
        /// <param name="contractorId"></param></param>

        public bool AddContractorSkill(int contractorId)
        {
            if (PrefJobCategoryCollection.Count == 0)
                return false;
            foreach (JobCategory jobCategory in PrefJobCategoryCollection)
            {
                jobCategory.AddContractorSkill(contractorId);
            }

            return true;
        }

        /// <summary>
        /// Updates ContractorSkills by checking if JobCat in preferred JobCat is present in the database.
        /// Add if present in Preferred JobCat Collection and not in database
        /// Delete if present in database and not in Preferred JobCat
        /// </summary>
        /// <param></param>

        public bool UpdateContractorSkill()
        {
            if (PrefJobCategoryCollection.Count == 0)
                return false;
            List<JobCategory> dbJobCatCollection = JobCategory.GetJobCatFromPref(Contractor.ContractorId);
            foreach (JobCategory jobCat in dbJobCatCollection)
            {
                bool toDelete = true;
                foreach (JobCategory prefJobCat in PrefJobCategoryCollection)
                {
                    if (jobCat.JobCatId == prefJobCat.JobCatId)
                        toDelete = false;
                }
                if (toDelete)
                    jobCat.DeleteContractorSkill(Contractor.ContractorId);
            }
            
            foreach (JobCategory jobCategory in PrefJobCategoryCollection)
            {
                bool toAdd = true;
                foreach (JobCategory prefJobCat in dbJobCatCollection)
                {
                    if (prefJobCat.JobCatId == jobCategory.JobCatId)
                    {
                        toAdd = false;
                    }
                }
                if (toAdd)
                    jobCategory.AddContractorSkill(Contractor.ContractorId);
            }

            return true;
        }

        #endregion JobCat CRUD

        #region Preferred JobCat Collection Management

        /// <summary>
        /// Reads the ContractorSkills table to fill in contractor's Preferred JobCat List
        /// </summary>
        /// <param></param>
        private void GetPrefJobCategory()
        {
            DAL dal = new DAL();
            string queryString = "select jobcategory.jobcategoryid, jobcategory.description, contractorSkill.jobcategoryid, jobcategory.active from jobcategory, contractorskill where contractorSkill.contractorid =" +
                                 Contractor.ContractorId + " and contractorskill.jobcategoryid = jobcategory.jobcategoryid;";
            DataTable dt = new DataTable();
            dt = dal.ReadRecords(queryString);
            foreach (DataRow dr in dt.Rows)
            {
                JobCategory jobCategory = new JobCategory(dr);
                PrefJobCategoryCollection.Add(jobCategory);
            }

        }

        /// <summary>
        /// Add JobCat to Preferred JobCat Collection
        /// </summary>

        private void AddJobCatToPref()
        {
            if (SelectedJobCategory == null)
            {
                return;
            }
            PrefJobCategoryCollection.Add(SelectedJobCategory);
            GetJobCategories();
        }

        /// <summary>
        /// Remove JobCat from Preferred JobCat Collection
        /// </summary>
        /// 
        private void RemoveJobCatFromPref()
        {
            if (SelectedPrefJobCategory == null)
            {
                return;
            }
            PrefJobCategoryCollection.Remove(SelectedPrefJobCategory);
            GetJobCategories();
        }

        #endregion Preferred JobCat Collection Management


        #endregion Methods

    }
}
