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
using Newtonsoft.Json;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json.Linq;
using Point = System.Windows.Point;
using System.Windows.Input;

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
        private int mouseX;
        private int mouseY;
        private CustomMouse customMouse = CustomMouse.CustomMouseInstance;
        private CustomKeyboard customKeyboard = CustomKeyboard.CustomKeyboardInstance;
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
        public int MouseX
        {
            get { return this.mouseX; }
            set { this.mouseX = value; }
        }
        public int MouseY
        {
            get { return this.mouseY; }
            set { this.mouseY = value; }
        }


        public Client(string serverIpAddr)
        {
            this.serverIpAddr = serverIpAddr;
            this.udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.mouseX = 0;
            this.mouseY = 0;
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
            // & - Frames handling
            // % - Mouse handling
            while (true)
            {
                lock (this)
                {
                    List<object> message = GetMessage(this.tcpSocket);
                    string data = (string)message[2];

                    switch (message[0])
                    {
                        case "!":
                            this.HandleUsersManagmentCommands(data);
                            break;
                        case "&":
                            this.HandleFramesCommands(data);
                            break;
                        case "%":
                            this.HandleMouseInput(data);
                            break;
                        case "^":
                            this.HandleKeyboardInput(data);
                            break;
                    }
                }
            }
        }

        // Handle with user managment commands:
        private void HandleUsersManagmentCommands(string data)
        {
            // Input: a string which represent a given user managments command from the server.
            // Output: The function handle with the commands.
            switch (data)
            {
                case "Exist":
                    this.serverRespond = "Exist";
                    break;
                case "NotExist":
                    this.serverRespond = "NotExist";
                    break;
                case "SuccessfullyAdded":
                    this.serverRespond = "SuccessfullyAdded";
                    break;
                case "NotAdded":
                    this.serverRespond = "NotAdded";
                    break;
            }
        }
        
        // Handle with frames commands:
        private void HandleFramesCommands(string data)
        {
            // Input: a string which represent a given frames command from the server.
            // Output: The function handles with the frames commands.
            switch (data)
            {
                case "StartSendFrames":
                    this.serverRespond = "StartSendFrames";
                    break;
                case "StopSendFrames":
                    this.serverRespond = "StopSendFrames";
                    break;
                case "PauseSendFrames":
                    this.serverRespond = "PauseSendFrames";
                    break;
            }
        }

        // Handle Input Commands:
        public void HandleMouseInput(string message)
        {
            // Input: string object which represent the message that was given from the server.
            // Ouput: The function update the CustomMouse object according to the given message's parameters.

            // Reading the data in json format
            dynamic data = JsonConvert.DeserializeObject(message);

            // Casting the data dynamic object to JObject
            JObject jsonData = (JObject)data;

            // Checking if changing position is needed
            if (jsonData.ContainsKey("x") && jsonData.ContainsKey("y"))
            {
                this.customMouse.ChangePosition((float)data.x, (float)data.y);         
            }

            // Updating the mouse parameters according to the given message
            switch ((string)data.type)
            {
                case "mouseMove":
                    this.customMouse.CurrentCommand = MouseCommands.Move;
                    break;
                case "mouseLeftPress":
                    this.customMouse.CurrentCommand = MouseCommands.LeftPress;
                    break;
                case "mouseRightPress":
                    this.customMouse.CurrentCommand = MouseCommands.RightPress;
                    break;
            }
        }
        public void HandleKeyboardInput(string message)
        {
            // Input: string which represent a keyboard commands query message.
            // Ouput: The function update the CustomKeyboard object according to the given message's parameters.

            // Reading the data in json format
            dynamic data = JsonConvert.DeserializeObject(message);

            // Casting the data dynamic object to JObject
            JObject jsonData = (JObject)data;

            switch ((string)data.type)
            {
                case "keyPress":
                    this.customKeyboard.CurrentKeyboardCommand = KeyboardCommands.KeyPress;
                    this.customKeyboard.CurrentKey = (Key)Enum.Parse(typeof(Key), data.PressedKey);
                    break;
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

        // The send and get message functions:
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
                this.udpSocket.SendTo(packet, this.serverEndPoint);
            }
        }
        
    }
}