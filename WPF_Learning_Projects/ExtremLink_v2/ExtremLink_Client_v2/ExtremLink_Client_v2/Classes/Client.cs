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

namespace ExtremLink_Client_v2.Classes
{
    public class Client
    {
        // *********************************
        // A class which represent a client.
        // *********************************

        // Attributes:

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

        protected Socket sessionTcpSocket;
        public Socket SessionTcpSocket
        {
            get { return this.sessionTcpSocket; }
            set { this.sessionTcpSocket = value; }
        }

        // A string which represent the ip address of the server:
        protected string serverIpAddr;

        // A string which represent the respond of the server:
        protected string serverRespond;
        public string ServerRespond
        {
            get { return this.serverRespond; }
        }

        // An IPEndPoint Object which contains the server's ip and port:
        protected IPEndPoint serverUdpEndPoint;


        public Client()
        {
            // Note: The server IP address you will change in the implementation of the classes of attacker and victim themself.
            this.tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.sessionTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ChangeSocketBufferSizeLimit(this.udpSocket, 4194304, 4194304);
            this.ChangeSocketBufferSizeLimit(this.sessionTcpSocket, 4194304, 4194304);
        }

        private void ChangeSocketBufferSizeLimit(Socket socket, int RecieveBufferSize=8192, int SendBufferSize=8192)
        {
            // Input: A socket object and 2 integers which rerpesent the size of the recieve and send socket's buffer size limit.
            // Output: The function change the buffer limit of the given socket
            socket.ReceiveBufferSize = RecieveBufferSize;
            socket.SendBufferSize = SendBufferSize;
        }
    }
}