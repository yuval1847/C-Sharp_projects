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
        private EndPoint serverEndPoint;

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
            this.serverEndPoint = new IPEndPoint(IPAddress.Parse(this.serverIpAddr), 1847);
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
                            if (data == "Exist") { this.serverRespond = "Exist"; }
                            else if (data == "NotExist") { this.serverRespond = "NotExist"; }
                            else if (data == "SuccessfullyAdded") { this.serverRespond = "SuccessfullyAdded"; }
                            else if (data == "NotAdded") { this.serverRespond = "SuccessfullyAdded"; }
                            break;
                        case "&":
                            // MessageBox.Show(data);
                            if (data == "StartSendFrames") { this.serverRespond = "StartSendFrames"; }
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
                clientSocket.SendTo(messageBytes, this.serverEndPoint);
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


        // Sending frame functions
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
        private void ClearBuffer(ref byte[] buffer)
        {
            // The function gets a byte array object.
            // The function clears its content.
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            Array.Clear(buffer, 0, buffer.Length);
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
        private byte[][] ToSegment(byte[] fileBuffer, int segmentSize)
        {
            int totalSegments = (int)Math.Ceiling((double)fileBuffer.Length / segmentSize);
            byte[][] segments = new byte[totalSegments][];

            for (int i = 0; i < totalSegments; i++)
            {
                int currentSegmentSize = Math.Min(segmentSize, fileBuffer.Length - (i * segmentSize));
                segments[i] = new byte[currentSegmentSize];
                Array.Copy(fileBuffer, i * segmentSize, segments[i], 0, currentSegmentSize);
            }

            return segments;
        }

        /*public void SendFrame(RenderTargetBitmap renderTarget)
        {
            // The first message's format is: amount of segments.
            // The other messages' format is: segment + segment's ID.

            byte[] frameData = this.ConvertRenderTargetBitmapToByteArray(renderTarget);
            this.CreatePngImageFile(frameData);
            // Sending the amount of segments
            int totalSegments = (int)Math.Ceiling((double)frameData.Length / SegmentSize); // Calculate the number of segments needed
            byte[] totalSegmentsInByteArray = new byte[] { Convert.ToByte(totalSegments) };
            this.udpSocket.SendTo(totalSegmentsInByteArray, this.serverEndPoint);  // Send the total number of segments

            byte[] segment = new byte[SegmentSize + 4];  // Prepare a buffer to store each segment (size + ID)

            for (int i = 0; i < totalSegments; i++)
            {
                int offset = i * SegmentSize;  // Calculate the offset for each segment
                int length = Math.Min(SegmentSize, frameData.Length - offset);  // Adjust length if it’s the last segment

                // Add the segment ID at the start of the segment buffer
                Array.Copy(BitConverter.GetBytes(i), 0, segment, 0, 4);

                // Add the segment's data starting from the calculated offset
                Array.Copy(frameData, offset, segment, 4, length);

                // Send the segment via UDP
                this.udpSocket.SendTo(segment, this.serverEndPoint);

                this.ClearBuffer(ref segment);  // Clear the buffer for the next segment
            }
        }
        */
        public void SendFrame(RenderTargetBitmap renderTarget)
        {
            // The first message's format is: amount of segments.
            // The other messages' format is: segment + segment's ID.

            // Convert RenderTargetBitmap to byte array
            byte[] frameData = this.ConvertRenderTargetBitmapToByteArray(renderTarget);

            // Create a temporary PNG file for debugging and future use
            this.CreatePngImageFile(frameData);

            // Get the file content
            byte[] fileContent = GetFileContent("tempFrame.png");

            // Send file size
            byte[] fileSizeBytes = BitConverter.GetBytes(fileContent.Length);
            this.udpSocket.SendTo(fileSizeBytes, this.serverEndPoint);

            // Send file content segment by segment
            byte[][] fileSegments = ToSegment(fileContent, 4096);
            foreach (var segment in fileSegments)
            {
                this.udpSocket.SendTo(segment, this.serverEndPoint);
            }
        }
    }
}