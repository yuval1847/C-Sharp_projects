﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

// Networking modules
using System.Net;
using System.Net.Sockets;

// Data handling modules
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.IO;
using System.IO.Compression;
using System.ComponentModel;
using System.Windows.Markup;
using System.Threading;
using System.Windows;
using System.Collections;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Net.NetworkInformation;
using System.Windows.Controls;
using System.Diagnostics;
using ExtremLink_Server.Classes;

namespace ExtremLink_Server.Classes
{
    public class Server
    {
        // A class which represent a server.
        // Attributes:
        private string serverIpAddress;
        private string clientIpAddress;
        private Socket udpSocket;
        private Socket tcpSocket;
        private Socket clientTcpSocket;
        private List<object> message;
        private string respond;
        private string udpRespond;
        private BitmapImage currentFrame;
        private EndPoint clientEndPoint;
        public readonly object fileLock = new object();


        public string ServerIpAddress
        {
            get { return this.serverIpAddress; }
            set { this.serverIpAddress = value; }
        }
        public Socket UdpSocket 
        { 
            get { return this.udpSocket; }
            set { this.udpSocket = value; }
        }
        public Socket TcpSocket
        {
            get { return this.tcpSocket; }
            set { this.tcpSocket = value; }
        }
        public Socket ClientTcpSocket
        {
            get { return this.clientTcpSocket; }
            set {this.clientTcpSocket = value;}
        }
        public string Respond
        {
            get { return this.respond; }
            set { this.respond = value; }
        }
        public string UdpRespond
        {
            get { return this.udpRespond; }
            set { this.udpRespond = value; }
        }
        public string ClientIpAddress
        {
            get { return this.clientIpAddress; }
            set { this.clientIpAddress = value; }
        }
        public BitmapImage CurrentFrame
        {
            get { return this.currentFrame; }
            set { this.currentFrame = value; }
        }


        public Server()
        {
            this.respond = "";
            FindIpAddress();
            // Create UDP socket
            this.udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.udpSocket.Bind(new IPEndPoint(IPAddress.Parse(this.serverIpAddress), 1847));
            // Create TCP socket
            this.tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.tcpSocket.Bind(new IPEndPoint(IPAddress.Parse(this.serverIpAddress), 1234));
            this.tcpSocket.Listen(1);
            Console.WriteLine("Waiting for client to connect...");
            this.clientTcpSocket = this.tcpSocket.Accept();
            Console.WriteLine("Client connected.");
            this.clientIpAddress = FindClientIpAddress(this.clientTcpSocket);
            this.clientEndPoint = new IPEndPoint(IPAddress.Parse(this.clientIpAddress), 1847);
        }

        public void FindIpAddress()
        {
            // The function gets nothing.
            // The function sets the server's ip address according to the local ip address.
            IPAddress[] localIpsAddr = Dns.GetHostAddresses(Dns.GetHostName());
            this.serverIpAddress = Convert.ToString(localIpsAddr[localIpsAddr.Length - 1]);
            MessageBox.Show($"Server IP: {this.serverIpAddress}", "IP found!");
        }
        public string FindClientIpAddress(Socket clientSocket)
        {
            // The function gets a socket.
            // The function returns the socket's client's ip.
            var remoteEndPoint = clientSocket?.RemoteEndPoint as IPEndPoint;
            return remoteEndPoint.Address.ToString();
        }
        public void Start()
        {
            // The function gets nothing.
            // The function starts the tasks of the functions which handling with packets.
            Console.WriteLine("Server started on UDP port 1847 and TCP port 1234.");
            Task.Run(() => this.HandleUdpCommunication());
            Task.Run(() => this.HandleTcpCommunication());
        }


        private async Task HandleUdpCommunication()
        {
            // The function gets nothing.
            // The fucntion handle with different type of TCP packets which are sent by the client.
            // & - frames handling.
            while (true)
            {
                this.currentFrame = this.GetFrame();
                Thread.Sleep(1000);
            }
        }

