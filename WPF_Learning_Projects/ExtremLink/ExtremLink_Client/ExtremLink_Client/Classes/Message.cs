using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremLink_Client.Classes
{
    enum TypeOfMessage
    {
        UserDatabase,
        Frame
    }

    internal class Message
    {
        private TypeOfMessage typeOfMessage;
        private IList<string> content;
        private const int contentPartsSize = 4096;
        private const string EOM = "EOM";

        public int contentPartsAmount
        {
            get { return this.content.Count / contentPartsSize; }
        }

        public Message(TypeOfMessage typeOfMessage, string content)
        {
            this.typeOfMessage = typeOfMessage;

            // Fix it by using a round function that round the division for the biggest number
            int contentPartsAmount = this.content.Count % contentPartsSize == 0 ? this.content.Count / contentPartsSize : ;
            if (this.content.Count % contentPartsSize != 0)
            {
                contentPartsAmount++;
            }
            for (int i = 0; i < )
        }

    }
}
