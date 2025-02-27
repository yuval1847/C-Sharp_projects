﻿using ExtremLink_Server.Classes;
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
        private CustomKeyboard customKeyboard = CustomKeyboard.CustomKeyboardInstance;
        private Session currentSession;

        // A thread which operate the recording
        private Thread recordingThread;

        // A bool parameter which state the status of the recording(if the program is in recording mode or not).
        private bool isRecording;


        // Constractor:
        public ControlPage(ContentControl contentMain, Server server)
        {
            this.contentMain = contentMain;
            this.server = server;
            this.isReceivingFrames = false;
            this.isRecording = false;
            this.recordingThread = new Thread(this.RecordSession);
            InitializeComponent();
            this.clientIpTextBlock.Text = $"Client's IP: {this.server.ClientIpAddress}";
        }


        // Frames buttons functions:
        private void UpdateBasicFramesButtonStatus(bool startBtnState, bool stopBtnState, bool pauseBtnState, bool recordBtnState)
        {
            // Input: 3 bool values which represent the status of each button.
            // Output: The function enable and disable each button according to the given values.
            startBtn.IsEnabled = startBtnState;
            stopBtn.IsEnabled = stopBtnState;
            pauseBtn.IsEnabled = pauseBtnState;
            recordBtn.IsEnabled = recordBtnState;
        }
        
        // Clicking the buttons functions:
        private void StartStreamBtnClick(object sender, RoutedEventArgs e)
        {
            // Input: Nothing.
            // Output: The function sends the client the command to start the sharescreen.
            this.isReceivingFrames = true;
            this.UpdateBasicFramesButtonStatus(false, true, true, true);
            this.server.SendMessage(this.server.ClientTcpSocket, "&", "StartSendFrames");
            UpdateFrame();
            // MessageBox.Show("The start send frame command was sent!");
        }
        private void StopStreamBtnClick(object sender, RoutedEventArgs e)
        {
            // Input: Nothing.
            // Output: The function sends the client the command to stop the sharescreen.
            this.isReceivingFrames = false;
            this.UpdateBasicFramesButtonStatus(true, false, false, false);
            this.StopRecordSession();
            this.server.SendMessage(this.server.ClientTcpSocket, "&", "StopSendFrames");
        }
        private void PauseStreamBtnClick(object sender, RoutedEventArgs e)
        {
            // Input: Nothing.
            // Output: The function sends the client the command to pause the sharescreen.
            this.isReceivingFrames = false;
            this.UpdateBasicFramesButtonStatus(true, true, false, false);
            this.StopRecordSession();
            this.server.SendMessage(this.server.ClientTcpSocket, "&", "PauseSendFrames");
        }
        private void RecordBtnCustomClick(object sender, RoutedEventArgs e)
        {
            // Input: Nothing.
            // Output: The function starts the recording of the sharing screen.
            this.isRecording = true;
            this.UpdateBasicFramesButtonStatus(false, true, true, false);
            this.StartRecordSession();
        }

        // Frames functions:
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
        private void FrameImg_KeyboardKeyDown(object sender, KeyEventArgs e)
        {
            // Input: Nothing.
            // Output: The function getting keyboard key and sending it to the client
            Key pressedKey = e.Key;
            this.customKeyboard.SendKeyboardCommands(this.server, pressedKey);
        }


        // Recording functions:
        // Note: You should kill the recording thread when the session stopped or paused
        private void RecordSession()
        {
            // Input: The function gets nothing.
            // Output: The function records the current controlling session.
            this.currentSession = new Session(DateTime.Now, (int)frameImg.Width, (int)frameImg.Height, 24);
            this.currentSession.StartRecording();
            BitmapImage currentFrame = LoadFrameFromFile();
            this.currentSession.AddFrame(currentFrame);
        }
        private void StartRecordSession()
        {
            // Input: Nothing.
            // Output: The function starts the recordingThread and start the recording of the current session.
            if (!this.recordingThread.IsAlive)
            {
                this.recordingThread.Start();
            }
        }
        private void StopRecordSession()
        {
            // Input: Nothing.
            // Output: The function kills the recordingThread, stop the recording of the current session
            // and upload the session to the database.
            if (this.recordingThread.IsAlive)
            {
                this.recordingThread.Abort();
            }
            this.currentSession.UploadSessionToDatabase();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            isReceivingFrames = false;
        }
    }
}
