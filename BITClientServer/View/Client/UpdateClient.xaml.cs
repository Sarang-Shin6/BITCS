using System.Windows;
using BITClientServer.Model;
using BITClientServer.ViewModel;

namespace BITClientServer.View.Client
{
    /// <summary>
    /// Interaction logic for UpdateClient.xaml
    /// </summary>
    public partial class UpdateClient : Window
    {
        public UpdateClient(Model.Client client)
        {
            InitializeComponent();
            this.DataContext = new UpdateClientViewModel(client);
        }

    }
}
