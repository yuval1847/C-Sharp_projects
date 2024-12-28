using ExtremLink_Server.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace ExtremLink_Server.Pages
{
    /// <summary>
    /// Interaction logic for ControlPage.xaml
    /// </summary>
    public partial class ControlPage : UserControl
    {
        private ContentControl contentMain;
        private Server server;
        private bool isReceivingFrames;
        private Thread frameUpdateThread;
        


        public ControlPage(ContentControl contentMain, Server server)
        {
            this.contentMain = contentMain;
            this.server = server;
            this.isReceivingFrames = false;
            InitializeComponent();
            this.clientIpTextBlock.Text = $"Client's IP: {this.server.ClientIpAddress}";
        }

        private void StartStreamBtnClick(object sender, RoutedEventArgs e)
        {
            if (!isReceivingFrames)
            {
                isReceivingFrames = true;
                this.server.SendMessage(this.server.ClientTcpSocket, "&", "StartSendFrames");

                // frameUpdateThread = new Thread(UpdateFrame);
                // frameUpdateThread.Start();
                UpdateFrame();
                playAndPauseBtn.Content = "Stop";
            }
            else
            {
                isReceivingFrames = false;
                this.server.SendMessage(this.server.ClientTcpSocket, "&", "StopSendFrames");
                playAndPauseBtn.Content = "Start";
            }
        }
        
        private BitmapImage LoadFrameFromFile()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string tempFramePath = System.IO.Path.Combine(baseDirectory, "tempFrame.png");
            return this.server.GetImageOfPNGFile(tempFramePath);
        }
        
        private async void UpdateFrame()
        {
            while (isReceivingFrames)
            {
                try
                {
                    if (this.server.CurrentFrame != null)
                    {
                        await frameImg.Dispatcher.InvokeAsync(() => frameImg.Source = LoadFrameFromFile());
                    }
                    // Set the sleep function so the frame rate will be around ~60 FPS
                    await Task.Delay(16);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating frame: {ex.Message}");
                    isReceivingFrames = false;
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            isReceivingFrames = false;
        }
    }
}
