using ExtremLink_Client.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for ConnectingPage.xaml
    /// </summary>
    public partial class ConnectingPage : UserControl
    {
        private ContentControl contentMain;
        private Client client;

        public ConnectingPage(ContentControl contentMain)
        {
            this.contentMain = contentMain;
            InitializeComponent();
        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            // The function got called by clicking on the connect button.
            // The function gets nothing.
            // The function create an client instance and connect to the server.
            Console.WriteLine("Click");
            string ServerIpAddr = ServerIpCustomTextBox.Text;
            this.client = new Client(ServerIpAddr, 1847);
            this.client.Start();
            this.contentMain.Content = new LoginPage(contentMain, client);
            
        }
    }
}
