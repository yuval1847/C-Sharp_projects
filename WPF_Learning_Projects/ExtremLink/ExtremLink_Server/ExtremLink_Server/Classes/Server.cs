using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Networking modules
using System.Net;
using System.Net.Sockets;

// SQL modules
using System.Data.SqlClient;
using System.Net.Http;
using System.IO;
using System.IO.Compression;
using System.ComponentModel;
using System.Windows.Markup;


namespace ExtremLink_Server.Classes
{
    public class Server
    {
        // A class which represent a server.
        // Attributes:
        private Socket udpSocket;
        private Socket tcpSocket;
        private Socket clientTcpSocket;

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

        public Server()
        {
            // Create UDP socket
            this.udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.udpSocket.Bind(new IPEndPoint(IPAddress.Any, 1847));
            // Create TCP socket
            this.tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.tcpSocket.Bind(new IPEndPoint(IPAddress.Any, 1234));
            this.tcpSocket.Listen(1);
            this.clientTcpSocket = this.tcpSocket.Accept();
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
            // The fucntion handle with different type of TCP packets which are sent by the client.
        }

        private async Task HandleTcpCommunication()
        {
            // The function gets nothing.
            // The fucntion handle with different type of TCP packets which are sent by the client.
            // The types of message are:
            // ! - Database functionality
            // @ -
            // # - 
            List<object> message = GetMessage(tcpSocket);
            string data = (string)message[2];
            switch (message[0])
            {
                case "!":
                    if (data.Contains("username"))
                    {
                        string username = data.Split(",")[0].Split("=")[1];
                        string password = data.Split(",")[1].Split("=")[1];
                        if (this.IsUserExist(username, password, "ExtremLinkDB.mdf"))
                        {
                            this.SendMessage(this.clientTcpSocket, "!", "Exist");
                        }
                        else
                        {
                            this.SendMessage(this.clientTcpSocket, "!", "NotExist");
                        }
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
            for(int i = 0; i < messageParts.Length; i++)
            {
                if(i == 1)
                {
                    messagePartsList.Add(int.Parse(messageParts[i]));
                }
                messagePartsList.Add(messageParts[i]);
            }
            return messagePartsList;
        }


        // SQL database queries functions
        public static SqlConnection ConnectToDB(string fileName)
        {
            // The function gets a string which represent the file name of the database.
            // The fucntion returns a SqlConnection object which connected to the database.

            // The path of the database
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=" + path + "; Integrated Security=True; Connect Timeout=30";
            SqlConnection conn = new SqlConnection(connString);
            return conn;
        }
        public bool IsUserExist(string username, string password, string databaseFileName)
        {
            // The function gets 2 strings which represent a username and a password.
            // The function sends a query to the database and returns true if the user exist, otherwise false.
            string sqlQuery = $"SELECT * FROM [dbo].[Table] WHERE username='{username}' AND password='{password}'";
            SqlConnection conn = ConnectToDB(databaseFileName);
            conn.Open();
            SqlCommand com = new SqlCommand(sqlQuery, conn);
            SqlDataReader data = com.ExecuteReader();
            bool found = (bool)data.Read();
            conn.Close();
            return found;
        }
    }
}
