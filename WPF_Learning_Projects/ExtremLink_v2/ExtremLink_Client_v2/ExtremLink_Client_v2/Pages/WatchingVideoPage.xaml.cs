using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using ExtremLink_Client_v2.Classes;

namespace ExtremLink_Client_v2.Pages
{
    /// <summary>
    /// Interaction logic for WatchingVideoPage.xaml
    /// </summary>
    public partial class WatchingVideoPage : UserControl
    {

        // Attributes:
        private ContentControl contentMain;

        public WatchingVideoPage(ContentControl contentMain)
        {
            this.contentMain = contentMain;
            InitializeComponent();
            this.CreateTempSessionVideoFile();
            this.LoadVideo();
        }

        // Functions of the session:
        private void CreateTempSessionVideoFile()
        {
            // Input: Nothing.
            // Output: The function creates a temp file which store the session's content.
            byte[] sessionContent = null;
            switch (User.UserInstance.TypeOfClient)
            {
                case TypeOfClient.Attacker:
                    sessionContent = Attacker.AttackerInstance.CurrentSessionBytes;
                    break;
                case TypeOfClient.Victim:
                    sessionContent = Victim.VictimInstance.CurrentSessionBytes;
                    break;
            }

            try
            {
                Session.CreateTempMP4File(sessionContent);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadVideo()
        {
            // Input: Nothing.
            // Output: The function play the upload the video to the VideoPlayer widget.
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string videoPath = System.IO.Path.Combine(projectDirectory, "tempRecvSession.mp4");

            // Ensure the file exists before trying to play
            if (System.IO.File.Exists(videoPath))
            {
                VideoPlayer.Source = new Uri(videoPath);
                VideoPlayer.Play();
            }
            else
            {
                MessageBox.Show("Video file not found!");
            }
        }
    }
}
