﻿using System;
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


namespace ExtremLink_Server.Classes
{
    internal class Server
    {
        /* A class which represent a server */
        private int port;
        private TcpListener listener;
        private string databaseFileName;

        public int Port { get { return this.port; }}
        public string DatabaseFileName { get { return this.databaseFileName; }}

        public Server(int port)
        {
            this.port = port;
            this.databaseFileName = "ExtremLinkDB.mdf";
            listener = new TcpListener(IPAddress.Any, port);
            
        }


        // Basic server managment commands
        public void Start()
        {
            // The function gets nothing.
            // The function starts the server communication with the client.
            this.listener.Start();
            TcpClient client = this.listener.AcceptTcpClient();
            this.listener.Stop();
            NetworkStream stream = client.GetStream();
            while (true)
            {
                this.HandleWithMessages(stream);
            }
            this.Stop(client);
        }

        public void Stop(TcpClient client)
        {
            // The function gets a client object.
            // The function close the socket with the client and stop the listener.
            client.Close();
        }

        // ************************************************************************
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

        public List<object> GetMsg(NetworkStream stream)
        {
            // The function gets a message from the client(not by calling).
            // The function returns the message in parts as a list object.
            byte[] buffer = new byte[4096];  // Buffer to store incoming data
            int bytesRead = stream.Read(buffer, 0, buffer.Length);  // Read from the stream
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
                List<object> messageComponents = new List<object>{typeOfMessage, dataLength, data};
                return messageComponents;
            }
            else
            {
                throw new IOException("No data received.");
            }
        }
        public void SendMsg(NetworkStream stream, string typeOfMessage, string data)
        {
            // The function gets 2 strings: 'typeOfMessage' which is a symbol which reprsent the type of the message
            // and 'data' which contains the data which have to be transfered.
            // The function creates a message in a byte array format and send it to the client.
            // The message format: Byte{typeOfMessage(string), dataLength(int), data(string), "EOM"}
            string endOfMessage = "EOM";
            string message = $"{typeOfMessage}|{data.Length}|{data}|{endOfMessage}";
            byte[] compressedMessage = this.Compress(message);
            stream.Write(compressedMessage, 0, compressedMessage.Length);
        }

        public void HandleWithMessages(NetworkStream stream)
        {
            // The function get a message in a List<object> formet.
            // The function handle with the message according to the message type according t this protocol:
            // ! - Database functionality
            // @ -
            // # - 
            List<object> message = this.GetMsg(stream);
            string data = (string)message[2];
            switch (message[0])
            {
                case "!":
                    if (data.Contains("username"))
                    {
                        string username = data.Split(",")[0].Split("=")[1];
                        string password = data.Split(",")[1].Split("=")[1];
                        if (this.IsUserExist(username, password))
                        {
                            // Complete if the user exist in the database.
                            this.SendMsg(stream, "!", "Exist");
                        }
                        else
                        {
                            // Complete if the user doesn't exist in the database.
                            this.SendMsg(stream, "!", "NotExist");
                        }
                    }
                    break;
            }
        }

        // SQL database queries functions
        public static SqlConnection ConnectToDB(string fileName)
        {
            // The function gets a string which represent a the file name of the database.
            // The fucntion returns a SqlConnection object which connected to the database.

            // The path of the database
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=" + path + "; Integrated Security=True; Connect Timeout=30";
            SqlConnection conn = new SqlConnection(connString);
            return conn;
        }

        public bool IsUserExist(string username, string password)
        {
            // The function gets 2 strings which represent a username and a password.
            // The function sends a query to the database and returns true if the user exist, otherwise false.
            string sqlQuery = $"SELECT * FROM [dbo].[Table] WHERE username='{username}' AND password='{password}'";
            SqlConnection conn = ConnectToDB(this.databaseFileName);
            conn.Open();
            SqlCommand com = new SqlCommand(sqlQuery, conn);
            SqlDataReader data = com.ExecuteReader();
            bool found = (bool)data.Read();
            conn.Close();
            return found;
        }
    }
}
