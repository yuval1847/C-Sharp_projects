using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremLink_Server.Classes
{
    internal class User
    {
        /*
        A class which represent the client's user
        */

        // Attributes
        private string username;
        public string UserName
        {
            get { return this.username; }
            set { this.username = value; }
        }


        // Singelton behavior
        private static User userInstance;
        public static User UserInstance
        {
            get
            {
                if (userInstance == null)
                {
                    userInstance = new User();
                }
                return userInstance;
            }
        }


        public User()
        {
            this.username = "";
        }
    }
}
