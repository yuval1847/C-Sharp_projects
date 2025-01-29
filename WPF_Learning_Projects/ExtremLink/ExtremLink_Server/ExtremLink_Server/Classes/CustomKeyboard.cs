using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace ExtremLink_Server.Classes
{
    enum KeyboardCommands
    {
        CommandLess,
        KeyPress
    }


    internal class CustomKeyboard
    {
        /*
        A class which represents a keyboard object.
        The class designed according to the 'Singleton' design pattern.
        */

        // Attributes:
        // A key type parameter which represent a key that was pressed on the keyboard.
        private Key currentKey;
        public Key CurrentKey
        {
            get { return this.currentKey; }
        }


        // Event for key press & release
        public event Action<Key> KeyPressed;
        public event Action<Key> KeyReleased;

        // Windows API dll libraries
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private LowLevelKeyboardProc proc;
        private IntPtr hookID = IntPtr.Zero;

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;


        // Singleton behavior
        private static CustomKeyboard customKeyboardInstance = null;
        public static CustomKeyboard CustomKeyboardInstance
        {
            get
            {
                if (customKeyboardInstance == null)
                {
                    customKeyboardInstance = new CustomKeyboard();
                }
                return customKeyboardInstance;
            }
        }


        // Constractor
        private CustomKeyboard()
        {

        }

        
        // Changing current command function
        private void ChangeCurrentKey(Key key)
        {
            // Input: A key in a type of Key enum as a parameter .
            // Ouput: The function changes the current key to the given key.
            this.currentKey = key;
        }


        // Generating queries functions:
        private string GenerateKeyPressQuery()
        {
            // Input: Nothing.
            // Output: A string which is a query in a json format which represent the current pressed key.
            return $"{{\"type\":\"keyPress\",\"PressedKey\":{this.currentKey}}}";
        }


        // Sending queries to server function:
        private void SendCommandQueryToClient(Server server, string commandQuery)
        {
            // Input: The function gets a server object and a string which represent the command.
            // Output: The function sends the command to the client.
            server.SendMessage(server.ClientTcpSocket, "^", commandQuery);
        }


        // Handling mouse commands
        private void SendKeyboardCommands(Server server, KeyboardCommands keyboardCommand)
        {
            // Input: Nothing.
            // Output: The function send the commands to the client.
            string commandQuery = "";
            switch (keyboardCommand)
            {
                case KeyboardCommands.KeyPress:
                    commandQuery = this.GenerateKeyPressQuery();
                    break;
            }
            if (keyboardCommand != KeyboardCommands.CommandLess)
            {
                this.SendCommandQueryToClient(server, commandQuery);
            }
        }

    }
}
