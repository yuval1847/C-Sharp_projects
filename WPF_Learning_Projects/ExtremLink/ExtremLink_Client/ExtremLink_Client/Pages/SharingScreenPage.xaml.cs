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
        private Thread serverFramesResponds;
        private Thread sharingScreenThread;
        private Thread localSharingScreenThread;
        private Thread mouseControllingThead;
        private Thread keyboardControllingThead;
        private bool isStreaming;
        private const string defaultPngFileOfFrame = "tempFrame.png";
        private CustomMouse customMouse = CustomMouse.CustomMouseInstance;
        private CustomKeyboard customKeyboard = CustomKeyboard.CustomKeyboardInstance;

        public ContentControl ContentMain
        {
            get { return this.contentMain; }
            set { this.contentMain = value; }
        }

        public SharingScreenPage(ContentControl contentMain, Client client)
        {
            this.contentMain = contentMain;
            this.client = client;
            this.isStreaming = false;

            this.serverFramesResponds = new Thread(this.HandleServerRespond);
            this.sharingScreenThread = new Thread(this.StartSharingScreen);
            this.localSharingScreenThread = new Thread(this.LocalSharingScreen);
            this.mouseControllingThead = new Thread(this.StartMouseControl);
            this.keyboardControllingThead = new Thread(this.StartKeyboardControl);

            this.serverFramesResponds.Start();
            InitializeComponent();
        }

        // Frames handling:
        // Controlling control threads.
        private void StartGettingControlled()
        {
            // Input: Nothing.
            // Output: The function starts all the threads of the controlling.
            this.sharingScreenThread.Start();
            this.localSharingScreenThread.Start();
            this.mouseControllingThead.Start();
            this.keyboardControllingThead.Start();
        }
        private void StopGettingControlled()
        {
            // Input: Nothing.
            // Output: The function kill all the threads of the controlling.
            this.sharingScreenThread.Join();
            this.localSharingScreenThread.Join();
            this.mouseControllingThead.Join();
            this.keyboardControllingThead.Join();
        }
        
        // Changing sharescreen TextBlock text.
        private void ChangeSharingScreenTitle(string newText) {
            // Input: a string which contains a text.
            // Output: The function changes the sharing screen title according to the given string using the UI thread.
            Dispatcher.Invoke(() => sharingScreenTitle.Text = newText);
        }

        // Handle server responds.
        private void HandleServerRespond()
        {
            // Input: Nothing.
            // Output: The function execute the frame functionality according to the client's server respond.
            switch (this.client.ServerRespond)
            {
                case "StartSendFrames":
                    this.isStreaming = true;
                    this.ChangeSharingScreenTitle("Sharing Screen Now");
                    this.StartGettingControlled();
                    break;

                case "StopSendFrames":
                    this.isStreaming = false;
                    this.StopGettingControlled();
                    this.ChangeSharingScreenTitle("Sharing Screen Stoped");
                    // Note: Here in this case change the page to the home page.
                    break;

                case "PauseSendFrames":
                    this.isStreaming = false;
                    this.ChangeSharingScreenTitle("Sharing Screen Puased");
                    this.StopGettingControlled();
                    break;
            }
        }


        private void StartSharingScreen()
        {
            // Input: Nothing.
            // Output: The function sending the frames of the shared screen to the server.
            while (true)
            {
                try
                {
                    var screen = CaptureScreen();
                    if (screen != null)
                    {
                        this.client.SendFrame(screen);
                    }
                    // MoveMouseToCoordinates(this.client.MouseX, this.client.MouseY);
                    // Around 1 FPS
                    // Note: try to use Task.delay intead.
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error capturing screen: {ex.Message}");
                }
            }
        }
        private void LocalSharingScreen()
        {
            // Input: Nothing.
            // Output: The function show
            while (true)
            {
                try
                {
                    var screen = CaptureScreen();
                    Dispatcher.Invoke(() => frameImg.Source = screen);
                    // Around 60 FPS
                    Thread.Sleep(16);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error local sharing screen: {ex.Message}");
                }
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

        // Import GetSystemMetrics from user32.dll
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        // Constants for virtual screen metrics
        private const int SM_CXVIRTUALSCREEN = 78;  // Virtual screen width
        private const int SM_CYVIRTUALSCREEN = 79;  // Virtual screen height
        private const int SM_XVIRTUALSCREEN = 76;   // Virtual screen left
        private const int SM_YVIRTUALSCREEN = 77;   // Virtual screen top

        // Taking screenshots:
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
                int screenWidth = GetSystemMetrics(SM_CXVIRTUALSCREEN);
                int screenHeight = GetSystemMetrics(SM_CYVIRTUALSCREEN);
                int screenLeft = GetSystemMetrics(SM_XVIRTUALSCREEN);
                int screenTop = GetSystemMetrics(SM_YVIRTUALSCREEN);
                /*
                int screenWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth * 2;
                int screenHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight * 2;
                int screenLeft = (int)System.Windows.SystemParameters.VirtualScreenLeft;
                int screenTop = (int)System.Windows.SystemParameters.VirtualScreenTop;
                */


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


        // Mouse handling:
        private void StartMouseControl()
        {
            // Input: Nothing
            // Ouput: The function starting the mouse control over the host itself.
            while (true)
            {
                this.customMouse.ExecuteCurrentMouseCommand();
                Thread.Sleep(50);
            }
        }

        // Keyboard handling:
        private void StartKeyboardControl()
        {
            // Input: Nothing.
            // Output: The function starting the keyboard control over the host itself.
            while (true)
            {
                this.customKeyboard.ExecuteCurrentKeyboardCommand();
                Thread.Sleep(20);
            }
        }
    }
}