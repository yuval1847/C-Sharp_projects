using ExtremLink_Client.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Interop;

namespace ExtremLink_Client.Pages
{
    /// <summary>
    /// Interaction logic for SharingScreenPage.xaml
    /// </summary>
    public partial class SharingScreenPage : UserControl
    {
        private ContentControl contentMain;
        private Client client;
        private Thread clientMessagesHandlingThread;
        private Thread sharingScreenThread;
        private Thread localSharingScreenThread;
        private bool isStreaming;

        public ContentControl ContentMain
        {
            get { return this.contentMain; }
            set { this.contentMain = value; }
        }

        public SharingScreenPage(ContentControl contentMain, Client client, Thread clientMessagesHandlingThread)
        {
            this.contentMain = contentMain;
            this.client = client;
            this.clientMessagesHandlingThread = clientMessagesHandlingThread;
            this.isStreaming = false;
            InitializeComponent();

            this.sharingScreenThread = new Thread(this.StartSharingScreen);
            this.sharingScreenThread.Start();

            // It will be started when the server start ask for sharing screen
            this.localSharingScreenThread = new Thread(this.LocalSharingScreen);
            
        }

        private void StartSharingScreen()
        {
            while (!this.isStreaming)
            {
                if (this.client.ServerRespond == "StartSendFrames")
                {
                    this.isStreaming = true;
                    Dispatcher.Invoke(() => sharingScreenTitle.Text = "Sharing Screen Now");
                    this.localSharingScreenThread.Start();
                }
                Thread.Sleep(100);
            }

            while (this.isStreaming)
            {
                try
                {
                    var screen = CaptureScreen();
                    if (screen != null)
                    {
                        string compressedFrame = this.client.CompressRenderTargetBitmap(screen);
                        this.client.SendMessage(this.client.UDPSocket, "&", $"frame_received:{compressedFrame}");
                    }
                    // Around 60 FPS
                    Thread.Sleep(16);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error capturing screen: {ex.Message}");
                    this.isStreaming = false;
                }
            }
        }
        private void LocalSharingScreen()
        {
            while (this.isStreaming)
            {
                var screen = CaptureScreen();
                Dispatcher.Invoke(() => frameImg.Source = screen);
                // Around 60 FPS
                Thread.Sleep(16);
            }
        }

        private RenderTargetBitmap CaptureScreen()
        {
            RenderTargetBitmap result = null;
            var screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            var screenHeight = (int)SystemParameters.PrimaryScreenHeight;

            try
            {
                if (!Dispatcher.CheckAccess())
                {
                    return Dispatcher.Invoke(() => CaptureScreen());
                }

                var drawingVisual = new DrawingVisual();
                using (var drawingContext = drawingVisual.RenderOpen())
                {
                    // Create a screen brush that captures the entire desktop
                    var screenBrush = new VisualBrush
                    {
                        Visual = GetDesktopWindow(),
                        Stretch = Stretch.None,
                        AlignmentX = AlignmentX.Left,
                        AlignmentY = AlignmentY.Top
                    };

                    // Draw the entire screen
                    drawingContext.DrawRectangle(screenBrush, null, new Rect(0, 0, screenWidth, screenHeight));
                }

                result = new RenderTargetBitmap(screenWidth, screenHeight, 96, 96, PixelFormats.Pbgra32);
                result.Render(drawingVisual);
                result.Freeze(); // Make it thread-safe
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Screen capture error: {ex.Message}");
            }

            return result;
        }

        // Add this helper method to get the desktop window
        private Visual GetDesktopWindow()
        {
            try
            {
                // Get the desktop window handle
                var desktopHandle = GetDesktopWindow();

                // Create HwndSource for the desktop window
                var parameters = new HwndSourceParameters("DesktopCapture");
                parameters.ParentWindow = desktopHandle;
                parameters.WindowStyle = 0x40000000; // WS_CHILD

                var source = new HwndSource(parameters);
                return source.RootVisual;
            }
            catch
            {
                return null;
            }
        }
    }
}
