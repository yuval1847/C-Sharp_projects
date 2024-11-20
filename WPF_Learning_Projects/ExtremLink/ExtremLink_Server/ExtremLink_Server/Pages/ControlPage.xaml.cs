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
    /// Interaction logic for ControlPage.xaml
    /// </summary>
    public partial class ControlPage : UserControl
    {
        private ContentControl contentMain;
        private Server server;
        public ControlPage(ContentControl contentMain, Server server)
        {
            this.contentMain = contentMain;
            this.server = server;
            //MessageBox.Show(this.server.ClientIpAddress);
            InitializeComponent();
            this.clientIpTextBlock.Text = $"Client's IP: {this.server.ClientIpAddress}";
        }

        private void StartStreamBtnClick(object sender, RoutedEventArgs e)
        {
            this.server.SendMessage(this.server.ClientTcpSocket, "&", "StartSendFrames");
            while (true)
            {
                if (this.server.Respond == "frame_received")
                {
                    frameImg.Source = this.server.CurrentFrame;
                    this.server.Respond = "";
                }
            }
        }
    }
}
