using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremLink_Client_v2.Classes
{
    enum TypeOfSessionRequest
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

        // A constructor
        public Session()
        {

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
            return $"{{\"requestType\":\"{requestType}\",\"Id\":\"{id}\"}}";
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

        // Get sessions properties/content functions
        
        /*private static IList<Session> GetSessionsProperties(string username)
        { 
            // Input: A string which represent a username.
            // Output: An Ilist of sessions which contains the properties of the user's session.

        }*/
    }
}
