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

        public void SwitchUser()
        {
            // The function gets nothing.
            // The function switches the current user to the next one.
            if(this.CurrentPlayer == this.players[0])
            {
                this.CurrentPlayer = this.players[1];
            }
            else
            {
                this.CurrentPlayer = this.players[0];
            }
        }

        public void ChangeButtonImage(Image btnImg)
        {
            // The function gets a button.
            // The function switch the button's image to the player's sign if it isn't filled already.
            if (btnImg.Source.ToString().Contains("empty_cell.png"))
            {
                btnImg.Source = new BitmapImage(new Uri($"pack://application:,,,/TicTacToe_WPF;component/Images/{this.CurrentPlayer.Type}.png"));
                this.SwitchUser();
                
            }
        }
    }
}
