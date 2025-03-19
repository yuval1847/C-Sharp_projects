using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using OpenCvSharp;
using System.Drawing;

namespace ExtremLink_Client.Classes
{
    class Attacker : Client
    {
        // *****************************************
        // A class which represent an attacker.
        // The class inherits from the client class.
        // *****************************************

        // Attributes:

        // Integer constants which represent the sockets' ports:
        public readonly int ATTACKER_TCP_PORT = 1234;
        public readonly int ATTACKER_UDP_PORT = 1235;

        // A string which represent the victim's IP address:
        private string victimIpAddr;
        public string VictimIpAddr
        {
            get { return this.victimIpAddr; }
        }

        // A BitmapImage object which represent the current frame:
        private BitmapImage currentFrame;
        public BitmapImage CurrentFrame
        {
            get { return this.currentFrame; }
        }

        // Singelton behavior:
        private static Attacker attackerInstance = null;
        public static Attacker AttackerInstance
        {
            get
            {
                if (attackerInstance == null)
                {
                    attackerInstance = new Attacker();
                }
                return attackerInstance;
            }
        }

        private string tempPngFileName;
        public string TempPngFileName
        {
            get { return this.tempPngFileName; }
        }


        // Constructor:
        private Attacker() : base()
        {

        }

        public void ConnectToServer(string serverIpAddr)
        {
            // Input: A string which represent the server ip address.
            // The function connects the TCP socket to the server.
            this.serverIpAddr = serverIpAddr;
            this.tcpSocket.Connect(new IPEndPoint(IPAddress.Parse(this.serverIpAddr), this.ATTACKER_TCP_PORT));
            this.udpSocket.Bind(new IPEndPoint(IPAddress.Parse(this.serverIpAddr), this.ATTACKER_UDP_PORT));
        }

        public void Start()
        {
            // The function gets nothing.
            // The function starts the tasks of the functions which handling with packets.
            Task.Run(() => this.HandleUdpCommunication());
            Task.Run(() => this.HandleTcpCommunication());
        }


        // Sending basic messages functions:
        private void SendMessageToClient(string typeOfMessage, string message, Socket socket)
        {
            // Input: A string which represent the message and the socket.
            // Output: The function sends the message via the given socket.
            byte[] buffer = Encoding.UTF8.GetBytes($"{typeOfMessage}|{message.Length}|{message}|EOM"); // EOM - "end of message"

            // Only compress if it's TCP
            switch (socket.ProtocolType)
            {
                case ProtocolType.Tcp:
                    socket.Send(buffer);
                    break;
                case ProtocolType.Udp:
                    socket.SendTo(buffer, new IPEndPoint(IPAddress.Parse(this.serverIpAddr), this.ATTACKER_UDP_PORT));
                    break;
            }
        }
        public void SendTCPMessageToClient(string typeOfMessage, string message)
        {
            // Input: A string which represent the message.
            // Output: The function sends the message via the tcp socket.
            this.SendMessageToClient(typeOfMessage, message, this.tcpSocket);
        }
        public void SendUDPMessageToClient(string typeOfMessage, string message)
        {
            // Input: A string which represent the message.
            // Output: The function sends the message via the tcp socket.
            this.SendMessageToClient(typeOfMessage, message, this.udpSocket);
        }


        // Getting basic messages functions:
        private List<object> OrderMessage(string message)
        {
            // The function gets a string which represent a message received from the client.
            // The fucntion returns the message as a list object which contains the message by it's parameters.
            string[] messageParts = message.Split('|');

            // Validate message format
            if (messageParts.Length != 4)
            {
                throw new Exception("The message isn't in the right format!");
            }
            if (messageParts[3] != "EOM")
            {
                throw new Exception("End of message wasn't fully received");
            }

            List<object> messagePartsList = new List<object>
                {
                    messageParts[0],                    // Message type
                    int.Parse(messageParts[1]),         // Data length
                    messageParts[2],                    // Data content
                    messageParts[3]                     // End of message marker
                };

            return messagePartsList;
        }
        public List<object> GetTCPMessageFromClient()
        {
            // Input: Nothing.
            // Output: The received message from the client via he tcp socket as a list of parameters.
            byte[] buffer = new byte[4096];
            int receivedBytes = this.tcpSocket.Receive(buffer);
            return this.OrderMessage(Encoding.UTF8.GetString(buffer, 0, receivedBytes));
        }
        public List<object> GetUDPMessageFromClient()
        {
            // Input: Nothing.
            // Output: The received message from the client via the udp socket as a list of parameters.
            byte[] buffer = new byte[4096];
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            int receivedBytes = this.udpSocket.ReceiveFrom(buffer, ref remoteEndPoint);
            return this.OrderMessage(Encoding.UTF8.GetString(buffer, 0, receivedBytes));
        }



        // Sockets handlers:
        // Tcp handler function:
        private async Task HandleTcpCommunication()
        {
            // Input: Nothing.
            // Output: The function handles with different types of messages over the tcp socket.
            // The types of message are:
            // G - General stuff handling
            // ! - Database functionality
            // & - Frames handling
            // % - Mouse handling
            // $ - Sessions handling
            while (true)
            {
                List<object> message = this.GetTCPMessageFromClient();
                string data = (string)message[2];
                switch (message[0])
                {
                    case "G":
                        this.HandleGeneralStuffMessages(data);
                        break;
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
                    case "$":
                        this.HandleSessionsCommands(data);
                        break;
                }
                
            }
        }

