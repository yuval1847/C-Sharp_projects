using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExtremLink_Client.Classes
{
    internal class Client
    {
        /* A class which represent a client */
        private TcpClient tcpClient;
        private string serverIp;
        private int port;
        private NetworkStream stream;

        public Client(string serverIp, int port)
        {
            this.serverIp = serverIp;
            this.port = port;
            this.tcpClient = new TcpClient();
            this.tcpClient.Connect(this.serverIp, this.port);
            this.stream = this.tcpClient.GetStream();
            Console.WriteLine("Connected to the server!");
        }

        // Basic client managment commands
        public void Start()
        {
            while (true)
            {
                /*
                try
                {
                    byte[] buffer = new byte[1024];
                    // Send message to server
                    Console.Write("Enter message to send: ");
                    string message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message)) break;

                    byte[] data = Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine($"Sent: {message}");

                    // Read response from server
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string responseMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received from server: {responseMessage}");
                }
                catch (Exception)
                {
                    Console.WriteLine("Disconnected from server.");
                    break;
                }
                */
            }
        }

        public void Close()
        {
            this.stream.Close();
            this.tcpClient.Close();
        }

        // Handeling messages
        public List<object> GetMessage()
        {
            // The function gets a message from the client(not by calling).
            // The function returns the message in parts as a list object.

        }

        public void SendMsg(string typeOfMessage, string data)
        {
            // The function gets 2 strings: 'typeOfMessage' which is a symbol which reprsent the type of the message
            // and 'data' which contains the data which have to be transfered.
            // The function creates a message in a byte array format and send it to the client.
        }

    }
}
