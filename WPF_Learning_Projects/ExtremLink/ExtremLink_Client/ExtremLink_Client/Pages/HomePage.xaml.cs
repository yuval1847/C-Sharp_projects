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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtremLink_Client.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        private ContentControl contentMain;
        private Client client;
        private User currentUser = User.UserInstance;


        public HomePage(ContentControl contentMain, Client client)
        {
            this.contentMain = contentMain;
            this.client = client;
            this.ChangeMainTitleTextBlockText($"Welcome back, {this.currentUser.UserName}");
            InitializeComponent();
        }

        private void ChangeMainTitleTextBlockText(string newText)
        {
            // Input: A string value which represent the new text of the main title.
            // Output: The function changes the text of the main title TextBlck to the given string.
            Dispatcher.Invoke(() => MainTitleTextBlock.Text = newText);
        }

        private void StartNowBtn_Click(object sender, RoutedEventArgs e)
        {
            // Input: Nothing.
            // Ouput: The function changes the current page from the home page to the sharing screen page.
            this.contentMain.Content = new SharingScreenPage(this.contentMain, this.client);
        }

        private void InstructionsBtn_Click(object sender, RoutedEventArgs e)
        {
            // Input: Nothing.
            // Output: The function changes the current page from the home page to the instructions page.
            this.contentMain.Content = new InstructionsPage(this.contentMain, this.client);
        }
    }
}
