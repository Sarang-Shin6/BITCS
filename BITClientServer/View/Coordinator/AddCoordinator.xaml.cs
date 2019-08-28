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

namespace BITClientServer.View.Coordinator
{
    /// <summary>
    /// Interaction logic for AddCoordinator.xaml
    /// </summary>
    public partial class AddCoordinator : Window
    {
        public AddCoordinator()
        {
            InitializeComponent();
            this.DataContext = new AddCoordinatorViewModel();
        }
    }
}
