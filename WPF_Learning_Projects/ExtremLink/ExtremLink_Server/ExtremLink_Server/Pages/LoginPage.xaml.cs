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

namespace ExtremLink_Server.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>

    public partial class LoginPage : UserControl
    {
        private ContentControl contentMain;

        public ContentControl ContentMain
        {
            get { return contentMain; }
            set { this.contentMain = value; }
        }
        public event EventHandler<LoginEventArgs> LoginAttempted;
        public event EventHandler ForgotPasswordRequested;

        public LoginPage(ContentControl contentMain)
        {
            InitializeComponent();
            this.ContentMain = contentMain;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Raise the LoginAttempted event
            LoginAttempted?.Invoke(this, new LoginEventArgs(username, password));
        }

        private void ForgotPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Raise the ForgotPasswordRequested event
            ForgotPasswordRequested?.Invoke(this, EventArgs.Empty);
        }
    }

    public class LoginEventArgs : EventArgs
    {
        public string Username { get; }
        public string Password { get; }

        public LoginEventArgs(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}

