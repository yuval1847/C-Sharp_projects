using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Net;

namespace ExtremLink_Client.Classes
{
    public class Client
    {
        // A class which represent a client.
        // Attributes:
        private Socket udpSocket;
        private Socket tcpSocket;
        private string serverIpAddr;
        private string serverRespond;

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
        public string ServerRespond
        {
            get { return this.serverRespond; }
            set 
            {
                this.serverRespond = value;
            }
        }


        public Client(string serverIpAddr)
        {
            this.serverIpAddr = serverIpAddr;
            this.udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
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
            // & - frames handling.

        }

        private async Task HandleTcpCommunication()
        {
            // The function gets nothing.
            // The fucntion handle with different type of TCP packets which are sent by the server.
            // The types of message are:
            // ! - Database functionality
            // @ -
            // # -
            while (true)
            {
                lock (this)
                {
                    List<object> message = GetMessage(this.tcpSocket);
                    string data = (string)message[2];
                    
                    switch (message[0])
                    {
                        case "!":
                            if (data == "Exist"){this.serverRespond = "Exist";}
                            else if(data == "NotExist"){this.serverRespond = "NotExist";}
                            else if(data == "SuccessfullyAdded") { this.serverRespond = "SuccessfullyAdded";}
                            else if (data == "NotAdded") { this.serverRespond = "SuccessfullyAdded"; }
                            break;
                        case "&":
                            // MessageBox.Show(data);
                            if(data == "StartSendFrames") { this.serverRespond = "StartSendFrames"; }
                            break;
                    }
                    
                }
            }
        }

        // Compress and decompress functions
        // Regular strings:
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
        public string CompressRenderTargetBitmap(RenderTargetBitmap renderTargetBitmap)
        {
            if (renderTargetBitmap == null)
                return string.Empty;

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    // Save the RenderTargetBitmap to the memory stream as PNG
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                    encoder.Save(memoryStream);
                    memoryStream.Position = 0;

                    byte[] imageBytes = memoryStream.ToArray();

                    // Compress the byte array using GZip
                    using (var compressedStream = new MemoryStream())
                    {
                        using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress, true))
                        {
                            gzipStream.Write(imageBytes, 0, imageBytes.Length);
                        }

                        // Get the compressed bytes and ensure proper Base64 encoding
                        byte[] compressedBytes = compressedStream.ToArray();
                        string base64String = Convert.ToBase64String(compressedBytes);

                        // Clean the string to ensure it's valid Base64
                        base64String = base64String.TrimEnd('=');
                        base64String = base64String.Replace('+', '-').Replace('/', '_');

                        return base64String;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Compression error: {ex.Message}");
                return string.Empty;
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
                clientSocket.SendTo(messageBytes, new IPEndPoint(IPAddress.Parse(this.serverIpAddr), 1847));
            }
        }
        public List<object> GetMessage(Socket clientSocket)
        {
            // The function gets a socket.
            // The function receives a message from the socket and returns the message in parts as a list object.
            byte[] buffer = new byte[65536];
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

                if (clientSocket.ProtocolType == ProtocolType.Udp)
                {
                    // For UDP, also store the sender's endpoint
                    messagePartsList.Add(remoteEndPoint);
                }

                return messagePartsList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing message: {ex.Message}");
            }
        }
    }
}