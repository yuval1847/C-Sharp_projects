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
using ExtremLink_Client.Classes;

namespace ExtremLink_Client.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {

        // Attirbutes:
        private ContentControl contentMain;


        private void UpdateHeader()
        {
            // Input: Nothing.
            // Output: The function updates the page's header with the username.
           headerTextBlock.Text = headerTextBlock.Text + User.UserInstance.UserName;
        }

        public HomePage(ContentControl contentMain)
        {
            this.contentMain = contentMain;
            InitializeComponent();
            this.UpdateHeader();
        }

        private void StartNowBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (User.UserInstance.TypeOfClient)
            {
                case TypeOfClient.Attacker:
                    this.contentMain.Content = new ControlPage(this.contentMain);
                    break;
                case TypeOfClient.Victim:
                    this.contentMain.Content = new SharingScreenPage(this.contentMain);
                    break;
            }
        }
        
        private void Instructions_Click(object sender, RoutedEventArgs e)
        {
            this.contentMain.Content = new InstructionsPage(this.contentMain);
        }

        private void Records_Click(object sender, RoutedEventArgs e)
        {
            this.contentMain.Content = new SessionsRecordsPage(this.contentMain);
        }
    }
}
