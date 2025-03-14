using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremLink_Server.Classes
{
    public class Log
    {
        //******************************************
        // A class which represent the server's log.
        //******************************************

        // Attributes:
        
        // An Ilist of the messages of the Log page
        private IList<string> messages;
        public IList<string> Messages
        {
            get { return this.messages; }
            set { this.messages = value; }
        }


        // Singleton behavior:
        private static Log logInstance;
        public static Log LogInstance
        {
            get
            {
                if (logInstance == null)
                {
                    logInstance = new Log();
                }
                return logInstance;
            }
        }

        // Constructor:
        private Log()
        {
            this.messages = new List<string>();
        }


        // Functions:
        public void AddMessage(string message)
        {
            // Input: A string which represent a new message.
            // Output: The function adds the given message to the messages Ilist.
            this.messages.Add(message);
        }
    }
}
