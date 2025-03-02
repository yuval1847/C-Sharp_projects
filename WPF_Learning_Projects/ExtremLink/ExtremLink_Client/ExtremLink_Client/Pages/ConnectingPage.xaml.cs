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
            string serverIpAddr = ServerIpCustomTextBox.Text;
            switch (User.UserInstance.TypeOfClient)
            {
                case TypeOfClient.Attacker:
                    Attacker.AttackerInstance.ConnectToServer(serverIpAddr);
                    Attacker.AttackerInstance.Start();
                    break;
                case TypeOfClient.Victim:
                    Victim.VictimInstance.ConnectToServer(serverIpAddr);
                    Victim.VictimInstance.Start();
                    break;
            }
            this.contentMain.Content = new LoginPage(this.contentMain);
        }

    }
}
