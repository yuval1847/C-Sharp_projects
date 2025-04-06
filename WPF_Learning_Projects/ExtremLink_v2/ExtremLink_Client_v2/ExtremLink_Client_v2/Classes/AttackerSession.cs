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
using System.Windows.Controls;
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
                try
                {
                    this.videoWriter.Release();
                    this.videoWriter.Dispose();
                }
                catch (Exception ex){}
            }
            this.videoContent = this.ConvertMP4ToByteArr(); 
        }

        private void SendSessionContentToServer()
        {
            // Input: Nothing.
            // Output: The function sends the session to the server via the attacker's session tcp socket. 
            // Define packet size (MTU-safe)
            
            const int packetSize = 1400; // Keeping under the MTU limit
            int totalPackets = (int)Math.Ceiling((double)this.videoContent.Length / packetSize);

            // Generate a random stream ID to identify this data stream
            int streamId = new Random().Next(1, int.MaxValue);

            // Send packets
            // MessageBox.Show($"{totalPackets}");
            for (int i = 0; i < totalPackets; i++)
            {
                // Calculate packet bounds
                int offset = i * packetSize;
                int size = Math.Min(packetSize, this.videoContent.Length - offset);

                // Construct packet (12 bytes of metadata + segment data)
                byte[] packet = new byte[size + 12];
                BitConverter.GetBytes(streamId).CopyTo(packet, 0);         // 4 bytes: Stream ID
                BitConverter.GetBytes(totalPackets).CopyTo(packet, 4);     // 4 bytes: Total packets
                BitConverter.GetBytes(i).CopyTo(packet, 8);                // 4 bytes: Packet index
                Array.Copy(this.videoContent, offset, packet, 12, size);               // Segment data

                // Send packet
                Attacker.AttackerInstance.SessionTcpSocket.Send(packet);
            }
        }
        public void SendSessionToServer()
        {
            // Input: Nothing.
            // Output: The function sends to the server the session's video content.
            this.SendSessionContentToServer();
        }
    }
}
