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
using System.Runtime.InteropServices;
using System.ComponentModel;

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
        private const string defaultPngFileOfFrame = "tempFrame.png";

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
                        string compressedFrame = this.client.ConvertRenderTargetBitmapToString(screen, 80, defaultPngFileOfFrame);
                        this.client.SendMessage(this.client.UDPSocket, "&", $"frame_received:{compressedFrame}");
                    }
                    // Around 1 FPS
                    Thread.Sleep(1000);
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

        // Screenshot:

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        private const uint SRCCOPY = 0x00CC0020;
        private System.Windows.Controls.Image img;
        public RenderTargetBitmap CaptureScreen()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(() => CaptureScreen());
            }

            IntPtr desktopDC = GetDC(IntPtr.Zero);
            if (desktopDC == IntPtr.Zero)
                throw new InvalidOperationException("Failed to get DC");

            try
            {
                int screenWidth = (int)System.Windows.SystemParameters.VirtualScreenWidth;
                int screenHeight = (int)System.Windows.SystemParameters.VirtualScreenHeight;
                int screenLeft = (int)System.Windows.SystemParameters.VirtualScreenLeft;
                int screenTop = (int)System.Windows.SystemParameters.VirtualScreenTop;

                IntPtr compatibleDC = CreateCompatibleDC(desktopDC);
                if (compatibleDC == IntPtr.Zero)
                    throw new InvalidOperationException("Failed to create compatible DC");

                IntPtr hBitmap = CreateCompatibleBitmap(desktopDC, screenWidth, screenHeight);
                if (hBitmap == IntPtr.Zero)
                {
                    DeleteObject(compatibleDC);
                    throw new InvalidOperationException("Failed to create compatible bitmap");
                }

                IntPtr oldBitmap = SelectObject(compatibleDC, hBitmap);

                BitBlt(compatibleDC, 0, 0, screenWidth, screenHeight, desktopDC, screenLeft, screenTop, SRCCOPY);

                // Convert to WPF bitmap
                BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                // Cleanup
                SelectObject(compatibleDC, oldBitmap);
                DeleteObject(hBitmap);
                DeleteObject(compatibleDC);

                // Create final RenderTargetBitmap
                RenderTargetBitmap renderTarget = new RenderTargetBitmap(screenWidth, screenHeight, 96, 96, PixelFormats.Pbgra32);
                var drawingVisual = new DrawingVisual();
                using (var drawingContext = drawingVisual.RenderOpen())
                {
                    drawingContext.DrawImage(bitmapSource, new System.Windows.Rect(0, 0, screenWidth, screenHeight));
                }

                renderTarget.Render(drawingVisual);
                renderTarget.Freeze();
                return renderTarget;
            }
            finally
            {
                ReleaseDC(IntPtr.Zero, desktopDC);
            }
        }
    }
}