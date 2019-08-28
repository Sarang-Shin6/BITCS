using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITClientServer.Commands;
using BITClientServer.Model;
using BITClientServer.View;

namespace BITClientServer.ViewModel
{
    class FindContractorViewModel : NotificationClass
    {
        private Contractor _contractor = new Contractor();

        public Contractor SelectedContractor
        {
            get => _contractor;
            set
            {
                _contractor = value;
                OnPropertyChanged("SelectedContractor");
            }
        }

        public Job Job { get; set; }

        public ObservableCollection<Contractor> ContractorCollection { get; set; } = new ObservableCollection<Contractor>();

        public RelayCommand<Window> AssignCommand => new RelayCommand<Window>(AssignContractor, true);

        public FindContractorViewModel(Job job)
        {
            Job = new Job(job.JobId);
            FillList();
        }

        private void FillList()
        {
            DAL dal = new DAL();
            string queryString = "call usp_GetContractors(" + Job.JobCategory.JobCatId + ", " + Job.Company.Location.LocationId + ")";
            DataTable dtCon = dal.ReadRecords(queryString);

            queryString = "call usp_GetContractorsHours(" + Job.EstimatedDuration + ", '" + DateConverter.ToSQL(Job.PreferredEndDate.ToShortDateString()) + "')";
            DataTable dtTotalHours = dal.ReadRecords(queryString);

            queryString = "call usp_GetContractorsTotalJobs()";
            DataTable dtTotalJobs = dal.ReadRecords(queryString);

            queryString = "call usp_GetRejections(" + Job.Contractor.ContractorId + ", " + Job.JobId + ")";
            DataTable dtRejection = dal.ReadRecords(queryString);

            bool toAdd = false;

            foreach (DataRow drCon in dtCon.Rows) //check location and skill
            {

                foreach (DataRow drTotalHours in dtTotalHours.Rows) //check total hour availability between today and preferredenddate
                {
                    MessageBox.Show(drTotalHours["contractorid"].ToString());
                    if (Convert.ToInt32(drCon["contractorid"]) == Convert.ToInt32(drTotalHours["contractorid"]))
                    {
                        toAdd = true;
                    }
                }

                foreach (DataRow drTotalJob in dtTotalJobs.Rows) // check total job number, must be under 2
                {
                    if (Convert.ToInt32(drCon["contractorid"]) == Convert.ToInt32(drTotalJob["contractorid"]))
                    {
                        toAdd = true;
                    }
                }

                foreach (DataRow drRejection in dtRejection.Rows)
                {
                    if (Convert.ToInt32(drCon["contractorid"]) == Convert.ToInt32(drRejection["contractorid"]))
                    {
                        toAdd = false;
                    }
                }


                if (toAdd)
                {
                    Contractor contractor = new Contractor(Convert.ToInt32(drCon["contractorid"]));
                    contractor.GetRating();
                    ContractorCollection.Add(contractor);
                }
            }
        }


        private void AssignContractor(Window window)
        {
            if (SelectedContractor == null)
            {
                return;
            }
            DAL dal = new DAL();
            string queryString = "";
            if (Job.Status.StatusId == 2)
                queryString = "update job set contractorid = " + SelectedContractor.ContractorId + ", statusid = 3 where jobid = " + Job.JobId;
            else
                queryString = "update job set contractorid = " + SelectedContractor.ContractorId + " where jobid = " + Job.JobId;

            try
            {
                if (dal.Query(queryString) == 1)
                {
                    MessageBox.Show("The Contractor has been successfully Requested.",
                        "Success!", MessageBoxButton.OK, MessageBoxImage.Hand);
                    window?.Close();
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
