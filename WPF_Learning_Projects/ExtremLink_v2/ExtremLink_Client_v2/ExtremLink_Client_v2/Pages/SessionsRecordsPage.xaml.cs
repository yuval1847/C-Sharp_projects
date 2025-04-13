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
        private User currentUser = User.UserInstance;
        private ContentControl contentMain;

        // A sessions Ilist which contains the session of the user.
        private IList<Session> userSessions;


        public SessionsRecordsPage(ContentControl contentMain)
        {
            this.contentMain = contentMain;
            this.Dispatcher.Invoke(() => { usernameTextBlock.Text = this.currentUser.UserName; });
            this.userSessions = new List<Session>();
            this.LoadRecords();
            InitializeComponent();
        }


        private void LoadRecords()
        {
            // Input: Nothing.
            // Output: The function load over the SessionRecordsList the records from the sessions records database.
            // Session.
            Session.SendRequest(TypeOfSessionRequest.GetSessionProperties);
            Dispatcher.Invoke(() =>
            {
                
            });
        }

    }
}
