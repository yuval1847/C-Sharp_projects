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
using TicTacToe_WPF.Classes;

namespace TicTacToe_WPF.Pages
{
    /// <summary>
    /// Interaction logic for UserTypeChoosingPage.xaml
    /// </summary>
    public partial class UserTypeChoosingPage : UserControl
    {
        private ContentControl mainContentControl;

        public UserTypeChoosingPage(ContentControl mainContentControl)
        {
            InitializeComponent();
            this.mainContentControl = mainContentControl;
        }

        private void ServerChoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            Classes.Server server = new Classes.Server();
            WaitingTextBlock.Visibility = Visibility.Visible;
            server.ListenAndConnect();
            this.mainContentControl = new Pages.GamePage(this.mainContentControl, server);
        }

        private void ClientChoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            Classes.Client client = new Classes.Client();
            client.ConnectToServer();
            this.mainContentControl = new Pages.GamePage(this.mainContentControl, server);
        }
    }
}
