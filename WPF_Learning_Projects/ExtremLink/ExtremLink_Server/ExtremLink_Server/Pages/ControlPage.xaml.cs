using ExtremLink_Server.Classes;
using System;
using System.CodeDom;
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
        private CustomMouse customMouse = CustomMouse.CustomMouseInstance;

        public ControlPage(ContentControl contentMain, Server server)
        {
            this.contentMain = contentMain;
            this.server = server;
            this.isReceivingFrames = false;
            InitializeComponent();
            this.clientIpTextBlock.Text = $"Client's IP: {this.server.ClientIpAddress}";
        }

        // Frames functions:
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
            lock (this.server.fileLock)
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string tempFramePath = System.IO.Path.Combine(baseDirectory, "tempFrame.png");
                return this.server.GetImageOfPNGFile(tempFramePath);
            }
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

        
        // Mouse functions:
        private void FrameImg_MouseMove(object sender, MouseEventArgs e)
        {
            // Input: nothing
            // Output: The function updates the mouse's coordinates sends them the client as a query.
            if (this.isReceivingFrames)
            {
                this.customMouse.ChangePosition(e.GetPosition(frameImg));
                this.customMouse.SendMouseCommands(this.server, MouseCommands.Move);
            }
        }
        private void FrameImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Input: nothing
            // Output: The function sends the client as a query about pressing on one of the mouse buttons(left ot right).
            if (this.isReceivingFrames)
            {
                this.customMouse.ChangePosition(e.GetPosition(frameImg));
                if (e.ChangedButton == MouseButton.Left)
                {
                    this.customMouse.SendMouseCommands(this.server, MouseCommands.LeftPress);
                }
                if (e.ChangedButton == MouseButton.Right)
                {
                    this.customMouse.SendMouseCommands(this.server, MouseCommands.RightPress);
                }
            }
        }

        // Keyboard functions:
        private void 

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            isReceivingFrames = false;
        }
    }
}
