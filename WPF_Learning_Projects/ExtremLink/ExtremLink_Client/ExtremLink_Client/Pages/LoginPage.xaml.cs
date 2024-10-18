using ExtremLink_Client.Classes;
using System;
using System.Collections.Generic;
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

namespace ExtremLink_Client.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        private ContentControl contentMain;
        private Client client;

        public ContentControl ContentMain
        {
            get { return contentMain; }
            set { this.contentMain = value; }
        }


        public LoginPage(ContentControl contentMain, Client client)
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
            this.client.SendMessage(this.client.TCPSocket, "!", $"username={usernameCustomTextBox.customTB.Text},password={passwordCustomTextBox.customTB.Text}");

        }
    }
}
