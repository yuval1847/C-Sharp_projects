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
using OpenCvSharp.Extensions;
using MediaInfo;
using System.Data.SqlClient;


namespace ExtremLink_Server.Classes
{

    internal class Session
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

        // A parameter which indicate the duration of the video
        private TimeSpan videoDuration;
        public TimeSpan VideoDuration
        {
            get { return this.videoDuration; }
        }

        // A parameter which contain the video itself as a byte array.
        private byte[] videoContent;
        public byte[] VideoContent
        {
            get { return this.videoContent; }
        }

        // OpenCv VideoWriter
        private VideoWriter videoWriter;
        private string tempVideoPath; // Temporary file path for video writing
        private int width, height, fps;


        // A constructor
        public Session(DateTime recordedTime, int width, int height, int fps)
        {
            this.recordedTime = recordedTime;
            this.videoDuration = new TimeSpan();
            this.width = width;
            this.height = height;
            this.fps = fps;
        }
        
        // A function which start the recoding by init the 
        public void StartRecording()
        {
            // Input: Nothing.
            // Output: The function create a temp mp4 file to store the frames as a video and
            // initializes the video writer which makes it able to start getting frames.
            tempVideoPath = Path.Combine(Path.GetTempPath(), $"session_{this.recordedTime.ToString()}.mp4");
            videoWriter = new VideoWriter(tempVideoPath, FourCC.MP4V, fps, new OpenCvSharp.Size(width, height));
        }

        // A function which add a frame to the entire session's video.
        public void AddFrame(BitmapImage frame)
        {
            // Input: The function gets a bitmap object.
            // Output: The function add the frame to the videoWriter
            Bitmap bitmapFrame = ConvertBitmapImageToBitmap(frame);
            Mat matFrame = BitmapConverter.ToMat(bitmapFrame);
            Cv2.Resize(matFrame, matFrame, new OpenCvSharp.Size(this.width, this.height));
            videoWriter.Write(matFrame);
        }

        // A function which converts BitmapImage to Bitmap
        private Bitmap ConvertBitmapImageToBitmap(BitmapImage bitmapImage)
        {
            // Input: A bitmap image object.
            // Output: The given BitmapImage object as a Bitmap object.
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(outStream);
                return new Bitmap(outStream);
            }
        }

        // A function which return the duration of an mp4 video
        private TimeSpan GetVideoDuration(string filePath)
        {
            // Input: A path which represent the path of the mp4 file.
            // Output: A TimeSpan object which represent the duration time of the video.
            var mediaInfo = new MediaInfo.MediaInfo();
            mediaInfo.Open(filePath);
            string durationStr = mediaInfo.Get(StreamKind.Video, 0, "Duration");
            mediaInfo.Close();
            if (double.TryParse(durationStr, out double durationMs))
            {
                return TimeSpan.FromMilliseconds(durationMs);
            }
            throw new Exception("Could not determine video duration");
        }

        // A function which stops recording and store video in byte array
        public void StopRecording()
        {
            // Input: Nothing.
            // Output: The function stops the recording by stop adding frames,
            // saves the video duration, store the video content and deletes the temp video file.
            this.videoWriter.Release();
            this.videoContent = File.ReadAllBytes(this.tempVideoPath);
            this.videoDuration = this.GetVideoDuration(this.tempVideoPath);
            File.Delete(this.tempVideoPath);
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
        /*
        public void UploadSessionToDatabase()
        {
            // Input: Nothing.
            // Output: The function uploads the session to the database.
            string amoutOfSessionsSqlQuery = "SELECT COUNT(*) FROM [dbo].[Table]";
            string sessionUploadQuery = "INSERT INTO [dbo].[Table] (Id, Username, VideoData, RecordedTime, Duration) VALUES (@Id, @Username, @VideoData, @RecordedTime, @Duration)";
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
                        com2.Parameters.AddWithValue("@Username", User.UserInstance.UserName);
                        com2.Parameters.AddWithValue("@VideoData", this.videoContent);
                        com2.Parameters.AddWithValue("@RecordedTime", this.recordedTime.ToString());
                        com2.Parameters.AddWithValue("@Duration", this.videoDuration.ToString());
                        com2.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection failed: " + ex.Message);
                }
            }
        }
        */
        // Sending client sessions functions:
        
    }
}
