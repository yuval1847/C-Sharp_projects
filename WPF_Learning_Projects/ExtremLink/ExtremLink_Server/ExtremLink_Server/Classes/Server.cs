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

        private Server()
        {
            this.respond = "";
            this.serverIpAddress = this.FindIpAddress();
        }

        // Connecting functions:
        private string FindIpAddress()
        {
            // The function gets nothing.
            // The function sets the server's ip address according to the local ip address.
            IPAddress[] localIpsAddr = Dns.GetHostAddresses(Dns.GetHostName());
            return Convert.ToString(localIpsAddr[localIpsAddr.Length - 1]);
        }
        public void ConnectToClients()
        {
            // Input: Nothing.
            // Output: The function wait for clients (both attacker and victims) to connect to the server.
            Client temp1 = new Client(this.serverIpAddress);
            temp1.ConnectToClient();
            List<object> ruleMessage = temp1.GetTCPMessageFromClient();
            switch (ruleMessage[2])
            {
                case "attacker":
                    this.attacker = temp1;
                    this.victim = new Client(this.serverIpAddress);
                    this.victim.ConnectToClient();
                    break;
                case "victim":
                    this.victim = temp1;
                    this.attacker = new Client(this.serverIpAddress);
                    this.attacker.ConnectToClient();
                    break;
            }
        }


        // A function which starts the messages handlers
        public void Start()
        {
            // The function gets nothing.
            // The function starts the tasks of the functions which handling with packets.
            Task.Run(() => this.HandleUdpCommunication());
            Task.Run(() => this.HandleAttackerTcpCommunication());
            Task.Run(() => this.HandleVictimTcpCommunication());
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

        private async Task HandleAttackerTcpCommunication()
        {
            // The function gets nothing.
            // The fucntion handle with different type of TCP packets which are sent by the attacker client.
            // The types of message are:
            // ~ - Rule choosing
            // ! - Database functionality
            // & - Frames handling
            // % - Mouse handling
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
                        case "&":
                            break;
                    }
                }
            }
        }
        
        
        // Handling clients' users managment messages function:       
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
                        break;
                    case TypeOfClient.victim:
                        this.victim.SendTCPMessageToClient("!", "Exist");
                        this.victim.User.UserName = username;
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
