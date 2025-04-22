using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Net.Mail;
using SendGrid;

namespace ExtremLink_Client_v2.Classes
{
    public static class EmailsManager
    {
        /*
        A class which manage the emails of the programs
        */

        // Attributes:
        private static string apiKey = "";


        // Functions:
        public static async Task SendEmail(string receiver, string subject, string msg)
        {
            // A function which sends emails messages.
        }
    }
}
