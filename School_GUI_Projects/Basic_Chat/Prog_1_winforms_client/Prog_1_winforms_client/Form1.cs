using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prog_1_winforms_client
{
    public partial class Form1 : Form
    {
        public delegate void deleForm();
        public static deleForm myDelegate;
        public Form1 frmChat;
        ServerData sd;
        public static string password, regInfo; //בשביל הרשמה
        public static string IPAddress, UserName, Port, mess;

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        public string firstCharInfoRes;

        

        //private void btnConnect_Click(object sender, EventArgs e)
        //{

        //}

        private void btnConnect_Click_1(object sender, EventArgs e)
        {
            UserName = "moran";
            IPAddress[] LocalIPs = Dns.GetHostAddresses(Dns.GetHostName());
            IPAddress = Convert.ToString(LocalIPs[LocalIPs.Length - 2]);
            Port = "8002";

            sd.StartServer(UserName, IPAddress, int.Parse(Port));
        }

        public Form1()
        {
            InitializeComponent();
            myDelegate = new deleForm(MyProtocol);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            frmChat = (Form1)this;//הגדרת מצביע לטופס הנוכחי
            sd = new ServerData(frmChat);//יצירת אובייקט חדש של המחלקה

        }

        // Blue
        private void btnSend_Click(object sender, EventArgs e)
        {
            sd.WriteToServer("#" + "B" + UserName + " : " + txtMess.Text + "\n");
        }

        // Red
        private void btnRedSend_Click(object sender, EventArgs e)
        {
            sd.WriteToServer("#" + "R" + UserName + " : " + txtMess.Text + "\n");
        }

        // Yellow
        private void btnYellowSend_Click(object sender, EventArgs e)
        {
            sd.WriteToServer("#" + "Y" + UserName + " : " + txtMess.Text + "\n");
        }

        public void MyProtocol()
        {
            string s;
            firstCharInfoRes = mess.Substring(0);
            switch (firstCharInfoRes[0])
            {// חלק זה יתבצע כל פעם שיתחבר לקוח חדש ואז נמחקת  רשימת המשתמשים שאונליין והיא נבנית מחדש 
                case '@':
                    lstUsers.Items.Clear();
                    firstCharInfoRes = firstCharInfoRes.Substring(1);// מוריד גם @ וגם רווח
                    firstCharInfoRes += " ";
                    while (firstCharInfoRes != " ")


                    {
                        s = firstCharInfoRes.Substring(0, firstCharInfoRes.IndexOf(" "));
                        lstUsers.Items.Add(s);
                        firstCharInfoRes = firstCharInfoRes.Substring(firstCharInfoRes.IndexOf(" ") + 1);
                    }
                    //  if (listUsers.Items.Count == 1)
                    //    lblStatus.Content = "Waiting for another player...";
                    break;

                // חלק זה יתבצע כשאר לקוח מתנתק מהצ'אט ואז רשימת השתמשים תעודכן בלעדיו
                case 'E':
                    s = firstCharInfoRes.Substring(1, firstCharInfoRes.IndexOf("\0") - 1);
                    lstUsers.Items.Remove(s);
                    break;
                // חלק זה מציג את ההודעה לכל המשתמשים בחלון הצאט
                case '#':

                    string tmp = firstCharInfoRes.Substring(1);

                    if (tmp[0] == 'R')
                    {
                        lblInfo.ForeColor = Color.Red;
                    }
                    else if (tmp[0] == 'B')
                    {
                        lblInfo.ForeColor = Color.Blue;
                    }
                    else if (tmp[0] == 'Y')
                    {
                        lblInfo.ForeColor = Color.Yellow;
                    }
                    tmp = tmp.Substring(1);
                    lblInfo.Text += tmp + "\n";
                    break;


            }
        }
    }
}

