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

namespace ExtremLink_Client.Classes
{
    class Attacker : Client
    {
        // *****************************************
        // A class which represent an attacker.
        // The class inherits from the client class.
        // *****************************************

        // Attributes:

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


        // Constructor:
        private Attacker() : base()
        {

        }

        public void Start()
        {
            // The function gets nothing.
            // The function starts the tasks of the functions which handling with packets.
            Task.Run(() => this.HandleUdpCommunication());
            Task.Run(() => this.HandleTcpCommunication());
        }


        // Sockets handlers:
        // Tcp handler function:
        private async Task HandleTcpCommunication()
        {
            // Input: Nothing.
            // Output: The function handles with different types of messages over the tcp socket.
            // The types of message are:
            // ! - Database functionality
            // & - Frames handling
            // % - Mouse handling
            // $ - Sessions handling
            while (true)
            {
                lock (this)
                {
                    List<object> message = this.GetTCPMessageFromClient();
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
                        case "$":
                            this.HandleSessionsCommands(data);
                            break;
                    }
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
                this.currentFrame = this.GetFrame();
                Thread.Sleep(1000);
            }
        }


        // Sub-Handlers:
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

    }
}
