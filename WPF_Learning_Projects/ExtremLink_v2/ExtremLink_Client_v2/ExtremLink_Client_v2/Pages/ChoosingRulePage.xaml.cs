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
    /// Interaction logic for ChoosingRulePage.xaml
    /// </summary>
    public partial class ChoosingRulePage : UserControl
    {
        // Attirbutes:
        private ContentControl contentMain;
        public ChoosingRulePage(ContentControl contentMain)
        {
            SoundManager.SoundManagerInstance.PlaySound(EPlaylist.Background);
            this.contentMain = contentMain;
            InitializeComponent();
        }

        private void AttackerBtn_Click(object sender, RoutedEventArgs e)
        {
            User.UserInstance.TypeOfClient = TypeOfClient.Attacker;
            this.contentMain.Content = new ConnectingPage(this.contentMain);
        }

        private void VictimBtn_Click(object sender, RoutedEventArgs e)
        {
            User.UserInstance.TypeOfClient = TypeOfClient.Victim;
            this.contentMain.Content = new ConnectingPage(this.contentMain);
        }
    }
}
