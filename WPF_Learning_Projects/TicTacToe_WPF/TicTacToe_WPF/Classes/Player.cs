using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace TicTacToe_WPF.Classes
{
    internal class Player
    {
        private Image img;
        private string type;

        public Image Img
        {
            get { return this.img; }
            set { this.img = value; }
        }

        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }


        public Player(string type)
        {
            this.type = type;
            this.img = new Image();

            // Set the image based on the type
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            if (this.type == "x")
            {
                bitmap.UriSource = new Uri("pack://application:,,,/TicTacToe_WPF;component/Images/x.png");
            }
            else
            {
                bitmap.UriSource = new Uri("pack://application:,,,/TicTacToe_WPF;component/Images/o.png");
            }
            bitmap.EndInit();
            this.Img.Source = bitmap;
        }

        
    }
}
