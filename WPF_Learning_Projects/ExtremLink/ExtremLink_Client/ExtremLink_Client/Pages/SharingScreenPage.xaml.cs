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
        bool isStreaming;

        public ContentControl ContentMain
        {
            get { return this.contentMain; }
            set { this.contentMain = value; }
        }

        public SharingScreenPage(ContentControl contentMain, Client client, Thread clientMessagesHandlingThread)
        {
            this.ContentMain = contentMain;
            this.client = client;
            this.clientMessagesHandlingThread = clientMessagesHandlingThread;
            this.isStreaming = false;
            this.sharingScreenThread = new Thread(this.StartSharingScreen);
            this.sharingScreenThread.Start();
            InitializeComponent();
        }
        
        private void StartSharingScreen()
        {
            // The function gets nothing.
            // The function sends the client's screenshots to the server.
            while (this.client.ServerRespond != "StartSendFrames")
            {
                if (this.client.ServerRespond == "StartSendFrames")
                {
                    this.isStreaming = true;
                }
                Thread.Sleep(100);
            }
            while (this.isStreaming)
            {
                this.client.SendMessage(this.client.UDPSocket, "&", $"frame_received:{this.client.CompressRenderTargetBitmap(this.CaptureScreen())}");
                Thread.Sleep(50);
            }
        }

        public RenderTargetBitmap CaptureScreen()
        {
            // Get the dimensions of the primary screen
            int screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            int screenHeight = (int)SystemParameters.PrimaryScreenHeight;

            // Create a VisualBrush of the screen
            Dispatcher.Invoke(() =>
            {
                VisualBrush visualBrush = new VisualBrush
                {
                    Visual = new System.Windows.Interop.HwndSource(
                    new System.Windows.Interop.HwndSourceParameters
                    {
                        Width = screenWidth,
                        Height = screenHeight,
                    }).RootVisual
                };
            

            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext context = drawingVisual.RenderOpen())
            {
                context.DrawRectangle(visualBrush, null, new Rect(0, 0, screenWidth, screenHeight));
            }

            // Render the screen into a RenderTargetBitmap
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                screenWidth, screenHeight, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(drawingVisual);
            return renderTargetBitmap;
            });
            return null;
        }
    }
}
