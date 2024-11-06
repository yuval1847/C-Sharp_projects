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
    public partial class SignUpPage : Page
    {
        private ContentControl contentMain;
        private Classes.Client client;

        public ContentControl ContentMain
        {
            get { return contentMain; }
            set { this.contentMain = value; }
        }


        public SignUpPage()
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
                if (this.client.ServerRespond == "SuccessfullyAdded")
                {
                    
                }
                else if (this.client.ServerRespond == "NotAdded")
                {
                    
                }
            }
        }

        private bool IsAllParametersRight()
        {
            // The function gets nothing.
            // The function returns true if all the sign-up parameter are fit the 
            // requirments, otherwise, it returns false and show textboxes of the problems.
            
            
            return true;
        }
    }
}
