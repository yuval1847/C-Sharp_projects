using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TicTacToe_WPF.Classes
{
    internal class Game
    {
        private Player[] players;
        private Player currentPlayer;

        public Player[] Players
        {
            get { return players; }
        }
        public Player CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; }
        }


        public Game(Player[] players)
        {
            this.players = players;

            if (this.players[0].Type == "x"){ currentPlayer = this.players[0]; }
            else { currentPlayer = this.players[1]; }
        }

        public void ChangeButtonImage(Button btn)
        {
            Image? buttonImage = btn.Content as Image;
            if (buttonImage.Source.Equals(new BitmapImage(new Uri("/Images/empty_cell.png", UriKind.Relative)))){
                if (CurrentPlayer.Type == "x")
                {
                    buttonImage.Source = new BitmapImage(new Uri("/Images/x.png", UriKind.Relative));
                }
                else
                {
                    buttonImage.Source = new BitmapImage(new Uri("/Images/o.png", UriKind.Relative));
                }
            }
        }
    }
}
