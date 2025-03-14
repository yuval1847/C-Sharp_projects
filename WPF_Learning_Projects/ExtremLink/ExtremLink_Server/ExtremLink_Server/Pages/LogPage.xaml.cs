using ExtremLink_Server.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtremLink_Server.Pages
{
    /// <summary>
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class LogPage : UserControl
    {

        // Attributes:
        private ContentControl contentMain;
        public ContentControl ContentMain
        {
            get { return this.contentMain; }
        }


        public LogPage(ContentControl contentMain)
        {
            this.contentMain = contentMain;
            InitializeComponent();
        }

        private void ClearLogsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
