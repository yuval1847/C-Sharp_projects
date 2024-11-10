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
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : UserControl
    {
        private ContentControl contentMain;
        private Classes.Client client;

        public ContentControl ContentMain
        {
            get { return contentMain; }
            set { this.contentMain = value; }
        }


        public SignUpPage(ContentControl contentMain, Classes.Client client)
        {
            this.ContentMain = contentMain;
            this.client = client;
            InitializeComponent();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsAllParametersRight())
            {
                Thread clientMessagesHandlingThread = new Thread(this.client.Start);
                clientMessagesHandlingThread.Start();
                this.client.SendMessage(this.client.TCPSocket, "!", $"signup,firstname={fnCustomTextBox.customTB.Text},lastname={lnCustomTextBox.customTB.Text},city={cityCustomTextBox.customTB.Text},phone={phoneCustomTextBox.customTB.Text},username={usernameCustomTextBox.customTB.Text},password={passwordCustomTextBox.customTB.Text},email={emailCustomTextBox.customTB.Text}");
                Thread.Sleep(500);
                MessageBox.Show(this.client.ServerRespond);
                if (this.client.ServerRespond == "SuccessfullyAdded")
                {
                    this.ContentMain.Content = new LoginPage(this.ContentMain, this.client);
                }
                //else if (this.client.ServerRespond == "NotAdded"){}
            }
        }
        private bool IsAllParametersRight()
        {
            // The function gets nothing.
            // The function returns true if all the sign-up parameter are fit the 
            // requirments, otherwise, it returns false and show textboxes of the problems.
            bool allValid = true;

            // Checking the fields:
            // First Name
            if (fnCustomTextBox.customTB.Text.Length == 0 || fnCustomTextBox.customTB.Text.Length > 16)
            {
                wrongFirstNameTextBlock.Visibility = Visibility.Visible;
                allValid = false;
            }
            else
            {
                wrongFirstNameTextBlock.Visibility = Visibility.Hidden;
            }

            // Last Name
            if (lnCustomTextBox.customTB.Text.Length == 0 || lnCustomTextBox.customTB.Text.Length > 16)
            {
                wrongLastNameTextBlock.Visibility = Visibility.Visible;
                allValid = false;
            }
            else
            {
                wrongLastNameTextBlock.Visibility = Visibility.Hidden;
            }

            // City
            if (cityCustomTextBox.customTB.Text.Length < 5 || cityCustomTextBox.customTB.Text.Length > 16 || cityCustomTextBox.customTB.Text.Any(char.IsDigit))
            {
                wrongCityTextBlock.Visibility = Visibility.Visible;
                allValid = false;
            }
            else
            {
                wrongCityTextBlock.Visibility = Visibility.Hidden;
            }

            // Phone
            if (phoneCustomTextBox.customTB.Text.Length < 10 || phoneCustomTextBox.customTB.Text.Length > 15 || !phoneCustomTextBox.customTB.Text.All(char.IsDigit))
            {
                wrongPhoneTextBlock.Visibility = Visibility.Visible;
                allValid = false;
            }
            else
            {
                wrongPhoneTextBlock.Visibility = Visibility.Hidden;
            }

            // Username
            if (usernameCustomTextBox.customTB.Text.Length < 5 || usernameCustomTextBox.customTB.Text.Length > 15)
            {
                wrongUsernameTextBlock.Visibility = Visibility.Visible;
                allValid = false;
            }
            else
            {
                wrongUsernameTextBlock.Visibility = Visibility.Hidden;
            }

            // Password
            if (passwordCustomTextBox.customTB.Text.Length < 8 || passwordCustomTextBox.customTB.Text.Length > 20)
            {
                wrongPasswordTextBlock.Visibility = Visibility.Visible;
                allValid = false;
            }
            else
            {
                wrongPasswordTextBlock.Visibility = Visibility.Hidden;
            }

            // Confirm Password
            if (passwordConfirmCustomTextBox.customTB.Text != passwordCustomTextBox.customTB.Text)
            {
                wrongPasswordConfirmTextBlock.Visibility = Visibility.Visible;
                allValid = false;
            }
            else
            {
                wrongPasswordConfirmTextBlock.Visibility = Visibility.Hidden;
            }

            // Email
            if (!emailCustomTextBox.customTB.Text.Contains("@"))
            {
                wrongEmailTextBlock.Visibility = Visibility.Visible;
                allValid = false;
            }
            else
            {
                wrongEmailTextBlock.Visibility = Visibility.Hidden;
            }
            return allValid;
        }
    }
}
