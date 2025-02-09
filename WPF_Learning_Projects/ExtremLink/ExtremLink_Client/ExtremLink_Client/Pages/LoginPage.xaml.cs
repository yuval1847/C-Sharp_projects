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
using ExtremLink_Client.Classes;


namespace ExtremLink_Client.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        private ContentControl contentMain;
        private Client client;
        private User currentUser = User.UserInstance;

        public ContentControl ContentMain
        {
            get { return this.contentMain; }
            set { this.contentMain = value; }
        }


        public LoginPage(ContentControl contentMain, Classes.Client client)
        {
            this.ContentMain = contentMain;
            this.client = client;
            InitializeComponent();
        }
        
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // The function called by clicking on the login button
            // The fucntion gets nothing.
            // The function send a message to the server via the client in order to login 
            // to the database.
            Thread clientMessagesHandlingThread = new Thread(this.client.Start);
            clientMessagesHandlingThread.Start();
            this.client.SendMessage(this.client.TCPSocket, "!", $"login,username={usernameCustomTextBox.customTB.Text},password={passwordCustomTextBox.customTB.Text}");
            // Waiting for reciving the server respond.
            Thread.Sleep(750);
            if (this.client.ServerRespond == "Exist")
            {
                this.currentUser.UserName = usernameCustomTextBox.customTB.Text;
                this.wrongLoginTextBlock.Visibility = Visibility.Visible;
                this.wrongLoginTextBlock.Text = "successfully connected!";
                this.wrongLoginTextBlock.Foreground = Brushes.Green;
                this.contentMain.Content = new SharingScreenPage(this.contentMain, this.client);
            }
            else if (this.client.ServerRespond == "NotExist")
            {
                this.wrongLoginTextBlock.Visibility = Visibility.Visible;
            }
            // clientMessagesHandlingThread.Abort();
        }

        private void CreateNewUser_Click(object sender, RoutedEventArgs e)
        {
            this.contentMain.Content = new SignUpPage(this.contentMain, this.client);
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
