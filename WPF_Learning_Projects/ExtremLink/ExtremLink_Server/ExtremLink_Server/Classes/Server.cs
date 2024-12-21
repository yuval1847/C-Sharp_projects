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
using System.Net.NetworkInformation;

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
        private RenderTargetBitmap currentFrame;
        private EndPoint clientEndPoint;
        private const int SegmentSize = 8192;

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
                // MessageBox.Show("a udp message was recieved");
                this.currentFrame = this.GetFrame();
                // MessageBox.Show("a frame was recieved");
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


        // Getting frame functions
        /* private void ClearBuffer(ref byte[] buffer)
        {
            // The function gets a byte array object.
            // The function clears its content.
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            Array.Clear(buffer, 0, buffer.Length);
        }
        private int ExtractSegmentID(ref byte[] buffer)
        {
            if (buffer == null || buffer.Length < 4)
                throw new ArgumentException("Buffer must have at least 4 bytes.");

            // Extract the last 4 bytes (segment ID)
            byte[] last4Bytes = new byte[4];
            Array.Copy(buffer, buffer.Length - 4, last4Bytes, 0, 4);

            // Resize the original buffer to remove the last 4 bytes (segment ID)
            byte[] dataWithoutID = new byte[buffer.Length - 4];
            Array.Copy(buffer, 0, dataWithoutID, 0, buffer.Length - 4);
            buffer = dataWithoutID; // Update the buffer without the segment ID

            return BitConverter.ToInt32(last4Bytes, 0);
        }
        private byte[] JoinByteArrays(byte[][] byteArrays)
        {
            // Calculate the total length of the combined array
            int totalLength = 0;
            foreach (var byteArray in byteArrays)
            {
                totalLength += byteArray.Length;
            }

            // Create a new array to hold all the bytes
            byte[] result = new byte[totalLength];

            // Copy each byte array into the result array
            int offset = 0;
            foreach (var byteArray in byteArrays)
            {
                Array.Copy(byteArray, 0, result, offset, byteArray.Length);
                offset += byteArray.Length;
            }

            return result;
        }
        private RenderTargetBitmap ConvertByteArrayToRenderTargetBitmap(byte[] byteArray)
        {
            // Create a MemoryStream from the byte array
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                // Create a BitmapImage from the MemoryStream
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                // Create a RenderTargetBitmap from the BitmapImage
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                    bitmapImage.PixelWidth,
                    bitmapImage.PixelHeight,
                    bitmapImage.DpiX,
                    bitmapImage.DpiY,
                    bitmapImage.Format);

                // Draw the BitmapImage onto the RenderTargetBitmap
                renderTargetBitmap.Render(new System.Windows.Controls.Image { Source = bitmapImage });

                return renderTargetBitmap;
            }
        }
        private void CreatePngImageFile(byte[] fileContent)
        {
            string fileName = "tempFrame.png";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            try
            {
                // Write the byte array to a file
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(fileContent, 0, fileContent.Length);
                }

                MessageBox.Show($"File created successfully at: {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while creating the PNG file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public RenderTargetBitmap GetFrame()
        {
            // The first message's format is: the amount of segments.
            // The other messages are: segment + segment's ID.

            byte[] buff = new byte[SegmentSize + 4];  // Buffer for receiving segments
            byte[] amountBuffer = new byte[1];        // Buffer for receiving the number of segments (1 byte)

            // Receive the first message: number of segments
            this.udpSocket.Receive(amountBuffer);
            int amountOfSegments = amountBuffer[0]; // Extract the amount of segments from the first byte
            MessageBox.Show($"Number of segments: {amountOfSegments}");

            Dictionary<int, byte[]> segmentsDict = new Dictionary<int, byte[]>();

            for (int i = 0; i < amountOfSegments; i++)
            {
                this.ClearBuffer(ref buff);
                this.udpSocket.Receive(buff);  // Receive the next segment

                // Extract the segment ID (first 4 bytes)
                int currentID = BitConverter.ToInt32(buff, 0);
                byte[] segmentData = buff.Skip(4).Take(SegmentSize).ToArray();  // Extract the actual segment data

                // Debugging information
                MessageBox.Show($"Received segment with ID: {currentID}, Size: {segmentData.Length}");

                segmentsDict.Add(currentID, segmentData);  // Store the segment in the dictionary
            }
            MessageBox.Show("Finished getting segments");
            // Sorting the segments dictionary by their ID
            var sortedDictionary = segmentsDict.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            // Combine the byte arrays of the sorted segments into a single byte array
            var segmentsArray = this.JoinByteArrays(sortedDictionary.Values.ToArray());

            // Creating a PNG file of the image
            this.CreatePngImageFile(segmentsArray);

            // Return the RenderTargetBitmap object after converting it from the byte array
            return this.ConvertByteArrayToRenderTargetBitmap(segmentsArray);
        }*/

        private byte[] GetFileContent(string fileName)
        {
            try
            {
                // Read all bytes of the file into a byte array
                byte[] fileContent = File.ReadAllBytes(fileName);
                return fileContent;
            }
            catch (Exception ex)
            {
                // Handle exceptions such as file not found or permission issues
                throw new IOException($"Failed to read file content: {ex.Message}", ex);
            }
        }
        private RenderTargetBitmap GetRenderTargetBitmapFromPng(byte[] fileContent)
        {
            try
            {
                if (fileContent == null || fileContent.Length == 0)
                    throw new ArgumentException("File content is null or empty.");

                // Load the PNG data into a BitmapImage
                BitmapImage bitmapImage = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(fileContent))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Load immediately to avoid memory issues
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze(); // Make it thread-safe
                }

                // Create a RenderTargetBitmap with the same size as the BitmapImage
                int pixelWidth = bitmapImage.PixelWidth;
                int pixelHeight = bitmapImage.PixelHeight;
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(pixelWidth, pixelHeight, 96, 96, PixelFormats.Pbgra32);

                // Draw the BitmapImage into the RenderTargetBitmap
                var visual = new DrawingVisual();
                using (var drawingContext = visual.RenderOpen())
                {
                    drawingContext.DrawImage(bitmapImage, new System.Windows.Rect(0, 0, pixelWidth, pixelHeight));
                }
                renderTargetBitmap.Render(visual);

                return renderTargetBitmap;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create RenderTargetBitmap: {ex.Message}", ex);
            }
        }
        public RenderTargetBitmap GetFrame()
        {
            // The first message's format is: the amount of segments.
            // The other messages are: segment + segment's ID.

            // Receive file size
            byte[] fileSizeBuffer = new byte[8];
            this.udpSocket.Receive(fileSizeBuffer);
            long fileSize = BitConverter.ToInt64(fileSizeBuffer, 0);
            MessageBox.Show($"File size: {fileSize} bytes");

            // Receive file content
            using (var fileStream = new FileStream("tempFrame.png", FileMode.Create))
            {
                byte[] buffer = new byte[4096];
                long totalBytesReceived = 0;
                while (totalBytesReceived < fileSize)
                {
                    int bytesToRead = (int)Math.Min(buffer.Length, fileSize - totalBytesReceived);
                    int bytesRead = this.udpSocket.Receive(buffer, 0, bytesToRead, SocketFlags.None);
                    fileStream.Write(buffer, 0, bytesRead);
                    totalBytesReceived += bytesRead;
                }
            }
            MessageBox.Show("The file was save!");
            byte[] fileContent = GetFileContent("tempFrame.png");
            return GetRenderTargetBitmapFromPng(fileContent);
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
