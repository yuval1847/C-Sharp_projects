using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ExtremLink_Client_v2.Classes;

namespace ExtremLink_Client_v2.Classes
{
    enum AttackerKeyboardCommands
    {
        CommandLess,
        KeyPress
    }


    internal class CustomKeyboardAttacker
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
            set { this.currentKey = value; }
        }

        // A KeyboardCommands enum parameter type which indicate the current status of the keyboard.
        private AttackerKeyboardCommands currentKeyboardCommand;
        public AttackerKeyboardCommands CurrentKeyboardCommand
        {
            get { return this.currentKeyboardCommand; }
            set { this.currentKeyboardCommand = value; }
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
        private static CustomKeyboardAttacker customKeyboardInstance = null;
        public static CustomKeyboardAttacker CustomKeyboardInstance
        {
            get
            {
                if (customKeyboardInstance == null)
                {
                    customKeyboardInstance = new CustomKeyboardAttacker();
                }
                return customKeyboardInstance;
            }
        }


        // Constractor
        private CustomKeyboardAttacker()
        {
            this.currentKey = Key.None;
            this.currentKeyboardCommand = AttackerKeyboardCommands.CommandLess;
        }


        // Generating queries functions:
        private string GenerateKeyPressQuery()
        {
            // Input: Nothing.
            // Output: A string which is a query in a json format which represent the current pressed key.
            return $"{{\"type\":\"keyPress\",\"PressedKey\":\"{this.currentKey}\"}}";
        }


        // Sending queries to server function:
        private void SendCommandQueryToClient(string commandQuery)
        {
            // Input: The function gets a server object and a string which represent the command.
            // Output: The function sends the command to the client.
            Attacker.AttackerInstance.SendTCPMessageToClient("^", commandQuery);
        }


        // Handling mouse commands
        public void SendKeyboardCommands(Key key)
        {
            // Input: A server object and a key value of the enum Key.
            // Output: The function send the commands to the client.
            this.currentKey = key;
            string commandQuery = this.GenerateKeyPressQuery();
            if (this.currentKeyboardCommand != AttackerKeyboardCommands.CommandLess)
            {
                this.SendCommandQueryToClient(commandQuery);
            }
            this.currentKeyboardCommand = AttackerKeyboardCommands.CommandLess;
            this.currentKey = Key.None;
        }
    }
}
