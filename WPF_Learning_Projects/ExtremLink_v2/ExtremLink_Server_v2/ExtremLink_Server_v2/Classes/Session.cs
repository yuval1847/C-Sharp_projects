using System;
using System.Windows;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;


namespace ExtremLink_Server_v2.Classes
{

    public class Session
    {
        /*
        A class which represent a session.
        */

        // Attributes:
        // A parameter which represent the recording date of the session's video.
        private DateTime recordedTime;
        public DateTime RecordedTime
        {
            get { return this.recordedTime; }
        }

        // A parameter which contain the video itself as a byte array.
        private byte[] videoContent;
        public byte[] VideoContent
        {
            get { return this.videoContent; }
            set { this.videoContent = value; }
        }

        private string username;
        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        // A constructor
        public Session(DateTime recordedTime, string username)
        {
            this.recordedTime = recordedTime;
            this.username = username;
        }

        // A function which upload the session to database
        private SqlConnection ConnectToDB()
        {
            // Input: Nothing.
            // Output: An SqlConnection object which connected to the sessions database.
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // Navigate up from bin/Debug to the project root
            string projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
            string dbPath = Path.Combine(projectRoot, "Databases", "SessionRecordsDB");
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }
        
        public void UploadSessionToDatabase()
        {
            // Input: Nothing.
            // Output: The function uploads the session to the database.
            string amoutOfSessionsSqlQuery = "SELECT COUNT(*) FROM [dbo].[Table]";
            string sessionUploadQuery = "INSERT INTO [dbo].[Table] (Id, Username, VideoData, RecordedTime) VALUES (@Id, @Username, @VideoData, @RecordedTime)";
            int amountOfSessions;
            using (SqlConnection conn = this.ConnectToDB())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand com2 = new SqlCommand(sessionUploadQuery, conn))
                    {
                        // Retrieve the amout of users in the database
                        using (SqlCommand com1 = new SqlCommand(amoutOfSessionsSqlQuery, conn))
                        {
                            amountOfSessions = (int)com1.ExecuteScalar();
                        }
                        // Add parameters to the command to avoid SQL injection
                        com2.Parameters.AddWithValue("@Id", amountOfSessions+1);
                        com2.Parameters.AddWithValue("@Username", this.username);
                        com2.Parameters.AddWithValue("@VideoData", this.videoContent);
                        com2.Parameters.AddWithValue("@RecordedTime", this.recordedTime.ToString());
                        com2.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection failed: " + ex.Message);
                }
            }
        }
    }
}
