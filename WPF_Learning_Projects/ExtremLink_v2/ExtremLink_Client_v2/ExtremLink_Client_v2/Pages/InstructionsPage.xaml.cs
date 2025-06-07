using ExtremLink_Client_v2.Classes;
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

namespace ExtremLink_Client_v2.Pages
{
    /// <summary>
    /// Interaction logic for InstructionsPage.xaml
    /// </summary>
    public partial class InstructionsPage : UserControl
    {
        // Attirbutes:
        private ContentControl contentMain;


        // Constractor:
        public InstructionsPage(ContentControl contentMain)
        {
            this.contentMain = contentMain;
            InitializeComponent();
        }


        // The back home button click function:
        private void BackHomeBtn_Click(object sender, RoutedEventArgs e)
        {
            // Input: Nothing.
            // Output: The function changes the current page to the home page.
            this.contentMain.Content = new HomePage(this.contentMain);
        }

        private void AutoReaderBtn_Click(object sender, RoutedEventArgs e)
        {
            // This function starts the auto read of the instruction.
            BackHomeBtn.IsEnabled = false;
            SoundManager.SoundManagerInstance.PlaySound(EPlaylist.InsturctionsReaderAI);
            Thread.Sleep(33000);
            BackHomeBtn.IsEnabled = true;
            SoundManager.SoundManagerInstance.PlaySound(EPlaylist.Background);
        }
    }
}
