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

namespace ExtremLink_Server.Pages
{
    /// <summary>
    /// Interaction logic for ConnectingPage.xaml
    /// </summary>
    public partial class ConnectingPage : UserControl
    {
        private ContentControl contentMain;

        public ConnectingPage(ContentControl contentMain)
        {
            this.contentMain = contentMain;
            InitializeComponent();
            Classes.Server server = new Classes.Server();
            waitingTextBlock.Text = "Waiting for client to login...";
            Thread clientMessagesHandlingThread = new Thread(server.Start);
            clientMessagesHandlingThread.Start();
        }
    }
}
