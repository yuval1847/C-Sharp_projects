using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Point = System.Windows.Point;
using System.CodeDom;

namespace ExtremLink_Server.Classes
{
    internal sealed class CustomMouse
    {
        /*
        A class which represent a mouse object.
        The class is designed according to the singleton design pattern.
        */

        private Point cursorsPos;
        private bool isLeftBtnDown;
        private bool isRightBtnDown;

        public Point CursorsPos
        {
            get { return this.cursorsPos; }
            set { this.cursorsPos = value; }
        }

        // Transform the class into a singleton:
        private static CustomMouse customMouseInstance = null;
        public static CustomMouse CustomMouseInstance
        {
            get
            {
                if (customMouseInstance == null)
                {
                    customMouseInstance = new CustomMouse();
                }
                return customMouseInstance;
            }
        }

        // Constractions:
        private CustomMouse()
        {
            this.cursorsPos = new Point(0, 0);
            this.isLeftBtnDown = false;
            this.isRightBtnDown = false;
        }

        // Functions:
        public void ChangePosition(Point newPos)
        {
            // Input: The function gets a new Point object which represent the new coordinate.
            // Output: The function set the current position to the new position.
            this.cursorsPos.X = newPos.X;
            this.cursorsPos.Y = newPos.Y;
        }
        public void ChangePosition(int newXPos, int newYPos)
        {
            // Input: The function gets 2 integers which represent the new coordinate.
            // Output: The function set the current position to the new position.
            this.cursorsPos.X = newXPos;
            this.cursorsPos.Y = newYPos;
        }
        public void PressLeftClick(Server server) 
        {
            // Input: The function gets a server object.
            // Output: The function sends the cient
            this.isLeftBtnDown = true;


        }
        public void SendMousePosition(Server server)
        {
            // Input: The function get a server object.
            // Output: The function sends the current mouse position to the client

            // Json format
            // Note: these coordinates should be scaled up in the client side according to the client's monitor size
            string message = $"{{\"type\":\"mouseMove\",\"x\":{this.CursorsPos.X},\"y\":{this.CursorsPos.Y}}}";
            server.SendMessage(server.ClientTcpSocket, "&", message);
        }
    }
}
