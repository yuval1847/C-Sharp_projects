using ExtremLink_Server.Classes;
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
using System.Windows.Threading;

namespace ExtremLink_Server.Pages
{
    /// <summary>
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class LogPage : UserControl
    {

        // Attributes:
        private ContentControl contentMain;
        public ContentControl ContentMain
        {
            get { return this.contentMain; }
        }


        // An integer which represent the current log message index.
        private int messageIndex;


        // A thread which handle with the log:
        private Thread logThread;


        public LogPage(ContentControl contentMain)
        {
            this.contentMain = contentMain;
            this.messageIndex = 0;

            InitializeComponent();

            this.logThread = new Thread(HandleLog);
            this.logThread.Start();
        }


        // Functions:
        private void AddLogEntry(string message)
        {
            // Input: A string which represent a log message.
            // Output: The function add the message to the logListBox.
            Dispatcher.Invoke(() => { logListBox.Items.Add($"> {message}"); });
        }
        private void UpdateLog()
        {
            // Input: Nothing.
            // Output: The function updates the log page according to the LogInstance.
            for (int i = this.messageIndex; i < Log.LogInstance.Messages.Count; i++)
            {
                this.AddLogEntry(Log.LogInstance.Messages[i]);
            }
            this.messageIndex = Log.LogInstance.Messages.Count - 1;

            // Auto-scroll to the latest log.
            Dispatcher.Invoke(() => { logListBox.ScrollIntoView(logListBox.Items[logListBox.Items.Count - 1]); });
        }
        private void HandleLog()
        {
            // Input: Nothing.
            // Output: The function handles the logs.
            while (true)
            {
                while (messageIndex == Log.LogInstance.Messages.Count - 1) { continue; }
                this.UpdateLog();
            }
        }

        private void ClearLogsButton_Click(object sender, RoutedEventArgs e)
        {
            // Input: Nothing.
            // Output: The function clears the logListBox from messages.
            Dispatcher.Invoke(() => { logListBox.Items.Clear(); });
        }
        private void logListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Input: Nothing.
            // Output: The function force the logListBox to be unselectable.
            logListBox.SelectedIndex = -1;
        }
    }
}
