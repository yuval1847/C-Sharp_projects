using ExtremLink_Server.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
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
        private AutoResetEvent serverResponseEvent;


        public ConnectingPage(ContentControl contentMain)
        {
            this.serverResponseEvent = new AutoResetEvent(false);
            this.contentMain = contentMain;
            InitializeComponent();
        }

        private void startListenBtn_Click(object sender, RoutedEventArgs e)
        {
            // The function got called by clicking on the connect button.
            // The function gets nothing.
            // The function creates the server start the server listener.
            startListenBtn.IsEnabled = false;
            waitingTextBlock.Visibility = Visibility.Visible;
            Thread s = new Thread(this.start);
            s.Start();
        }

        private void start()
        {
            // The function gets nothing.
            // The function start the connection.
            Server.ServerInstance.ConnectToClients();
            Server.ServerInstance.Start();

            Dispatcher.Invoke(() =>
            {
                waitingTextBlock.Text = "Waiting for attacker &\n victim to login...";
            });

            Thread clientMessagesHandlingThread = new Thread(() =>
            {
                while (true)
                {
                    if (Server.ServerInstance.Attacker.User.UserName != "" && Server.ServerInstance.Victim.User.UserName != "")
                    {
                        this.serverResponseEvent.Set();
                        break;
                    }
                    Thread.Sleep(100); // Prevent tight loop
                }
            });
            clientMessagesHandlingThread.Start();

            // Wait for server response event
            this.serverResponseEvent.WaitOne();

            Dispatcher.Invoke(() =>
            {
                this.contentMain.Content = new LogPage(this.contentMain);
            });
        }
    }
}
