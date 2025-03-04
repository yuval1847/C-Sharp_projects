using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ExtremLink_Server.Classes
{
    internal class Client
    {
        //****************************************************************************
        // A class which represent a client (attacker or victim) from the server side.
        //****************************************************************************

        // Attributes:

        // Integer constants which represent the sockets' ports:
        protected readonly int TCP_PORT = 1234;
        protected readonly int UDP_PORT = 1847;

        // A string which represent the IP address of this client:
        private string ipAddress;
        public string IpAddress
        {
            get { return this.ipAddress; }
        }

        // A socket object of TCP:
        private Socket tcpSocket;
        public Socket TcpSocket
        {
            get { return this.tcpSocket; }
            set { this.tcpSocket = value; }
        }

        // A socket object of UDP:
        private Socket udpSocket;
        public Socket UdpSocket
        {
            get { return this.udpSocket; }
            set { this.udpSocket = value; }
        }

        public Client(string ipAddress)
        {
            this.ipAddress = ipAddress;
        }


        // Sending basic messages functions:
        private void SendMessageToClient(string message, string typeOfMessage, Socket socket)
        {
            // Input: A string which represent the message and the socket.
            // Output: The function sends the message via the given socket.
            byte[] buffer = Encoding.UTF8.GetBytes($"{typeOfMessage}|{message.Length}|{message}|EOM"); // EOM - "end of message"

            // Only compress if it's TCP
            switch (socket.ProtocolType)
            {
                case ProtocolType.Tcp:
                    socket.Send(buffer);
                    break;
                case ProtocolType.Udp:
                    socket.SendTo(buffer, new IPEndPoint(IPAddress.Parse(this.ipAddress), 1847));
                    break;
            }
        }
        public void SendTCPMessageToClient(string message, string typeOfMessage)
        {
            // Input: A string which represent the message.
            // Output: The function sends the message via the tcp socket.
            this.SendMessageToClient(message, typeOfMessage, this.tcpSocket);
        }
        public void SendUDPMessageToClient(string message, string typeOfMessage)
        {
            // Input: A string which represent the message.
            // Output: The function sends the message via the tcp socket.
            this.SendMessageToClient(message, typeOfMessage, this.udpSocket);
        }

        // Getting basic messages functions:
        private List<object> OrderMessage(string message)
        {
            // The function gets a string which represent a message received from the client.
            // The fucntion returns the message as a list object which contains the message by it's parameters.
            string[] messageParts = message.Split('|');

            // Validate message format
            if (messageParts.Length != 4)
            {
                throw new Exception("The message isn't in the right format!");
            }
            if (messageParts[3] != "EOM")
            {
                throw new Exception("End of message wasn't fully received");
            }

            List<object> messagePartsList = new List<object>
                {
                    messageParts[0],                    // Message type
                    int.Parse(messageParts[1]),         // Data length
                    messageParts[2],                    // Data content
                    messageParts[3]                     // End of message marker
                };

            return messagePartsList;
        }
        public List<object> GetTCPMessageFromClient() 
        {
            // Input: Nothing.
            // Output: The received message from the client via he tcp socket as a list of parameters.
            byte[] buffer = new byte[4096];
            int receivedBytes = this.tcpSocket.Receive(buffer);
            return this.OrderMessage(Encoding.UTF8.GetString(buffer, 0, receivedBytes));
        }
        public List<object> GetUDPMessageFromClient()
        {
            // Input: Nothing.
            // Output: The received message from the client via the udp socket as a list of parameters.
            byte[] buffer = new byte[4096];
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            int receivedBytes = this.udpSocket.ReceiveFrom(buffer, ref remoteEndPoint);
            return this.OrderMessage(Encoding.UTF8.GetString(buffer, 0, receivedBytes));
        }
    }
}
