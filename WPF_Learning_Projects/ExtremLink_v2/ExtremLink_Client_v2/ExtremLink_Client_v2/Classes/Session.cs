using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenCvSharp;
using System.Text.RegularExpressions;

namespace ExtremLink_Client_v2.Classes
{
    public enum TypeOfSessionRequest
    {
        GetSessionProperties,
        GetSessionContent
    }


    public class Session
    {
        /*
        A class which represent a single session
        */

        

        // Attributes:
        // A parameter which represent the recording date of the session's video.
        private DateTime recordedTime;
        public DateTime RecordedTime
        {
            get { return this.recordedTime; }
            set { this.recordedTime = value; }
        }

        // A parameter which contain the video itself as a byte array.
        private byte[] videoContent;
        public byte[] VideoContent
        {
            get { return this.videoContent; }
            set { this.videoContent = value; }
        }

        // A parameter which contains the session's id
        private int id;
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }


        // A constructor
        public Session()
        {

        }
        public Session(DateTime recordedTime, int id)
        {
            this.recordedTime = recordedTime;
            this.id = id;
        }

        // A function which convert the video content to actual video.


        // A function which request from the server the sessions for a specific user
        
        private static string GenerateGetAllSessionsPropertiesQuery(TypeOfSessionRequest requestType, string username)
        {
            // Input: A string which represent a username.
            // Output: A string json request to get all the sessions
            // properties which are belong to the specified user.
            return $"{{\"requestType\":\"{requestType}\",\"typeOfClient\":\"{User.UserInstance.TypeOfClient}\",\"username\":\"{username}\"}}";
        }
        private static string GenerateGetSessionContent(TypeOfSessionRequest requestType, int id)
        {
            // Input: A which integer which represent session id.
            // Output: A string json request to get the content of the
            // session based on the session id.
            return $"{{\"requestType\":\"{requestType}\",\"typeOfClient\":\"{User.UserInstance.TypeOfClient}\",\"Id\":\"{id}\"}}";
        }

        private static void SendSessionRequestToServer(string message)
        {
            // Input: The function gets a Client object and a string which represent the message.
            // Output: The function sends the server the message via the tcp socket.
            if (User.UserInstance.TypeOfClient == TypeOfClient.Attacker)
            {
                Attacker.AttackerInstance.SendTCPMessageToClient("📹🕑", message);
            }
            else
            {
                Victim.VictimInstance.SendTCPMessageToClient("📹🕑", message);
            }
        }

        public static void SendRequest(TypeOfSessionRequest requestType, int id=0)
        {
            // Input: A TypeOfRequest enum value.
            // Output: The function handle which different type of request and executes them.
            string requestQuery = "";

            switch (requestType)
            {
                case TypeOfSessionRequest.GetSessionProperties:
                    requestQuery = GenerateGetAllSessionsPropertiesQuery(requestType, User.UserInstance.UserName);
                    break;

                case TypeOfSessionRequest.GetSessionContent:
                    requestQuery = GenerateGetSessionContent(requestType, id);
                    break;
            }

            SendSessionRequestToServer(requestQuery);
        }

        // A function which organize string of session properties list to properties
        public static IList<Session> FromSessionPropertiesListJsonStrToSessionIlist(string sessionPropertiesList)
        {
            // Input: A string in JSON format which contains user's session properties.
            // Output: An IList of Session objects which represent these session properties.
            int amountOfSessions = Regex.Matches(sessionPropertiesList, "\"Id[0-9]+\"").Count;
            IList<Session> sessions = new List<Session>();

            for (int i = 0; i < amountOfSessions; i++)
            {
                string idPattern = $"\"Id{i + 1}\":\"(.*?)\"";
                string usernamePattern = $"\"Username{i + 1}\":\"(.*?)\"";
                string recordedTimePattern = $"\"RecordedTime{i + 1}\":\"(.*?)\"";

                string id = Regex.Match(sessionPropertiesList, idPattern).Groups[1].Value;
                string username = Regex.Match(sessionPropertiesList, usernamePattern).Groups[1].Value;
                string recordedTime = Regex.Match(sessionPropertiesList, recordedTimePattern).Groups[1].Value;

                sessions.Add(new Session(DateTime.Parse(recordedTime), int.Parse(id)));
            }

            return sessions;
        }


        public static void CreateTempMP4File(byte[] videoContent)
        {
            // Input: A byte array which represents an mp4 file content.
            // Output: The function creates a temp mp4 file and store the video content inside.
            string fileName = "tempRecvSession.mp4";
            if (videoContent == null || videoContent.Length == 0)
            {
                throw new ArgumentException("Video content is null or empty.");
            }
            string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
            File.WriteAllBytes(tempFilePath, videoContent);
        }
    }
}
