using System.Windows;
using BITClientServer.ViewModel;
using MySqlX.XDevAPI;

namespace BITClientServer.View.Client
{
    /// <summary>
    /// Interaction logic for AddJob.xaml
    /// </summary>
    public partial class AddJob : Window
    {
        public AddJob(int companyid, int clientid, int coordinatorid)
        {
            InitializeComponent();
            
            this.DataContext = new AddJobViewModel(companyid, clientid, coordinatorid);
            //TODO

        }
    }
}
