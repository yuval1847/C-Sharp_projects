using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Networking modules
using System.Net;
using System.Net.Sockets;

namespace ExtremLink_Server.Classes
{
    internal class Server
    {
        /* A class which represent a server */
        private int port;
        private TcpListener listener;

        public int Port { get { return this.port; }}

        public Server(int port)
        {
            this.port = port;
            listener = new TcpListener(IPAddress.Any, port);
        }

        // Basic server managment commands
        public void Start()
        {
            // The function gets nothing.
            // The function starts the server communication with the client.
            this.listener.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                Console.WriteLine("Waiting for client...");
                TcpClient client = this.listener.AcceptTcpClient();
                Console.WriteLine("Client connected!");


                /*
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received: {receivedMessage}");

                string responseMessage = "Hello from server!";
                byte[] responseData = Encoding.ASCII.GetBytes(responseMessage);
                stream.Write(responseData, 0, responseData.Length);
                Console.WriteLine("Response sent.");
                */
                client.Close();
            }
        }

        public void Stop()
        {
            this.listener.Stop();
        }
    }
}
