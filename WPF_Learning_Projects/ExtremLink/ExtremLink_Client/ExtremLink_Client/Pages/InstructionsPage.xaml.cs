using ExtremLink_Client.Classes;
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

namespace ExtremLink_Client.Pages
{
    /// <summary>
    /// Interaction logic for InstructionsPage.xaml
    /// </summary>
    public partial class InstructionsPage : UserControl
    {
        // Attirbutes:
        private ContentControl contentMain;
        private Client client;


        // Constractor:
        public InstructionsPage(ContentControl contentMain, Client client)
        {
            this.contentMain = contentMain;
            this.client = client;
            InitializeComponent();
        }


        // The back home button click function:
        private void BackHomeBtn_Click(object sender, RoutedEventArgs e)
        {
            // Input: Nothing.
            // Output: The function changes the current page to the home page.
            this.contentMain.Content = new HomePage(this.contentMain, this.client);
        }
    }
}
