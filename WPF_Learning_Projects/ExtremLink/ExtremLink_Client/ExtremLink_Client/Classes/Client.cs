﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtremLink_Client.Classes
{
    /*public class Client
    {
        // A class which represent a client
        private TcpClient tcpClient;
        private string serverIp;
        private int port;
        private NetworkStream stream;

        public Client(string serverIp, int port)
        {
            this.serverIp = serverIp;
            this.port = port;
            this.tcpClient = new TcpClient();
        }

        // Basic client managment commands
        public void Start()
        {
            // The function gets nothing.
            // The function connect to the server and start recieve and send messages via the stream.
            this.tcpClient.Connect(this.serverIp, this.port);
            this.stream = this.tcpClient.GetStream();
            Console.WriteLine("The client connected to the server.");
            while (true)
            {
                // Complete here the mechanizem of sending and reciving messages via the stream.
                this.HandleWithMessages();
            }
            this.Close();
        }

        public void Close()
        {
            // The function gets nothing.
            // The function close the stream and disconnect from the server.
            this.stream.Close();
            this.tcpClient.Close();
        }


        // Messages handle functions
        public byte[] Compress(string data)
        {
            // The function gets a string.
            // The function return the given string after compressing in bytes format.
            if (string.IsNullOrEmpty(data))
                return new byte[0];

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    gzipStream.Write(dataBytes, 0, dataBytes.Length);
                }
                return memoryStream.ToArray();
            }
        }
        public string Decompress(byte[] data)
        {
            // The function gets a byte array.
            // The function return the given byte array as a string after decompressing.
            if (data == null || data.Length == 0)
                return string.Empty;

            using (var memoryStream = new MemoryStream(data))
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                using (var reader = new StreamReader(gzipStream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public List<object> GetMsg()
        {
            // The function gets a message from the client(not by calling).
            // The function returns the message in parts as a list object.
            byte[] buffer = new byte[4096];  // Buffer to store incoming data
            int bytesRead = this.stream.Read(buffer, 0, buffer.Length);  // Read from the stream
            if (bytesRead > 0)
            {
                byte[] receivedData = new byte[bytesRead];
                Array.Copy(buffer, receivedData, bytesRead);
                string decompressedMessage = Decompress(receivedData);
                string[] parts = decompressedMessage.Split('|');
                if (parts.Length != 4 || parts[3] != "EOM")
                {
                    throw new InvalidDataException("Message format error: EOM missing or invalid format.");
                }
                string typeOfMessage = parts[0];
                int dataLength = int.Parse(parts[1]);
                string data = parts[2];
                List<object> messageComponents = new List<object> { typeOfMessage, dataLength, data };
                return messageComponents;
            }
            else
            {
                throw new IOException("No data received.");
            }
        }
        public void SendMsg(string typeOfMessage, string data)
        {
            // The function gets 2 strings: 'typeOfMessage' which is a symbol which reprsent the type of the message
            // and 'data' which contains the data which have to be transfered.
            // The function creates a message in a byte array format and send it to the server.
            // The message format: Byte{typeOfMessage(string), dataLength(int), data(string), "EOM"}
            string endOfMessage = "EOM";
            string message = $"{typeOfMessage}|{data.Length}|{data}|{endOfMessage}";
            byte[] compressedMessage = this.Compress(message);
            this.stream.Write(compressedMessage, 0, compressedMessage.Length);
        }

        public void HandleWithMessages()
        {
            // The function get a message in a List<object> formet.
            // The function handle with the message according to the message type according to this protocol:
            // ! - Database functionality
            // @ - 
            // # - 
            List<object> message = this.GetMsg();
            string data = (string)message[2];
            switch (message[0])
            {
                case "!":
                    if(data == "Exist")
                    {
                        // Complete the exist user results.
                    }
                    if(data == "NotExist")
                    {
                        // Complete the unexist user results.
                    }
                    break;
            }
        }
    }*/

    public class Client
    {
        // A class which represent a client.
        // Attributes:
        private Socket udpSocket;
        private Socket tcpSocket;
        private string serverIpAddr;
        private string serverReplay;

        public Socket UDPSocket
        {
            get { return this.udpSocket; }
            set { this.udpSocket = value; }
        }
        public Socket TCPSocket
        {
            get { return this.tcpSocket; }
            set { this.tcpSocket = value; }
        }
        public string ServerReplay
        {
            get { return this.serverReplay; }
            set { this.serverReplay = value; }
        }

        public Client(string serverIpAddr)
        {
            this.serverIpAddr = serverIpAddr;

            // Create UDP socket
            this.udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            // Create TCP socket
            this.tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void ConnectToServer()
        {
            // The function gets nothing.
            // The function connects the TCP socket to the server.
            this.tcpSocket.Connect(this.serverIpAddr, 1234);
        }

        public void Start()
        {
            // The function gets nothing.
            // The function starts the tasks of the functions which handling with packets.
            Console.WriteLine("Server started on UDP port 1234 and TCP port 1847.");

            Task.Run(() => this.HandleUdpCommunication());
            Task.Run(() => this.HandleTcpCommunication());
        }

        private async Task HandleUdpCommunication()
        {
            // The function gets nothing.
            // The fucntion handle with different type of TCP packets which are sent by the server.
        }

        private async Task HandleTcpCommunication()
        {
            // The function gets nothing.
            // The fucntion handle with different type of TCP packets which are sent by the server.
            // The types of message are:
            // ! - Database functionality
            // @ -
            // # - 
            List<object> message = GetMessage(tcpSocket);
            string data = (string)message[2];
            switch (message[0])
            {
                case "!":
                    if(data == "Exist")
                    {
                        // Important Note: the server replay is a string that is accessible
                        // out of the class which can be accessible like from the login page.
                        this.serverReplay = "Exist";
                    }
                    if(data == "NotExist")
                    {
                        this.serverReplay = "NotExist";
                    }
                    break;
            }
        }

        public byte[] Compress(string data)
        {
            // The function gets a string.
            // The function return the given string after compressing in bytes format.
            if (string.IsNullOrEmpty(data))
                return new byte[0];

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    gzipStream.Write(dataBytes, 0, dataBytes.Length);
                }
                return memoryStream.ToArray();
            }
        }
        public string Decompress(byte[] data)
        {
            // The function gets a byte array.
            // The function return the given byte array as a string after decompressing.
            if (data == null || data.Length == 0)
                return string.Empty;

            using (var memoryStream = new MemoryStream(data))
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                using (var reader = new StreamReader(gzipStream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public void SendMessage(Socket clientSocket, string typeOfMessage, string data)
        {
            // The function gets a socket and  2 strings: 'typeOfMessage' which is a symbol which reprsent the type of the message
            // and 'data' which contains the data which have to be transfered.
            // The function creates a message in a byte array format and sends it to the client.
            // The message format: Byte{typeOfMessage(string), dataLength(int), data(string), "EOM"}
            string endOfMessage = "EOM";
            string message = $"{typeOfMessage}|{data.Length}|{data}|{endOfMessage}";
            byte[] compressedMessage = this.Compress(message);
            clientSocket.Send(compressedMessage);
        }
        public List<object> GetMessage(Socket clientSocket)
        {
            // The function gets a socket.
            // The function recieve a message from the socket and returns the message in parts as a list object.
            byte[] buffer = new byte[4096];
            int bytesRead = clientSocket.Receive(buffer);
            string[] messageParts = this.Decompress(buffer).Split('|');

            if (messageParts.Length != 4)
            {
                throw new Exception("Error: The message isn't in the right format!");
            }

            if (messageParts[3] != "EOM")
            {
                throw new Exception("End of message not received correctly. The message is cut.");
            }

            List<object> messagePartsList = new List<object>();
            for (int i = 0; i < messageParts.Length; i++)
            {
                if (i == 1)
                {
                    messagePartsList.Add(int.Parse(messageParts[i]));
                }
                messagePartsList.Add(messageParts[i]);
            }
            return messagePartsList;
        }

    }
}
