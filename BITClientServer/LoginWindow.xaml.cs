using System.Windows;

namespace BITClientServer.Model
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            View.MainMenu mainMenu = new View.MainMenu();
            //mainMenu.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainMenu.Show();
            this.Close();
        }
    }
}
