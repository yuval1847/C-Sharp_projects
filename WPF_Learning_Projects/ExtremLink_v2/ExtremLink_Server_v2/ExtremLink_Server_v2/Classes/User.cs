using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremLink_Server_v2.Classes
{
    public class User
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


        public User()
        {
            this.username = "";
        }
    }
}
