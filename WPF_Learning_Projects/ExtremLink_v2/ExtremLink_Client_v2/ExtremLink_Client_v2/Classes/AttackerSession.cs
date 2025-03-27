using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        // OpenCv VideoWriter
        private VideoWriter videoWriter;
        private string tempVideoPath; // Temporary file path for video writing
        private int width, height;
        
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
                new OpenCvSharp.Size(width, height));
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

        public void Stop()
        {
            this.isActive = false;
            this.videoWriter.Release();
            this.videoWriter.Dispose();
        }
    }
}
