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
            get { return img; }
            set { img = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }


        public Player(string type)
        {
            this.type = type;

            // Set the image based on the type
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            if (this.type == "x")
            {
                bitmap.UriSource = new Uri("Images/x.png");
            }
            else
            {
                bitmap.UriSource = new Uri("Images/o.png");
            }
            bitmap.EndInit();
            this.Img.Source = bitmap;
        }

        
    }
}
