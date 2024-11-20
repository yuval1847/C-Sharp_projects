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
        }

        private void StartSharingScreen()
        {
            while (!this.isStreaming)
            {
                if (this.client.ServerRespond == "StartSendFrames")
                {
                    this.isStreaming = true;
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
                    Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error capturing screen: {ex.Message}");
                    this.isStreaming = false;
                }
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
                    var screenBrush = new VisualBrush
                    {
                        Visual = Application.Current.MainWindow,
                        Stretch = Stretch.None,
                        AlignmentX = AlignmentX.Left,
                        AlignmentY = AlignmentY.Top
                    };

                    drawingContext.DrawRectangle(screenBrush, null, new Rect(0, 0, screenWidth, screenHeight));
                }

                result = new RenderTargetBitmap(screenWidth, screenHeight, 96, 96, PixelFormats.Pbgra32);
                result.Render(drawingVisual);
                result.Freeze(); // Make it thread-safe
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => MessageBox.Show($"Screen capture error: {ex.Message}"));
            }

            return result;
        }
    }
}
