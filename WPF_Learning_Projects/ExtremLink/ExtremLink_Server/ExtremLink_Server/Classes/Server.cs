using System;
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

namespace ExtremLink_Server.Classes
{
    public class Server
    {
        // A class which represent a server.
        // Attributes:
        private string serverIpAddress;
        private Socket udpSocket;
        private Socket tcpSocket;
        private Socket clientTcpSocket;
        private List<object> message;
        private string respond;
        private string udpRespond;
        private string clientIpAddress;
        private RenderTargetBitmap currentFrame;

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
        public RenderTargetBitmap CurrentFrame
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
                List<object> message = GetMessage(this.udpSocket);
                string data = (string)message[2];
                switch (message[0])
                {
                    case "&":
                        if (data.Contains("frame_received"))
                        {
                            this.udpRespond = "frame_received";
                            this.currentFrame = this.DecompressRenderTargetBitmap(data);
                        }
                        break;
                }
            }
        }

        private async Task HandleTcpCommunication()
        {
            // The function gets nothing.
            // The fucntion handle with different type of TCP packets which are sent by the client.
            // The types of message are:
            // ! - Database functionality
            // @ -
            // # - 
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
        // Compress and decompress functions
        // Regular string:
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
        // Frames bitmap:
        public RenderTargetBitmap DecompressRenderTargetBitmap(string compressedData)
        {
            // The function takes a Base64 string representing a compressed RenderTargetBitmap
            // and returns the decompressed RenderTargetBitmap object.

            if (string.IsNullOrEmpty(compressedData))
                return null;

            try
            {
                // Convert the Base64 string back to a compressed byte array
                byte[] compressedBytes = Convert.FromBase64String(compressedData);

                // Decompress the byte array
                using (var compressedStream = new MemoryStream(compressedBytes))
                {
                    using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                    {
                        using (var decompressedStream = new MemoryStream())
                        {
                            // Copy decompressed data to the memory stream
                            gzipStream.CopyTo(decompressedStream);

                            // Reset the position of the memory stream
                            decompressedStream.Position = 0;

                            // Decode the decompressed data into a BitmapFrame
                            BitmapDecoder decoder = BitmapDecoder.Create(
                                decompressedStream,
                                BitmapCreateOptions.PreservePixelFormat,
                                BitmapCacheOption.OnLoad);

                            // Convert the BitmapFrame to a RenderTargetBitmap
                            BitmapSource bitmapSource = decoder.Frames[0];

                            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                                bitmapSource.PixelWidth,
                                bitmapSource.PixelHeight,
                                bitmapSource.DpiX,
                                bitmapSource.DpiY,
                                PixelFormats.Pbgra32);

                            // Render the BitmapSource into the RenderTargetBitmap
                            DrawingVisual drawingVisual = new DrawingVisual();
                            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                            {
                                drawingContext.DrawImage(bitmapSource, new System.Windows.Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight));
                            }
                            renderTargetBitmap.Render(drawingVisual);

                            return renderTargetBitmap;
                        }
                    }
                }
            }
            catch
            {
                // Handle exceptions (e.g., invalid data format)
                return null;
            }
        }



        // The send and get message functions
        public void SendMessage(Socket clientSocket, string typeOfMessage, string data)
        {
            // The function gets a socket and  2 strings: 'typeOfMessage' which is a symbol which reprsent the type of the message
            // and 'data' which contains the data which have to be transfered.
            // The function creates a message in a byte array format and sends it to the client.
            // The message format: Byte{typeOfMessage(string), dataLength(int), data(string), "EOM"}
            string endOfMessage = "EOM";
            string message = $"{typeOfMessage}|{data.Length}|{data}|{endOfMessage}";
            byte[] compressedMessage = this.Compress(message);
            if (clientSocket.ProtocolType == ProtocolType.Tcp)
            {
                clientSocket.Send(compressedMessage);
            }
            else
            {
                clientSocket.SendTo(compressedMessage, new IPEndPoint(IPAddress.Parse(this.clientIpAddress), 1847));
            }
        }
        public List<object> GetMessage(Socket clientSocket)
        {
            // The function gets a socket.
            // The function recieve a message from the socket and returns the message in parts as a list object.
            byte[] buffer = new byte[4096];
            int bytesRead = clientSocket.Receive(buffer);
            
            byte[] actualData = new byte[bytesRead];
            Array.Copy(buffer, actualData, bytesRead);

            string[] messageParts = this.Decompress(actualData).Split('|');
            if (messageParts.Length != 4)
            {
                throw new Exception("Error: The message isn't in the right format!");
            }

            if (messageParts[3] != "EOM")
            {
                throw new Exception("End of message not received correctly. The message is cut.");
            }

            List<object> messagePartsList = new List<object>();
            messagePartsList.Add(messageParts[0]);
            messagePartsList.Add(int.Parse(messageParts[1]));
            messagePartsList.Add(messageParts[2]);
            messagePartsList.Add(messageParts[3]);
            return messagePartsList;
        }


        // SQL database queries functions
        public static SqlConnection ConnectToDB(string fileName)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\yuval\Desktop\C# projects\WPF_Projects\ExtremLink\ExtremLink_Server\ExtremLink_Server\Databases\"+fileName+";Integrated Security=True";
            //string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="+Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExtremLink_Server", "Databases", fileName)+";Integrated Security=True";
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
