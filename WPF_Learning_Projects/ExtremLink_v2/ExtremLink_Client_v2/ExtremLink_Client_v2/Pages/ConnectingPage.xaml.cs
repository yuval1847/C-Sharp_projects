using ExtremLink_Client_v2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ExtremLink_Client_v2.Pages
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

        // IP validation functions
        private bool IsLegalIP(string ipAddress)
        {
            // A function which check if the ip is according the IPv4 format.
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;

            string[] parts = ipAddress.Split('.');
            if (parts.Length != 4)
                return false;

            foreach (string part in parts)
            {
                if (!int.TryParse(part, out int num))
                    return false;

                if (num < 0 || num > 255)
                    return false;

                // Disallow leading zeros (e.g., "01", "001")
                if (part.Length > 1 && part.StartsWith("0"))
                    return false;
            }

            return true;
        }
        private void RaiseErrorLabel(string errorMsg)
        {
            // A function which change the error label according to the given error.
            wrongIpTextBlock.Foreground = Brushes.Red;
            wrongIpTextBlock.Text = errorMsg;
            wrongIpTextBlock.Visibility = Visibility.Visible;
        }
        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            // The function got called by clicking on the connect button.
            // The function gets nothing.
            // The function create an client instance and connect to the server.
            string serverIpAddr = ServerIpCustomTextBox.customTB.Text;
            if (!IsLegalIP(serverIpAddr))
            {
                this.RaiseErrorLabel("Error: This IP doesn't follow the IPv4 protocol's syntax!");
                return;
            }
            try
            {
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
            }
            // Note: It might take a little bit of time 
            catch (Exception ex) 
            {
                this.RaiseErrorLabel("Error: This IP isn't used by any server on this local network!");
                return;
            }
            this.contentMain.Content = new LoginPage(this.contentMain);
        }
    }
}