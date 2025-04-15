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


namespace ExtremLink_Client_v2.Pages
{
    /// <summary>
    /// Interaction logic for SessionsRecordsPage.xaml
    /// </summary>
    public partial class SessionsRecordsPage : UserControl
    {
        private ContentControl contentMain;

        public SessionsRecordsPage(ContentControl contentMain)
        {
            this.contentMain = contentMain;
            InitializeComponent();
            this.Dispatcher.Invoke(() => { usernameTextBlock.Text = User.UserInstance.UserName; });
            this.LoadRecords();
        }


        private void LoadRecords()
        {
            // Input: Nothing.
            // Output: The function load over the SessionRecordsList the records from the sessions records database.
            // Session.
            Session.SendRequest(TypeOfSessionRequest.GetSessionProperties);
            Thread.Sleep(2000);
            Dispatcher.Invoke(() =>
            {
                // Clear the current items
                SessionRecordsList.Items.Clear();

                // Check if there are any sessions for the user
                if (User.UserInstance.UserSessions != null)
                {
                    foreach (var session in User.UserInstance.UserSessions)
                    {
                        var displayItem = new
                        {
                            Id = session.Id,
                            Date = session.RecordedTime.ToString("yyyy-MM-dd HH:mm"),
                            VideoName = $"Session_{session.Id}.mp4"
                        };

                        SessionRecordsList.Items.Add(displayItem);
                    }
                }
                else
                {
                    MessageBox.Show("No session records were found.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });
        }

        

    }
}
