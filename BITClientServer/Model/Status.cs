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
    public class Status
    {
        public int StatusId { get; set; }
        public string Description { get; set; }

        public Status(){}

        public Status(int statusId)
        {
            GetStatus(statusId);
        }

        public void GetStatus(int statusId)
        {
            DAL dal = new DAL();
            string queryString = "select * from Status where statusid = " + statusId + ";";
            DataTable dt = dal.ReadRecords(queryString);
            try
            {
                if (dt.Rows.Count == 1)
                {
                    this.StatusId = Convert.ToInt32(dt.Rows[0][0]);
                    this.Description = dt.Rows[0][1].ToString();
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong whilst obtaining status. Please try again later. " + e.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
