using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BITClientServer.ViewModel;

namespace BITClientServer.View.Job
{
    /// <summary>
    /// Interaction logic for FindClient.xaml
    /// </summary>
    public partial class FindContractor : Window
    {
        public FindContractor(Model.Job job)
        {
            InitializeComponent();
            this.DataContext = new FindContractorViewModel(job);
        }
    }
}
