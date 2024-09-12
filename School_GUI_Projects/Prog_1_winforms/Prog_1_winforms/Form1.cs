using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Prog_1_winforms
{
    public partial class Form1 : Form
    {
        
        private string IpAddress; 
        private TcpListener tcpLsn; 
        private Thread tcpThd;
        ClientConnect c1;
        List<ClientConnect> allClients = new List<ClientConnect>();
        private static int connectId = 0; // משתנה ששומר את מספר הלקוחות שהתחברו ואת מספר הלקוח 
        string NameOfUsers, str;
        
        public Form1()
        {
            InitializeComponent();
        }
        
        private void GetIpAdDress() // את כתובת המחשב IpAddress פונקציה שתשמור במשתנה IpAdress את הכתובת
        {
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            IpAddress = Convert.ToString(localIP[localIP.Length - 2]);
            // IpAddress ="127.0.0.1";        
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            GetIpAdDress();
            tcpLsn = new TcpListener(System.Net.IPAddress.Parse(IpAddress), 8002);//  יצירת מאזין- 
            tcpLsn.Start();// מפעילה את המאזין
            lblInfo.Text = "Listen at: " + tcpLsn.LocalEndpoint.ToString();// מציג את הכתובת והפורט
            c1 = new ClientConnect();
            tcpThd = new Thread(new ThreadStart(NewClientConnected));// יצירת תהליך שפועל ברקע   
            tcpThd.Start();
        }


        public void NewClientConnected() //זו פונקציה שאנו נרשום אשר פעולותיה מתבצעות ברקע של התוכנית 
        {
            //פונקציה המפעילה תהליך חדש
            //היא בעלת לולאה אינסופית(זהו תהליך ותהליך לא נגמר אף פעם) המטפלת בהתחברות של הלקוח החדש
            Socket s;
            //s = tcpLsn.AcceptSocket();
            string strmess;

            while (true)
            {
                try
                {
                    s = tcpLsn.AcceptSocket();
                    c1 = new ClientConnect();
                    c1.clientSocket = s;
                    c1.clientThread = new Thread(new ThreadStart(ReadSocket)); //Threadשומרת את ה
                    int ret = 0;
                    Byte[] receive = new Byte[10]; //ברשת נתונים עוברים רק בביטים
                    ret = s.Receive(receive, receive.Length, 0); //מחזיק את כמות הבייטים שיתקבלו
                    strmess = System.Text.Encoding.UTF8.GetString(receive); //strmess שורה זו ממירה את המערך שהוא מטיפוס בייט למחרוזת שאותה הוא שומר ב
                    c1.name = strmess.Substring(0, ret); //שורה זו מחלצת את מה שרשום עד לרווח ה1
                    Interlocked.Increment(ref connectId);
                    c1.clientnum = connectId;
                    UpDateDataGrid(connectId + " : " + strmess.Substring(0, ret) + "\r\n");

                    lock (this)
                    {
                        allClients.Add(c1); //את שעת ההתחברות textboxמעדכנת את הטבלה ומוסיפה ל 
                        UpDateDataGrid(" Connected> " + connectId + " " + DateTime.Now.ToLongTimeString() + "\r\n");
                        c1.clientThread.Start();
                    }
                }
                catch (Exception e)
                {
                    break;
                }
            }
        }



        public void UpDateDataGrid(string displayString)//אם תוך כדי תהליכים נצטרך לעדכן תיבת טקסט או כפתורים על הטופס נעשה אינבואוק
        {
            if (txtData.InvokeRequired)
                txtData.Invoke(new MethodInvoker(() => txtData.AppendText(displayString + "\n")));


        }

        public void ReadSocket()// תהליך של כול לקוח
        {

            long realId = c1.clientnum; // The realId saves the real number of the client that sends the info
            Socket s = c1.clientSocket;
            int ret = 0; // This object will contain the number of characters that are passed in the message
            Byte[] receive; // In this array I'll save the info from the client
            receive = new Byte[2000]; // If the client is connected we reboot the array;
            while (true) // מנהל המשחק: כרגע מקבלת ממל מי כול לקוח ומוסרת אותו לכול הלקוחות
            {
                try
                {
                    if (s.Connected)
                    {
                        ret = s.Receive(receive, receive.Length, 0);//s.Receive is a command that gets the info from the client and put it in the array receive
                        if (ret > 0) // If a message is rececived
                        {
                            foreach (ClientConnect c in allClients)
                            {
                                if (c.clientSocket.Connected)
                                    c.clientSocket.Send(receive, receive.Length, SocketFlags.None);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }// If a message was received and its' characters length is 0 we get out of the loop
                }
                catch (Exception e) // If an error occured we want to stop the thread
                {
                    UpDateDataGrid(e.ToString()); // Show on the screen in the txtbox the error that occured
                    if (!s.Connected) break;// If the client is not connected we get out of the loop
                }
            }


            CloseTheThread(realId);// We'll get to this line only if an error occured, because we only get out of the loop if there was an error
                                   //that's why this function will only be summaned if an error occured
        }
        private void CloseTheThread(long realId)
        {
            //This function closes the thread - the process of the client that caused the error
            try
            {
                for (int i = 0; i < allClients.Count; i++)
                {
                    if (allClients[i].clientnum == realId)
                        allClients[i].clientThread.Abort();
                }
            }

            catch (Exception e)
            {
                lock (this)
                {
                    for (int i = 0; i < allClients.Count; i++)
                    {
                        if (allClients[i].clientnum == realId)
                            allClients.Remove(allClients[i]);
                    }

                    UpDateDataGrid("Disconnected>" + realId + " " + DateTime.Now.ToLongTimeString());
                }
            }
        }
        private void OnClosing() // זוהי פונקציה שמחסלת תהליכים היא סוגרת את הthread ואת המאזין
        {
            if (tcpLsn != null)
            { tcpLsn.Stop(); }

            foreach (ClientConnect cd in allClients)
            {
                if (cd.clientSocket.Connected) cd.clientSocket.Close();
                if (cd.clientThread.IsAlive) cd.clientThread.Abort();
            }


            if (tcpThd.IsAlive) tcpThd.Abort();

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnClosing();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnClosing();
        }

    }
}
