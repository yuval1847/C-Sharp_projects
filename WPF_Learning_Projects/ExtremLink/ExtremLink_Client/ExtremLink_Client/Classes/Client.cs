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
        // *********************************
        // A class which represent a client.
        // *********************************

        // Attributes:

        // Integer constants which represent the sockets' ports:
        protected readonly int TCP_PORT = 1234;
        protected readonly int UDP_PORT = 1847;

        // The tcp socket:
        protected Socket tcpSocket;
        public Socket TCPSocket
        {
            get { return this.tcpSocket; }
            set { this.tcpSocket = value; }
        }

        // The udp socket:
        protected Socket udpSocket;
        public Socket UDPSocket
        {
            get { return this.udpSocket; }
            set { this.udpSocket = value; }
        }

        // A string which represent the ip address of the server:
        protected string serverIpAddr;

        // A string which represent the respond of the server:
        protected string serverRespond;
        public string ServerRespond
        {
            get { return this.serverRespond; }
        }

        // An Endpoint Object which contains the server's ip and port:
        protected EndPoint serverUdpEndPoint;


        public Client()
        {
            // Note: The server IP address you will change in the implementation of the classes of attacker and victim themself.
            this.tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public void ConnectToServer(string serverIpAddr)
        {
            // Input: A string which represent the server ip address.
            // The function connects the TCP socket to the server.
            this.serverIpAddr = serverIpAddr;
            this.tcpSocket.Connect(this.serverIpAddr, TCP_PORT);
            this.serverUdpEndPoint = new IPEndPoint(IPAddress.Parse(this.serverIpAddr), UDP_PORT);
        }

        /*
        public void Start()
        {
            // The function gets nothing.
            // The function starts the tasks of the functions which handling with packets.
            Console.WriteLine("Server started on TCP port 1234 and UDP port 1847.");
            Task.Run(() => this.HandleUdpCommunication());
            Task.Run(() => this.HandleTcpCommunication());
        }

        
        // Note: After finishing the attacker and victim, delete the handle functions
        protected async Task HandleTcpCommunication()
        {
            // The function gets nothing.
            // The fucntion handle with different type of TCP packets which are sent by the server.
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
        protected async Task HandleUdpCommunication()
        {
            // The function gets nothing.
            // The fucntion handle with different type of TCP packets which are sent by the server.
            // & - frames handling.

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
                    this.customKeyboard.CurrentKeyboardCommand = KeyboardCommands.KeyPress;
                    this.customKeyboard.CurrentKey = (Key)Enum.Parse(typeof(Key), data.PressedKey);
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

        */

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
                    socket.SendTo(buffer, new IPEndPoint(IPAddress.Parse(this.serverIpAddr), 1847));
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
    }
}