using ExtremLink_Client.Classes;
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

namespace ExtremLink_Client.Pages
{
    /// <summary>
    /// Interaction logic for ChoosingRulePage.xaml
    /// </summary>
    public partial class ChoosingRulePage : UserControl
    {
        // Attirbutes:
        private ContentControl contentMain;
        private Client client;
        public ChoosingRulePage(ContentControl contentMain, Client client)
        {
            this.contentMain = contentMain;
            this.client = client;
            InitializeComponent();
        }

        private void AttackerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.contentMain.Content = new ControlPage(this.contentMain, this.client);
        }

        private void VictimBtn_Click(object sender, RoutedEventArgs e)
        {
            this.contentMain.Content = new SharingScreenPage(this.contentMain, this.client);
        }
    }
}
