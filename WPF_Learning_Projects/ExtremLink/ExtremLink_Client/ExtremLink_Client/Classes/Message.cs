using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private const int ContentPartsSize = 4096;
        // private const string EOM = "EOM";

        public int ContentPartsAmount
        {
            get { return this.content.Count; }
        }

        public Message(TypeOfMessage typeOfMessage, string content)
        {
            this.typeOfMessage = typeOfMessage;

            // Create an ilist of the message in segments
            this.content = new List<string>();
            int partsAmount = (int)Math.Round((double)this.content.Count/ (double)(ContentPartsSize));
            string tempPart;
            for (int i = 0; i < partsAmount-1; i++)
            {
                tempPart = content.Substring(i*ContentPartsSize, ContentPartsSize);
                this.content.Add(tempPart);
            }
            this.content.Add(content.Substring((partsAmount - 1)*ContentPartsSize, content.Length));
        } 
        public Message()
        {

        }
        public IList<byte> ConvertToByte()
        {
            // The function gets nothing.
            // The function returns the message in binary.
            IList<byte> byteContent = new List<byte>();
            for(int i = 0; i < this.ContentPartsAmount; i++)
            {
                byteContent.Add(Encoding.UTF8.GetBytes(this.content[i]));
            }
        }
    }
}
