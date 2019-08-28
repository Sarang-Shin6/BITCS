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
using BITClientServer.View.Client;
using BITClientServer.View.Contractor;
using BITClientServer.View.Job;
using BITClientServer.ViewModel;

namespace BITClientServer.View
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
            tbClient.DataContext = new ClientViewModel();
            tbContractor.DataContext = new ContractorViewModel();
            tbCoordinator.DataContext = new CoordinatorViewModel();
            tbJob.DataContext = new JobViewModel();
            gbLocation.DataContext = new LocationManagerViewModel();
            gbJobCat.DataContext = new JobCategoryManagerViewModel();
        }

        #region Textbox Select All
        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/564b5731-af8a-49bf-b297-6d179615819f/how-to-selectall-in-textbox-when-textbox-gets-focus-by-mouse-click?forum=wpf

        private void SelectAddress(object sender, RoutedEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb != null)
            {
                tb.SelectAll();
            }
        }



        private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb != null)
            {
                if (!tb.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    tb.Focus();
                }
            }
        }
        #endregion Textbox Select All
    }
}