        private async Task HandleTcpCommunication()
        {
            // The function gets nothing.
            // The fucntion handle with different type of TCP packets which are sent by the client.
            // The types of message are:
            // ! - Database functionality
            // & - Frames handling
            // % - Mouse handling
            // $ - Sessions handling
            while (true)
            {
                lock (this){
                    List<object> message = GetMessage(this.clientTcpSocket);
                    string data = (string)message[2];
                    switch (message[0])
                    {
                        case "!":
                            if (data.Contains("login"))
                            {
                                data = data.Replace("login,", "");
                                string username = data.Split(",")[0].Split("=")[1];
                                string password = data.Split(",")[1].Split("=")[1];

                                if (this.IsUserExist(username, password, "ExtremLinkDB.mdf"))
                                {
                                    this.SendMessage(this.clientTcpSocket, "!", "Exist");
                                    User.UserInstance.UserName = username;
                                    this.respond = "Exist";
                                }
                                else
                                {
                                    this.SendMessage(this.clientTcpSocket, "!", "NotExist");
                                    this.respond = "NotExist";
                                }
                            }

                            else if (data.Contains("signup"))
                            {
                                data = data.Replace("signup,", "");
                                string[] parametersArr = data.Split(",");
                                for (int i = 0; i < 7; i++)
                                {
                                    parametersArr[i] = parametersArr[i].Split("=")[1];
                                }

                                if (this.AddUser(parametersArr, "ExtremLinkDB.mdf"))
                                {
                                    this.SendMessage(this.clientTcpSocket, "!", "SuccessfullyAdded");
                                }
                                else
                                {
                                    this.SendMessage(this.clientTcpSocket, "!", "NotAdded");
                                }
                            }
                            break;
                    }
                }
            }
        }
        // Compress and decompress functions:
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


        // The send and get message functions
        public void SendMessage(Socket clientSocket, string typeOfMessage, string data)
        {
            // The function gets a socket and 2 strings: 'typeOfMessage' which represents the type of the message
            // and 'data' which contains the data which have to be transferred.
            // The function creates a message in a byte array format and sends it to the client.
            // The message format: Byte{typeOfMessage(string), dataLength(int), data(string), "EOM"}
            string endOfMessage = "EOM";
            string message = $"{typeOfMessage}|{data.Length}|{data}|{endOfMessage}";
            byte[] messageBytes;

            // Only compress if it's TCP
            if (clientSocket.ProtocolType == ProtocolType.Tcp)
            {
                messageBytes = this.Compress(message);
                clientSocket.Send(messageBytes);
            }
            else
            {
                // For UDP, send uncompressed data
                messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
                clientSocket.SendTo(messageBytes, new IPEndPoint(IPAddress.Parse(this.clientIpAddress), 1847));
            }
        }
        public List<object> GetMessage(Socket clientSocket)
        {
            // The function gets a socket.
            // The function receives a message from the socket and returns the message in parts as a list object.
            byte[] buffer = new byte[4096];
            int bytesRead;
            EndPoint remoteEndPoint = null;

            if (clientSocket.ProtocolType == ProtocolType.Udp)
            {
                // For UDP, we need to use remoteEndPoint to receive data
                remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                bytesRead = clientSocket.ReceiveFrom(buffer, ref remoteEndPoint);
            }
            else
            {
                // For TCP, use regular Receive
                bytesRead = clientSocket.Receive(buffer);
            }

            // Process received data
            byte[] actualData = new byte[bytesRead];
            Array.Copy(buffer, actualData, bytesRead);

            try
            {
                string decodedMessage;
                if (clientSocket.ProtocolType == ProtocolType.Tcp)
                {
                    // Decompress TCP messages
                    decodedMessage = this.Decompress(actualData);
                }
                else
                {
                    // Don't decompress UDP messages
                    decodedMessage = System.Text.Encoding.UTF8.GetString(actualData);
                }

                string[] messageParts = decodedMessage.Split('|');

                // Validate message format
                if (messageParts.Length != 4)
                {
                    throw new Exception("Error: The message isn't in the right format!");
                }
                if (messageParts[3] != "EOM")
                {
                    throw new Exception("End of message not received correctly. The message is cut.");
                }

                // Create return list
                List<object> messagePartsList = new List<object>
                {
                    messageParts[0],                    // Message type
                    int.Parse(messageParts[1]),         // Data length
                    messageParts[2],                    // Data content
                    messageParts[3]                     // End of message marker
                };

                return messagePartsList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing message: {ex.Message}");
            }
        }


        public BitmapImage GetImageOfPNGFile(string fileName)
        {
            // Load the PNG file as a BitmapImage
            BitmapImage bitmapImage = new BitmapImage();
            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Load the data into memory immediately
                    bitmapImage.StreamSource = stream; // Use the FileStream as the source
                    bitmapImage.EndInit();
                    bitmapImage.Freeze(); // Make the BitmapImage thread-safe
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading frame: {ex.Message}");
            }

            return bitmapImage;
        }

        // Getting frame function

