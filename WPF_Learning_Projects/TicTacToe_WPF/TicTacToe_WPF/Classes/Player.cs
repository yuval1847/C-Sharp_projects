using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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


        public Player(Image img, string type)
        {
            this.img = img;
            this.type = type;
        }



    }
}
