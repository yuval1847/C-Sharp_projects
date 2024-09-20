using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TicTacToe_WPF.Classes
{
    internal class Game
    {
        private Button[,] board;
        private Player[] players;
        private Player currentPlayer;

        public Button[,] Board
        {
            get { return board; }
            set { board = value; }
        }

        public Player[] Players
        {
            get { return players; }
        }

        public Player CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; }
        }


        public Game(Button[,] board, Player[] players)
        {
            this.board = board;
            this.players = players;

            if (this.players[0].Type == "x"){ currentPlayer = this.players[0]; }
            else { currentPlayer = this.players[1]; }

        }


    }
}
