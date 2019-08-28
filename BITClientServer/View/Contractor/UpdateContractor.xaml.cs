using System.Windows;
using System.Windows.Media.TextFormatting;
using BITClientServer.Model;
using BITClientServer.ViewModel;

namespace BITClientServer.View.Contractor
{
    /// <summary>
    /// Interaction logic for UpdateClient.xaml
    /// </summary>
    public partial class UpdateContractor : Window
    {
        public UpdateContractor(Model.Contractor contractor)
        {
            InitializeComponent();
            this.DataContext = new UpdateContractorViewModel(contractor);
        }
    }
}
