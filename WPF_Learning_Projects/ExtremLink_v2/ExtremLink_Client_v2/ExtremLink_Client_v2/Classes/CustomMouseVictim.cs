using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Point = System.Windows.Point;

namespace ExtremLink_Client_v2.Classes
{

    public enum VictimMouseCommands
    // An enum which different input types of the mouse.
    {
        CommandLess,
        Move,
        LeftPress,
        RightPress
    }

    public sealed class CustomMouseVictim
    {
        /*
        A class which represents a mouse object.
        The class designed according to the 'Singleton' design pattern.
        */


        // WIN API dll libraries
        // Importing the mouse_event function from user32.dll
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        // Constants for mouse actions
        private const uint MOUSEEVENTF_MOVE = 0x0001;
        private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x00010;


        // Attributes:
        // A point object which represent the mouse position
        private Point cursorsPos;
        public Point CursorsPos
        {
            get { return this.cursorsPos; }
            set { this.cursorsPos = value; }
        }

        // A MouseCommands enum which represent the current mouse command
        private VictimMouseCommands currentCommand;
        public VictimMouseCommands CurrentCommand
        {
            get { return this.currentCommand; }
            set { this.currentCommand = value; }
        }


        // Singleton behavior:
        private static CustomMouseVictim customMouseInstance = null;
        public static CustomMouseVictim CustomMouseInstance
        {
            get
            {
                if (customMouseInstance == null)
                {
                    customMouseInstance = new CustomMouseVictim();
                }
                return customMouseInstance;
            }
        }


        // Constraction:
        private CustomMouseVictim()
        {
            this.cursorsPos = new Point(0, 0);
            this.currentCommand = VictimMouseCommands.CommandLess;
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
        public void ChangePosition(float newXPos, float newYPos)
        {
            // Input: 2 integers which represent the new coordinate.
            // Output: The function set the current position to the new position.
            this.cursorsPos.X = newXPos;
            this.cursorsPos.Y = newYPos;
            // MessageBox.Show($"{GetSystemMetrics(0)}, {GetSystemMetrics(1)}");
        }


        // Mouse moving functions:
        private void MoveMouseToPosition()
        {
            // Input: nothing.
            // Output: The funcion moves the mouse cruser to the specified position.

            // Convert screen coordinates to absolute coordinates (0 to 65535)
            int absoluteX = (int)(this.cursorsPos.X * 65535 / GetSystemMetrics(0)); // Width
            int absoluteY = (int)(this.cursorsPos.Y * 65535 / GetSystemMetrics(1)); // Height

            // Move the mouse to the specified position
            mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, absoluteX, absoluteY, 0, UIntPtr.Zero);
        }


        // Mouse clicking functions:
        // Left and Right clicking function:
        private void LeftClick()
        {
            // Input: Nothing.
            // Output: The function simulate a left mouse click.
            this.MoveMouseToPosition();
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }
        private void RightClick()
        {
            // Input: Nothing.
            // Output: The function simulate a right mouse click.
            this.MoveMouseToPosition();
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
        }

        // Executing the current mouse command function:
        public void ExecuteCurrentMouseCommand()
        {
            // Input: nothing.
            // Output: The function executes the current mouse funtion.
            switch (this.currentCommand)
            {
                case VictimMouseCommands.Move:
                    this.MoveMouseToPosition();
                    break;
                case VictimMouseCommands.LeftPress: 
                    this.LeftClick();
                    break;
                case VictimMouseCommands.RightPress:
                    this.RightClick();
                    break;
            }
            this.currentCommand = VictimMouseCommands.CommandLess;
        }
    }
}
