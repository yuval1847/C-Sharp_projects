using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremLink_Client.Classes
{
    class Session
    {
        /*
        A class which represent a single session
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

        // A function which convert the video content to actual video.
    }
}
