using ExtremLink_Client.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;


namespace ExtremLink_Client.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        private ContentControl contentMain;
        public ContentControl ContentMain
        {
            get { return this.contentMain; }
            set { this.contentMain = value; }
        }
        
        public LoginPage(ContentControl contentMain)
        {
            this.ContentMain = contentMain;
            InitializeComponent();
        }
        
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // The function called by clicking on the login button
            // The fucntion gets nothing.
            // The function send a login request message to the server via the client in order to login as a user
            string loginRequest = $"login,username={usernameCustomTextBox.customTB.Text},password={passwordCustomTextBox.customTB.Text}";
            switch (User.UserInstance.TypeOfClient)
            {
                case TypeOfClient.Attacker:
                    Attacker.AttackerInstance.SendTCPMessageToClient("!", loginRequest);
                    break;
                case TypeOfClient.Victim:
                    Victim.VictimInstance.SendTCPMessageToClient("!", loginRequest);
                    break;
            }
            Thread.Sleep(750);
            if (Attacker.AttackerInstance.ServerRespond == "Exist")
            {
                User.UserInstance.UserName = usernameCustomTextBox.customTB.Text;
                this.contentMain.Content = new HomePage(this.contentMain);
            }
            else
            {
                this.wrongLoginTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void CreateNewUser_Click(object sender, RoutedEventArgs e)
        {
            this.contentMain.Content = new SignUpPage(this.contentMain);
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
