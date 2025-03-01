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
    /// Interaction logic for ControlPage.xaml
    /// </summary>
    public partial class ControlPage : UserControl
    {

        // Attirbutes:
        private ContentControl contentMain;
        private Client client;
        private CustomMouse customMouse = CustomMouse.CustomMouseInstance;
        private CustomKeyboard customKeyboard = CustomKeyboard.CustomKeyboardInstance;
        private Session currentSession;
        private Thread recordingThread;


        public ControlPage(ContentControl contentMain, Client client)
        {
            this.contentMain = contentMain;
            this.client = client;
            this.clientIpTextBlock.Text = $"Client's IP: {this.client.ClientIpAddress}";
            InitializeComponent();
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void RecordBtn_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
