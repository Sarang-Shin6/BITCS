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

namespace BITClientServer.View.Job
{
    /// <summary>
    /// Interaction logic for UpdateJob.xaml
    /// </summary>
    public partial class UpdateJob : Window
    {
        public UpdateJob(Model.Job job)
        {
            InitializeComponent();
            this.DataContext = new UpdateJobViewModel(job);
        }

    }
}