        // Udp handler function:
        private async Task HandleUdpCommunication()
        {
            // Input: Nothing.
            // Output: The function handles with different types of messages over the udp socket.
            // & - Frames handling.
            while (true)
            {
                byte[] currentFrameByteArr = this.GetFrame();
                this.currentFrame = this.ConvertByteArrayToBitmapImage(currentFrameByteArr);
                this.InsertBitmapImageToPngFile(this.currentFrame);
                Thread.Sleep(1000);
            }
        }


        // Sub-Handlers:
        // Handle general stuff messages:
        private void HandleGeneralStuffMessages(string data)
        {
            // Input: A string which represent a given general stuff message from the server.
            // Output: The function handle with the given message.
            dynamic message = JsonConvert.DeserializeObject(data);
            JObject jsonData = (JObject)message;
            if (jsonData.ContainsKey("victim"))
            {
                this.victimIpAddr = message.victim;
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
            // Output: The function

        }

        // Handle Input commands:
        private void HandleMouseInput(string message)
        {
            // Input: string object which represent the message that was given from the server.
            // Ouput: The function 

        }
        private void HandleKeyboardInput(string message)
        {
            // Input: string which represent a keyboard commands query message.
            // Ouput: The function 
            
        }

        // Handle Sessions commands:
        private void HandleSessionsCommands(string message)
        {
            // Input: A string which represent the server message.
            // Output: The function handles which the sessions message.

            // Reading the data in json format
            dynamic data = JsonConvert.DeserializeObject(message);

            // Casting the data dynamic object to JObject
            JObject jsonData = (JObject)data;

        }



        // Getting frames functions:
        public byte[] GetFrame()
        {
            // Input: Nothing.
            // Output: The function gets a frame from the victim as a byte array in h265 format.
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
            using (var ms = new MemoryStream())
            {
                for (int i = 0; i < totalPackets; i++)
                {
                    if (receivedPackets.TryGetValue(i, out var segment))
                    {
                        ms.Write(segment, 0, segment.Length);
                    }
                    else
                    {
                        throw new Exception($"Missing packet {i}");
                    }
                }
                return ms.ToArray();
            }
        }
        public BitmapImage ConvertByteArrayToBitmapImage(byte[] h265Bytes)
        {
            try
            {
                // Write the byte array to a temporary file since VideoCapture needs a file or stream
                string tempFile = Path.GetTempFileName() + ".h265";
                File.WriteAllBytes(tempFile, h265Bytes);

                // Open the video file using VideoCapture
                using (var videoCapture = new VideoCapture(tempFile))
                {
                    if (!videoCapture.IsOpened())
                        throw new Exception("Could not open video file");

                    // Read the first frame
                    using (var frame = new Mat())
                    {
                        if (!videoCapture.Read(frame) || frame.Empty())
                            throw new Exception("Could not read frame from video");

                        // Convert the frame (BGR) to RGB
                        using (var rgbFrame = new Mat())
                        {
                            Cv2.CvtColor(frame, rgbFrame, ColorConversionCodes.BGR2RGB);

                            // Convert Mat to byte array
                            byte[] imageData = rgbFrame.ToBytes(".bmp"); // Use BMP format for simplicity

                            // Create BitmapImage from byte array
                            BitmapImage bitmap = new BitmapImage();
                            using (var ms = new MemoryStream(imageData))
                            {
                                bitmap.BeginInit();
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.StreamSource = ms;
                                bitmap.EndInit();
                                bitmap.Freeze(); // Make it usable across threads
                            }

                            // Clean up temporary file
                            File.Delete(tempFile);

                            return bitmap;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error converting H.265 to BitmapImage: {ex.Message}");
            }
        }
        public string InsertBitmapImageToPngFile(BitmapImage frame)
        {
            // Input: A BitmapImage object which contains an image.
            // Output: The function creates a png file, inserts the given BitmapImage to the png file and returns it name.
            if (frame == null)
                throw new ArgumentNullException(nameof(frame), "BitmapImage cannot be null");

            // Create a PNG encoder
            PngBitmapEncoder encoder = new PngBitmapEncoder();

            // Add the BitmapImage to the encoder
            encoder.Frames.Add(BitmapFrame.Create(frame));

            // Generate a unique file name with timestamp
            string fileName = $"tempFrame.png";
            string filePath = Path.Combine(Path.GetTempPath(), fileName);

            try
            {
                // Save the encoded image to a file
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(fs);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to create PNG file from BitmapImage: {ex.Message}", ex);
            }
        }
        public BitmapImage GetBitmapImageFromPNGFile(string pngFileName)
        {
            // Input: A string which represent the path of a png file.
            // Output: A BitmapImage which represent the image of the given png file.
            if (string.IsNullOrEmpty(pngFileName))
                throw new ArgumentException("PNG file name cannot be null or empty", nameof(pngFileName));

            if (!File.Exists(pngFileName))
                throw new FileNotFoundException($"The specified PNG file does not exist: {pngFileName}", pngFileName);

            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(pngFileName, UriKind.Absolute); // Use absolute path
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // Load immediately into memory
                bitmap.EndInit();
                bitmap.Freeze(); // Make it immutable and thread-safe

                return bitmap;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load PNG file into BitmapImage: {ex.Message}", ex);
            }
        }
    }
}
