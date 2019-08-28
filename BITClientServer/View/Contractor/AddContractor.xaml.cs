using System.Windows;
using BITClientServer.Commands;
using BITClientServer.ViewModel;

namespace BITClientServer.View.Contractor
{
    /// <summary>
    /// Interaction logic for AddContractor.xaml
    /// </summary>
    public partial class AddContractor : Window
    {
        public AddContractor()
        {
            InitializeComponent();
            this.DataContext = new AddContractorViewModel();
        }
    }
}
