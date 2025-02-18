using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ExtremLink_Server.Classes
{

    internal class Session
    {
        /*
        A class which represent a session 
        */

        // Attributes:
        // A parameter which represent the recording date of the session's video.
        private string recordedTime;
        public string RecordedTime
        {
            get { return this.recordedTime; }
            set { this.recordedTime = value; }
        }

        // A parameter which indicate the duration of the video
        private string videoDuration;
        public string VideoDuration
        {
            get { return this.videoDuration; }
            set { this.videoDuration = value; }
        }

        // A parameter which contain the video itself as a byte array.
        private byte[] videoContent;
        public byte[] VideoContent
        {
            get { return this.videoContent; }
            set { this.videoContent = value; }
        }

        // A constructor
        public Session(string recordedTime, string videoDuration, byte[] videoContent)
        {
            this.recordedTime = recordedTime;
            this.videoDuration = videoDuration;
            this.videoContent = videoContent;
        }


        // A function which add a frame to the entire session's video.
        public void AddFrame(BitmapImage frame)
        {
            // Input: The function gets a bitmap object.
            // Output: The function add the frame to the video content.
            byte[] byteFrame = this.ConvertBitmapImageToByteArray(frame);
            if (this.videoContent == null)
            {
                this.videoContent = byteFrame;
            }
            else
            {
                byte[] newVideoContent = new byte[this.videoContent.Length + byteFrame.Length];
                Buffer.BlockCopy(this.videoContent, 0, newVideoContent, 0, this.videoContent.Length);
                Buffer.BlockCopy(byteFrame, 0, newVideoContent, this.videoContent.Length, byteFrame.Length);
                this.videoContent = newVideoContent;
            }
        }


        // A function which convert frames to byte array.
        private byte[] ConvertBitmapImageToByteArray(BitmapImage frame)
        {
            // Input: The function gets a BitmapImage object.
            // Output: The function converts the given BitmapImage
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder(); // You can use JpegBitmapEncoder, etc.
                encoder.Frames.Add(BitmapFrame.Create(frame));
                encoder.Save(ms);
                return ms.ToArray();
            }
        }
    }
}
