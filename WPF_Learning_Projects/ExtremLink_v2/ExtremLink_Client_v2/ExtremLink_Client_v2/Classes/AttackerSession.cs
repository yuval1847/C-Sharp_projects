using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ExtremLink_Client_v2.Classes
{
    internal class AttackerSession
    {
        /*
        A class which represent a session.
        */

        // A integer constant which indicate the amount of FPS of each video
        private readonly int FPS = 24;

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

        // OpenCv VideoWriter
        private VideoWriter videoWriter;
        private string tempVideoPath;
        private int width, height;

        // A MutexLock for the videoWriter
        private readonly object lockObj = new object();

        private bool isActive;
        public bool IsActive
        {
            get { return this.isActive; }
        }

        // A constructor
        public AttackerSession(int width, int height)
        {
            this.recordedTime = DateTime.Now;
            this.tempVideoPath = $"{this.recordedTime.ToString("yyyy-MM-dd_HH-mm-ss")}_record.mp4";
            this.width = width;
            this.height = height;
            this.isActive = false;
        }


        // A function which init the writer and enables it to start getting frames
        public void Start()
        {
            this.videoWriter = new VideoWriter(
                this.tempVideoPath,
                FourCC.H264,
                FPS,
                new OpenCvSharp.Size(this.width, this.height));
            this.isActive = true;
        }

        private Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                using (var tempBitmap = new Bitmap(outStream))
                {
                    return new Bitmap(tempBitmap);
                }
            }
        }

        // A function which add frames to the session
        public void WriteFrame(BitmapImage bitmapImage)
        {
            if (bitmapImage != null)
            {
                lock (lockObj)
                {
                    using (Bitmap bitmap = this.BitmapImageToBitmap(bitmapImage))
                    {
                        using (Mat mat = BitmapConverter.ToMat(bitmap))
                        {
                            Cv2.Resize(mat, mat, new OpenCvSharp.Size(width, height));
                            this.videoWriter.Write(mat);
                        }
                    }
                }
            }
        }

        private byte[] ConvertMP4ToByteArr()
        {
            // Input: Nothing.
            // Output: A byte array which represent the session as mp4 file.
            try
            {
                return File.ReadAllBytes(this.tempVideoPath);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Stop()
        {
            this.isActive = false;
            lock (lockObj)
            {
                this.videoWriter.Release();
                this.videoWriter.Dispose();
            }
            this.videoContent = this.ConvertMP4ToByteArr(); 
        }

        private string GenerateSessionRecordedTimeMessage()
        {
            // Input: Nothing.
            // Output: The function creates a json format request of the recorded time of the session.
            return $"{{\"RecordedTime\":\"{this.recordedTime.ToString()}\"}}";
        }

        private void SendSessionRecordedTimeToServer()
        {
            // Input: Nothing.
            // Output: The function sends the session's recorded time (except the content) to the server via the attacker's tcp socket.
            string message = this.GenerateSessionRecordedTimeMessage();
            Attacker.AttackerInstance.SendTCPMessageToClient("📹🕑", message);
        }
        private void SendSessionContentToServer()
        {
            // Input: Nothing.
            // Output: The function sends the session to the server via the attacker's session tcp socket. 
            using (FileStream fileStream = new FileStream(this.tempVideoPath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[8192];
                int bytesRead;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Attacker.AttackerInstance.SessionTcpSocket.Send(buffer, 0, bytesRead, SocketFlags.None);
                }
            }
        }
        public void SendSessionToServer()
        {
            // Input: Nothing.
            // Output: The function sends to the server the session recorded time and it's content.
            this.SendSessionRecordedTimeToServer();
            this.SendSessionContentToServer();
        }
    }
}
