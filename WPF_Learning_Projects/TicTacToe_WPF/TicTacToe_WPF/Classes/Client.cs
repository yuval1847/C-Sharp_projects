using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace TicTacToe_WPF.Classes
{
    internal class Client
    {
        private int port;
        private string ipAddr;
        private Player player;
        private TcpClient clientSocket;
        private int id;
        private NetworkStream stream;


        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        public string IpAddr
        {
            get { return ipAddr; }
            set { ipAddr = value; }
        }
        public Player Player
        {
            get { return player; }
            set { player = value; }
        }
        public TcpClient ClientSocket
        {
            get { return clientSocket; }
            set { clientSocket = value; }
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public NetworkStream Stream
        {
            get { return stream; }
            set { stream = value; }
        }


        public Client()
        {
            this.Port = 1847;
            this.IpAddr = "127.0.0.1";
            this.Player = new Player("x");
            this.ID = 102;
        }

        public void ConnectToServer()
        {
            this.ClientSocket = new TcpClient("0.0.0.0", this.Port);
        }


        public List<object> GetMsg()
        {
            // The function gets a message from the client.
            // The function return the message as list of values.
            this.Stream = this.ClientSocket.GetStream();
            byte[] buffer = new byte[256];
            int bytesRead = this.Stream.Read(buffer, 0, buffer.Length);

            string messageType = Encoding.ASCII.GetString(buffer, 0, 2); // 2 bytes for the message type
            int id = BitConverter.ToInt16(buffer, 2);                    // 2 bytes for the ID
            int strLength = BitConverter.ToInt16(buffer, 4);                // 2 bytes for the length of the string
            string message = Encoding.ASCII.GetString(buffer, 6, strLength); // The message string

            List<object> msgLst = new List<object>();
            msgLst.Add(messageType);
            msgLst.Add(id);
            msgLst.Add(message);

            return msgLst;
        }
        public void SendMsg(string msgType, string msgContent)
        {
            // The function gets the message properties.
            // msgType(string): '!' - end the game, '#' - place on board
            // msgContent(string): end the game - 'x/o/TIE', place on board - '[x][y]'.
            // The function sends the message to the server.

            // Now prepare the cle's response (in the same format as the custom protocol)
            byte[] messageType = Encoding.ASCII.GetBytes(msgType);
            byte[] messageId = BitConverter.GetBytes((short)this.ID);
            byte[] messageContentLength = BitConverter.GetBytes((short)msgContent.Length);
            byte[] messageContent = Encoding.ASCII.GetBytes(msgContent);

            // Combine all parts into one byte array
            byte[] msgPacket = new byte[messageType.Length + messageId.Length + messageContentLength.Length + messageContent.Length];
            Buffer.BlockCopy(messageType, 0, msgPacket, 0, messageType.Length);
            Buffer.BlockCopy(messageId, 0, msgPacket, messageType.Length, messageId.Length);
            Buffer.BlockCopy(messageContentLength, 0, msgPacket, messageType.Length + messageId.Length, messageContentLength.Length);
            Buffer.BlockCopy(messageContent, 0, msgPacket, messageType.Length + messageId.Length + messageContentLength.Length, messageContent.Length);

            // Send the response to the client
            this.Stream.Write(msgPacket, 0, msgPacket.Length);
            Console.WriteLine("Sent msg to server");
        }
    }
}
