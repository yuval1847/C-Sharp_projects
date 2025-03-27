using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremLink_Client_v2.Classes
{
    enum TypeOfRequest
    {
        GetSessionProperties,
        GetSessionContent
    }


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

        // A parameter which contain the video itself as a byte array.
        private byte[] videoContent;
        public byte[] VideoContent
        {
            get { return this.videoContent; }
            set { this.videoContent = value; }
        }

        // A constructor
        public Session(string recordedTime, byte[] videoContent)
        {
            this.recordedTime = recordedTime;
            this.videoContent = videoContent;
        }

        // A function which convert the video content to actual video.


        // A function which request from the server the sessions for a specific user
        private static string GenerateGetAllSessionsPropertiesQuery(string username)
        {
            // Input: A string which represent a username.
            // Output: A string json request to get all the sessions
            // properties which are belong to the specified user.
            return $"{{\"username\":\"{username}\"}}";
        }
        private static string GenerateGetSessionContent(int id)
        {
            // Input: A which integer which represent session id.
            // Output: A string json request to get the content of the
            // session based on the session id.
            return $"{{\"Id\":\"{id}\"}}";
        }

        /*
        private static void SendSessionRequestToServer(Client client, string message)
        {
            // Input: The function gets a Client object and a string which represent the message.
            // Output: The function sends the server the message via the tcp socket.
            client.SendTCPMessageToClient("$", message);
        }
        */

        public static void SendRequest(Client client, TypeOfRequest requestType, int id=0)
        {
            // Input: A TypeOfRequest enum value.
            // Output: The function handle which different type of request and executes them.
            string requestQuery = "";

            switch (requestType)
            {
                case TypeOfRequest.GetSessionProperties:
                    requestQuery = GenerateGetAllSessionsPropertiesQuery(User.UserInstance.UserName);
                    break;

                case TypeOfRequest.GetSessionContent:
                    requestQuery = GenerateGetSessionContent(id);
                    break;
            }

            // SendSessionRequestToServer(client, requestQuery);
        }
    }
}
