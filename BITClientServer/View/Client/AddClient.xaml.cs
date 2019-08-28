using System.Windows;
using BITClientServer.ViewModel;
using MySqlX.XDevAPI;

namespace BITClientServer.View.Client
{
    /// <summary>
    /// Interaction logic for AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        public AddClient()
        {
            InitializeComponent();
            this.DataContext = new AddClientViewModel();
        }
    }
}
