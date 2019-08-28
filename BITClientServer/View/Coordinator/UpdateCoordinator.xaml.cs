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
using BITClientServer.Model;
using BITClientServer.ViewModel;

namespace BITClientServer.View.Coordinator
{
    /// <summary>
    /// Interaction logic for UpdateCoordinator.xaml
    /// </summary>
    public partial class UpdateCoordinator : Window
    {
        public UpdateCoordinator(Model.Coordinator coordinator)
        {
            InitializeComponent();
            this.DataContext = new UpdateCoordinatorViewModel(coordinator);

        }
    }
}
