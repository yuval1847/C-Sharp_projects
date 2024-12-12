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
using System.Security.Cryptography.X509Certificates;

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
        public string CompressString(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(inputBytes, 0, inputBytes.Length);
                }
                return Convert.ToBase64String(outputStream.ToArray());
            }
        }
        public string DecompressString(string compressedInput)
        {
            if (string.IsNullOrEmpty(compressedInput))
                throw new ArgumentNullException(nameof(compressedInput));

            byte[] compressedBytes = Convert.FromBase64String(compressedInput);
            using (MemoryStream inputStream = new MemoryStream(compressedBytes))
            {
                using (GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        gzipStream.CopyTo(outputStream);
                        return Encoding.UTF8.GetString(outputStream.ToArray());
                    }
                }
            }
        }
        // Frames bitmap:
        /*public string CompressRenderTargetBitmapToString(RenderTargetBitmap renderTargetBitmap, int qualityPercentage)
        {
            // The function gets a RenderTargetBitmap object and a quality presentages as an integer.
            // The function compress it and returns it as a string.
            if (renderTargetBitmap == null)
                throw new ArgumentNullException(nameof(renderTargetBitmap));
            if (qualityPercentage < 1 || qualityPercentage > 100)
                throw new ArgumentOutOfRangeException(nameof(qualityPercentage), "Quality percentage must be between 1 and 100.");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Use JpegBitmapEncoder to encode the RenderTargetBitmap with quality settings
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.QualityLevel = qualityPercentage;
                encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                encoder.Save(memoryStream);

                // Convert the byte array to a Base64 string
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }*/
        // Create a function that gets a RenderTargetImage save it to a png file and return the file buff:
        public string ConvertRenderTargetBitmapToString(RenderTargetBitmap renderTargetBitmap, int qualityPercentage, string filePath)
        {
            // The function gets a RenderTargetBitmap object, and integet that represent the quality of the image.
            // The function creates a PNG file that store the image and returns the file's buff as a string.
            if (renderTargetBitmap == null)
                throw new ArgumentNullException(nameof(renderTargetBitmap), "RenderTargetBitmap cannot be null.");
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            if (qualityPercentage < 1 || qualityPercentage > 100)
                throw new ArgumentOutOfRangeException(nameof(qualityPercentage), "Quality must be between 1 and 100.");

            // Create a PNG encoder
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            // Save the image to the specified file
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                encoder.Save(fileStream);
            }

            // Read the saved file into a buffer and convert it to a Base64 string
            byte[] buffer = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(buffer);
        }
        public byte[] GetFileBuff(string filePath) {
            // The function gets a file path.
            // The function returns the file buff.
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            // Check if the file exists
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The specified file does not exist.", filePath);

            // Read and return the file buffer
            return File.ReadAllBytes(filePath);
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
                // A test for analysing the length of the message.
                MessageBox.Show($"The total length of message: {messageBytes.Length}");
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

        public void SendFrame(RenderTargetBitmap frame)
        {
            // The function gets a RednerTargetBitmap object which represent a frame.
            // The function sends the frame to the server.
            try
            {
                // Convert the RenderTargetBitmap to a byte array
                byte[] frameBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    // Encode the RenderTargetBitmap to a PNG format
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(frame));
                    encoder.Save(ms);
                    frameBytes = ms.ToArray();
                }
                // Send the frame bytes to the server
                this.udpSocket.SendTo(frameBytes, new IPEndPoint(IPAddress.Parse(this.serverIpAddr), 1847));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending the frame: {ex.Message}");
            }
        }
    }
}