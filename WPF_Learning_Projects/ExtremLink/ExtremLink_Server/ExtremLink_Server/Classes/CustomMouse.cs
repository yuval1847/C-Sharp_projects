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
    enum MouseCommands
    // An enum which different input types of the mouse.
    {
        CommandLess,
        Move,
        LeftPress,
        RightPress
    }


    internal sealed class CustomMouse
    {
        /*
        A class which represents a mouse object.
        The class designed according to the 'Singleton' design pattern.
        */

        // Attributes:
        // A point object which represent the mouse position
        private Point cursorsPos;
        public Point CursorsPos
        {
            get { return this.cursorsPos; }
            set { this.cursorsPos = value; }
        }


        // Singleton behavior:
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


        // Constraction:
        private CustomMouse()
        {
            this.cursorsPos = new Point(0, 0);
        }


        // Functions:

        // Changing position functions:
        public void ChangePosition(Point newPos)
        {
            // Input: A new Point object which represent the new coordinate.
            // Output: The function set the current position to the new position.
            this.cursorsPos.X = newPos.X;
            this.cursorsPos.Y = newPos.Y;
        }
        public void ChangePosition(int newXPos, int newYPos)
        {
            // Input: 2 integers which represent the new coordinate.
            // Output: The function set the current position to the new position.
            this.cursorsPos.X = newXPos;
            this.cursorsPos.Y = newYPos;
        }

        
        // Generating queries functions:
        private string GeneratePositionQuery()
        {
            // Input: nothing.
            // Output: A string which is a query in a json format which represent the mouse coordinates.
            // Note: these coordinates should be scaled up in the client side according to the client's monitor size
            return $"{{\"type\":\"mouseMove\",\"x\":{this.CursorsPos.X},\"y\":{this.CursorsPos.Y}}}";
        }
        private string GenerateLeftPressingQuery()
        {
            // Input: nothing.
            // Output: A string which is a query in a json format which represent a left click of the mouse.
            return $"{{\"type\":\"mouseLeftPress\",\"x\":{this.CursorsPos.X},\"y\":{this.CursorsPos.Y}}}";
        }
        private string GenerateRightPressingQuery()
        {
            // Input: nothing.
            // Output: A string which is a query in a json format which represent a left click of the mouse.
            return $"{{\"type\":\"mouseRightPress\",\"x\":{this.CursorsPos.X},\"y\":{this.CursorsPos.Y}}}";
        }
        
        
        // Sending queries to server function:
        private void SendCommandQueryToClient(Server server, string commandQuery)
        {
            // Input: The function gets a server object and a string which represent the command.
            // Output: The function sends the command to the client.
            server.SendMessage(server.ClientTcpSocket, "%", commandQuery);
        }


        // Handling mouse commands
        public void SendMouseCommands(Server server, MouseCommands mouseCommand)
        {
            // Input: The function gets a server object and an mouse command.
            // Ouput: The function send the commands to the client.
            string commandQuery = "";
            switch (mouseCommand)
            {
                case MouseCommands.Move:
                    commandQuery = this.GeneratePositionQuery();
                    break;
                case MouseCommands.LeftPress:
                    commandQuery = this.GenerateLeftPressingQuery();
                    break;
                case MouseCommands.RightPress:
                    commandQuery = this.GenerateRightPressingQuery();
                    break;
            }
            if (mouseCommand != MouseCommands.CommandLess)
            {
                this.SendCommandQueryToClient(server, commandQuery);
            }

            // A delay between each command message
            Task.Delay(500).Wait();
        }
    }
}
