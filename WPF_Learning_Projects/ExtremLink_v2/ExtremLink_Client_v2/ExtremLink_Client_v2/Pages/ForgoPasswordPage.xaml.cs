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
using MailKit.Net.Smtp;
using MimeKit;

namespace ExtremLink_Client_v2.Pages
{
    /// <summary>
    /// Interaction logic for ForgoPasswordPage.xaml
    /// </summary>
    public partial class ForgoPasswordPage : UserControl
    {

        private ContentControl mainContent;

        public ForgoPasswordPage(ContentControl mainContent)
        {
            InitializeComponent();
            this.mainContent = mainContent;
        }

        private void BackLoginPageBtn_Click(object sender, RoutedEventArgs e)
        {
            // This function returns to the login page.
            this.mainContent.Content = new LoginPage(this.mainContent);
        }

        private void ShowPasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            // A function which show's the password of a specific user according to it's given credentials.
            string forgotPasswordRequest = $"forgotPassword,username={usernameCustomTextBox.Text},city={cityCustomTextBox.Text},phone={phoneCustomTextBox.Text}";
            switch (User.UserInstance.TypeOfClient)
            {
                case TypeOfClient.Attacker:
                    Attacker.AttackerInstance.SendTCPMessageToClient("!", forgotPasswordRequest);
                    break;
                case TypeOfClient.Victim:
                    Victim.VictimInstance.SendTCPMessageToClient("!", forgotPasswordRequest);
                    break;
            }

            Thread.Sleep(2000);
            string password = "";
            switch (User.UserInstance.TypeOfClient)
            {
                case TypeOfClient.Attacker:
                    if (Attacker.AttackerInstance.ServerRespond != null && Attacker.AttackerInstance.ServerRespond.Contains("password"))
                    {
                        wrongPropertiesTextBlock.Visibility = Visibility.Hidden;
                        password = Attacker.AttackerInstance.ServerRespond.Split('=')[1];
                        MessageBox.Show($"The user's password is: {password}");
                    }
                    else
                    {
                        wrongPropertiesTextBlock.Visibility = Visibility.Visible;
                    }
                    break;

                case TypeOfClient.Victim:
                    if (Victim.VictimInstance.ServerRespond != null && Victim.VictimInstance.ServerRespond.Contains("password"))
                    {
                        wrongPropertiesTextBlock.Visibility = Visibility.Hidden;
                        password = Victim.VictimInstance.ServerRespond.Split('=')[1];
                        MessageBox.Show($"The user's password is: {password}");
                    }
                    else
                    {
                        wrongPropertiesTextBlock.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }
    }
}
