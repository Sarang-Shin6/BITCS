using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITClientServer.Commands;
using BITClientServer.Model;
using BITClientServer.View;
using Org.BouncyCastle.Asn1.Crmf;

namespace BITClientServer.ViewModel
{
    public class LoginViewModel
    {
        private string _username;
        public string Username
        {
            get { return _username == "Username" ? "" : _username; }
            set { _username = value == "" ? "Username" : value; }
        }

        public string Password { private get; set; } = "Password";

        public RelayCommand<Window> LoginCommand => new RelayCommand<Window>(CheckLogin, true);
        public RelayCommand<Window> CloseCommand => new RelayCommand<Window>(CloseWindow, true);

        private DataTable dt;

        public bool Login()
        {
            DAL dal = new DAL();
            String queryString =
                "select c.admin, s.staffid from staff as s inner join coordinator as c on c.coordinatorid = s.staffid where email = lower('" +
                this.Username + "') and password = SHA1('" + this.Password + "');";
            dt = dal.ReadRecords(queryString);
            if (dt.Rows.Count == 1)
            {
                return true;
            }
            return false;
        }
        

        private void CheckLogin(Window window)
        {
            Login();   
            if (dt.Rows.Count == 1)
            {
                Session.CoordinatorId = dt.Rows[0][1].ToString();
                Session.Admin = Convert.ToBoolean(dt.Rows[0][0]);
                OpenMainWindow();
                window?.Close();
            }
            else
            {
                MessageBox.Show(
                    "Invalid Username and Password. Please contact the System Administrator if you have lost your login details.",
                    "Unsuccessful Login", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void OpenMainWindow()
        {
            MainMenu mm = new MainMenu();
            mm.Show();
        }

        private void CloseWindow(Window window)
        {
            window?.Close();
        }


    }
}
