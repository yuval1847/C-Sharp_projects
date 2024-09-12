using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prog_1_winforms
{
    class ClientConnect
    {
        public int clientnum;
        public string name;  //שם הלקוח שיתחבר לשרת 
        public Socket clientSocket; // הסוקט שדרכו הלקוח התחבר לשרת 
        public Thread clientThread;//התהליך של הלקוח שדרכו תתנהל כל ההתקשרות של השרת מול הלקוח 
    }

}
