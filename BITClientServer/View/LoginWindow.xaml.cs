using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BITClientServer.ViewModel;

namespace BITClientServer.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtEmail.Focus();
            this.DataContext = new LoginViewModel();

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }
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
