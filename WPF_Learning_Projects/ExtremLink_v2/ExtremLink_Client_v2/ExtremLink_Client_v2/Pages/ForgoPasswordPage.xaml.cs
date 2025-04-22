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

        private void SendsPassViaEmailBtn_Click(object sender, RoutedEventArgs e)
        {
            // A function which sends to the given user his password via his email.

        }
    }
}