        public BitmapImage GetFrame()
        {
            lock (fileLock)
            {
                try
                {
                    // Store received packets
                    Dictionary<int, byte[]> receivedPackets = new Dictionary<int, byte[]>();
                    int streamId = -1;
                    int totalPackets = -1;

                    // Receive packets
                    while (receivedPackets.Count < totalPackets || totalPackets == -1)
                    {
                        // Buffer size for each packet
                        byte[] buffer = new byte[1412]; // 1400 + 12 bytes for metadata
                        int bytesRead = this.udpSocket.Receive(buffer);

                        // Extract metadata
                        int receivedStreamId = BitConverter.ToInt32(buffer, 0);
                        int receivedTotalPackets = BitConverter.ToInt32(buffer, 4);
                        int packetIndex = BitConverter.ToInt32(buffer, 8);

                        // Ensure packets are part of the same stream
                        if (streamId == -1) streamId = receivedStreamId;
                        if (totalPackets == -1) totalPackets = receivedTotalPackets;
                        if (streamId != receivedStreamId) continue;

                        // Extract segment data
                        byte[] segmentData = new byte[bytesRead - 12];
                        Array.Copy(buffer, 12, segmentData, 0, bytesRead - 12);

                        // Store segment if not already received
                        if (!receivedPackets.ContainsKey(packetIndex))
                        {
                            receivedPackets[packetIndex] = segmentData;
                        }
                    }

                    // Reassemble the file content
                    using (var fileStream = new FileStream("tempFrame.png", FileMode.Create))
                    {
                        for (int i = 0; i < totalPackets; i++)
                        {
                            if (receivedPackets.TryGetValue(i, out var segment))
                            {
                                fileStream.Write(segment, 0, segment.Length);
                            }
                            else
                            {
                                throw new Exception($"Missing packet {i}");
                            }
                        }
                    }

                    // Load the PNG file to a BitmapImage object
                    var bitmap = new BitmapImage();
                    using (var stream = new FileStream("tempFrame.png", FileMode.Open, FileAccess.Read))
                    {
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.None;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();
                    }
                    bitmap.Freeze();
                    return bitmap;

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                    return null;
                }
            }
        }


        // SQL database queries functions
        public static SqlConnection ConnectToDB(string fileName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // Navigate up from bin/Debug to the project root
            string projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
            string dbPath = Path.Combine(projectRoot, "Databases", fileName);
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }
        public bool IsUserExist(string username, string password, string databaseFileName)
        {
            string sqlQuery = "SELECT * FROM [dbo].[Table] WHERE username = @username AND password = @password";
            bool found = false;
            using (SqlConnection conn = ConnectToDB(databaseFileName))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand com = new SqlCommand(sqlQuery, conn))
                    {
                        // Add parameters to the command to avoid SQL injection
                        com.Parameters.AddWithValue("@username", username);
                        com.Parameters.AddWithValue("@password", password);

                        object result = com.ExecuteScalar();
                        int count = result == null ? 0 : Convert.ToInt32(result);
                        // If count is greater than 0, the user exists
                        found = (count > 0);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection failed: " + ex.Message);
                }
            }
            // MessageBox.Show(Convert.ToString(found));
            return found;
        }
        public bool AddUser(string[] parameters, string databaseFileName)
        {
            // The function gets an array of parameters.
            // The function returns true if the user isn't in the database and creates
            // a new user according to the paramters, otherwise return false.
            if (this.IsUserExist(parameters[4], parameters[5], "ExtremLinkDB.mdf")) {return false;}
            string amoutOfUserSqlQuery = "SELECT COUNT(*) FROM [dbo].[Table]";
            string addUserSqlQuery = "INSERT INTO [dbo].[Table] (Id, firstName, lastName, city, phone, username, password, email) VALUES (@userID, @firstname, @lastname, @city, @phone, @username, @password, @email)";
            int amountOfUsers = 0;
            using (SqlConnection conn = ConnectToDB(databaseFileName))
            {
                try
                {
                    conn.Open();
                    // Retrieve the amout of users in the database
                    using (SqlCommand com1 = new SqlCommand(amoutOfUserSqlQuery, conn))
                    {
                        amountOfUsers = (int)com1.ExecuteScalar();
                    }
                    using (SqlCommand com2 = new SqlCommand(addUserSqlQuery, conn))
                    {
                        // Add parameters to the command to avoid SQL injection
                        com2.Parameters.AddWithValue("@userID", amountOfUsers+1);
                        com2.Parameters.AddWithValue("@firstname", parameters[0]);
                        com2.Parameters.AddWithValue("@lastname", parameters[1]);
                        com2.Parameters.AddWithValue("@city", parameters[2]);
                        com2.Parameters.AddWithValue("@phone", parameters[3]);
                        com2.Parameters.AddWithValue("@username", parameters[4]);
                        com2.Parameters.AddWithValue("@password", parameters[5]);
                        com2.Parameters.AddWithValue("@email", parameters[6]);
                        com2.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection failed: " + ex.Message);
                }
            }
            return true;
        }

    }
}
