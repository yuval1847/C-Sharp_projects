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
        private const int Segment_Size = 4096;

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
                // MessageBox.Show("a udp message was recieved");
                List<object> message = GetMessage(this.udpSocket);
                string data = (string)message[2];
                // MessageBox.Show("a frame was recieved");
                switch (message[0])
                {
                    case "&":
                        if (data.Contains("frame_received"))
                        {
                            data = data.Substring("frame_received".Length + 1);
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
            if (string.IsNullOrEmpty(compressedData))
            {
                return null;
            }
            try
            {
                // Restore Base64 string to standard format
                compressedData = compressedData.Replace('-', '+').Replace('_', '/');

                // Add padding if needed
                switch (compressedData.Length % 4)
                {
                    case 2: compressedData += "=="; break;
                    case 3: compressedData += "="; break;
                }

                // Convert the Base64 string back to a compressed byte array
                byte[] compressedBytes = Convert.FromBase64String(compressedData);

                using (var compressedStream = new MemoryStream(compressedBytes))
                using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                using (var decompressedStream = new MemoryStream())
                {
                    gzipStream.CopyTo(decompressedStream);
                    decompressedStream.Position = 0;

                    // Create decoder
                    BitmapDecoder decoder = BitmapDecoder.Create(
                        decompressedStream,
                        BitmapCreateOptions.PreservePixelFormat,
                        BitmapCacheOption.OnLoad);

                    if (decoder.Frames.Count == 0)
                        return null;

                    // Get the first frame
                    BitmapSource bitmapSource = decoder.Frames[0];

                    // Create new RenderTargetBitmap
                    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                        bitmapSource.PixelWidth,
                        bitmapSource.PixelHeight,
                        bitmapSource.DpiX,
                        bitmapSource.DpiY,
                        PixelFormats.Pbgra32);

                    // Draw the bitmap
                    DrawingVisual drawingVisual = new DrawingVisual();
                    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                    {
                        drawingContext.DrawImage(bitmapSource,
                            new Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight));
                    }

                    renderTargetBitmap.Render(drawingVisual);
                    renderTargetBitmap.Freeze(); // Make it thread-safe
                    return renderTargetBitmap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Decompression error: {ex.Message}");
                return null;
            }
        }


        // The send functions: 
        public void SendMessage(Socket clientSocket, string typeOfMessage, string data)
        {
            // The function gets a socket and 2 strings: 'typeOfMessage' which represents the type of the message
            // and 'data' which contains the data which have to be transferred.
            // The function creates a message in a byte array format and sends it to the client.
            // The message format: Byte{typeOfMessage(string), dataLength(int), data(string), "EOM"}

            // If the data requires segmentation due to it's length
            if (data.Length > Segment_Size)
            {
                this.SendMessage(clientSocket, typeOfMessage, this.SplitStringToSegments(data));
                return;
            }

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
        private void SendMessage(Socket clientSocket, string typeOfMessage, IList<string> data)
        {
            // The function gets the parameters of the original SendMessage function 
            // but the data itself divided into parts.
            // The function call the sending function with all the string one by one.
            foreach (var message in data)
            {
                this.SendMessage(clientSocket, typeOfMessage, message);
            }
        }
        public IList<string> SplitStringToSegments(string s)
        {
            // The function gets a string.
            // The function returns an Ilist of sub-strings of the given string
            // after spliting them ny the size of 4096.
            string tempString = "";
            IList<string> segments = new List<string>();
            for (int i = 0; i < (s.Length / Segment_Size) + 1; i++)
            {
                if (s.Length < Segment_Size)
                {
                    segments.Add(tempString);
                }
                else
                {
                    tempString = s.Substring(0, Segment_Size);
                    segments.Add(tempString);
                    s = s.Replace(tempString, "");
                }
            }
            return segments;
        }

        // The get functions:
        public List<object> GetMessage(Socket clientSocket)
        {
            string completeMessage = "";
            bool isComplete = false;
            List<object> finalMessageParts = null;
            EndPoint remoteEndPoint = null;

            while (!isComplete)
            {
                // Receive raw data
                byte[] buffer = new byte[4096];
                int bytesRead;

                try
                {
                    if (clientSocket.ProtocolType == ProtocolType.Udp)
                    {
                        remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        bytesRead = clientSocket.ReceiveFrom(buffer, ref remoteEndPoint);
                    }
                    else
                    {
                        bytesRead = clientSocket.Receive(buffer);
                    }

                    byte[] actualData = new byte[bytesRead];
                    Array.Copy(buffer, actualData, bytesRead);

                    // Decode message
                    string decodedMessage;
                    if (clientSocket.ProtocolType == ProtocolType.Tcp)
                    {
                        decodedMessage = this.Decompress(actualData);
                    }
                    else
                    {
                        decodedMessage = System.Text.Encoding.UTF8.GetString(actualData);
                    }

                    // Process message parts
                    (isComplete, finalMessageParts) = ProcessMessageParts(decodedMessage, ref completeMessage);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error processing message: {ex.Message}");
                }
            }

            // Add remote endpoint for UDP connections
            if (clientSocket.ProtocolType == ProtocolType.Udp)
            {
                finalMessageParts.Add(remoteEndPoint);
            }
            MessageBox.Show(finalMessageParts[finalMessageParts.Count-1].ToString());
            return finalMessageParts;
        }
        private (bool isComplete, List<object> messageParts) ProcessMessageParts(string message, ref string completeMessage)
        {
            string[] parts = message.Split('|');

            if (parts.Length != 4)
            {
                throw new Exception("Error: The message isn't in the right format!");
            }

            if (parts[3] != "EOM")
            {
                throw new Exception("End of message not received correctly. The message is cut.");
            }

            string messageType = parts[0];
            int dataLength = int.Parse(parts[1]);
            string data = parts[2];
            string endMarker = parts[3];

            // Handle segmented messages
            if (dataLength > Segment_Size)
            {
                completeMessage += data;

                if (completeMessage.Length >= dataLength)
                {
                    return (true, new List<object> { messageType, dataLength, completeMessage, endMarker });
                }
                return (false, null);
            }

            // Handle single messages
            return (true, new List<object> { messageType, dataLength, data, endMarker });
        }
       
        
        // SQL database queries functions
        private static SqlConnection ConnectToDB(string fileName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // Navigate up from bin/Debug to the project root
            string projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
            string dbPath = Path.Combine(projectRoot, "Databases", fileName);
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True";
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