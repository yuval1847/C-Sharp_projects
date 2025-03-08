using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ExtremLink_Client.Classes
{
    class Victim : Client
    {
        // *****************************************
        // A class which represent a victim.
        // The class inherits from the client class.
        // *****************************************

        // Attributes:

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


        public void Start()
        {
            // The function gets nothing.
            // The function starts the tasks of the functions which handling with packets.
            Console.WriteLine("Server started on TCP port 1234 and UDP port 1847.");
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

            // Checking if changing position is needed
            if (jsonData.ContainsKey("x") && jsonData.ContainsKey("y"))
            {
                CustomMouseVictim.CustomMouseInstance.ChangePosition((float)data.x, (float)data.y);
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
                this.udpSocket.SendTo(packet, this.serverUdpEndPoint);
            }
        }
    }
}
