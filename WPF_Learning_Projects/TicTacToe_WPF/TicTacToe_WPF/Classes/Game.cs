using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TicTacToe_WPF.Classes
{
    internal class Game
    {
        private Button[,] board;
        private Image[,] boardImages;
        // private Player[] players;
        private Player playerX, playerO;
        private Player currentPlayer;
        private Image winningLine;

        public Button[,] Board
        {
            get { return this.board; }
            set { this.board = value; }
        }
        public Image[,] BoardImages
        {
            get { return this.boardImages; }
            set { this.boardImages = value; }
        }
        //public Player[] Players
        //{
        //    get { return players; }
        //}
        public Player CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; }
        }
        public Image WinningLine
        {
            get { return this.winningLine; }
            set { this.winningLine = value; }
        }
        public Player PlayerX
        {
            get { return this.playerX; }
            set { this.playerX = value; }
        }
        public Player PlayerO
        {
            get { return this.playerO; }
            set { this.playerO = value; }
        }


        public Game(Player playerX, Player playerO, Button[,] board, Image[,] boardImages, Image winningLine)
        {
            this.board = board;
            this.BoardImages = boardImages;
            this.winningLine = winningLine;
            this.playerX = playerX;
            this.playerO = playerO;

            currentPlayer = this.PlayerX; 
        }

        public void SwitchUser()
        {
            // The function gets nothing.
            // The function switches the current user to the next one.
            if (this.CurrentPlayer == this.PlayerX)
            {
                this.CurrentPlayer = this.PlayerO;
            }
            else
            {
                this.CurrentPlayer = this.PlayerX;
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

        public void SetWinningLine(string direction)
        {
            // The function gets string of the line direction("Horizontal", "Vertical", "CrossTopBottom", "CrossBottomTop").
            // The function set the image of the winning line.
            if (direction == "Horizontal" || direction == "Vertical" || direction == "CrossTopBottom" || direction == "CrossBottomTop")
            {
                this.WinningLine.Source = new BitmapImage(new Uri($"pack://application:,,,/TicTacToe_WPF;component/Images/{direction}.png"));
            }
            else
            {
                return;
            }
        }
        public Player? CheckBoardStatus()
        {
            // The function gets nothing.
            // The function returns the winner player object if there is a winner, otherwise null.

            // Check for winning rows:

            // First row
            // X
            if (this.BoardImages[0, 0].Source.ToString().Contains("x.png") && this.BoardImages[0, 1].Source.ToString().Contains("x.png") && this.BoardImages[0, 2].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("Horizontal");
                return PlayerX;
            }
            // O
            if (this.BoardImages[0, 0].Source.ToString().Contains("o.png") && this.BoardImages[0, 1].Source.ToString().Contains("o.png") && this.BoardImages[0, 2].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("Horizontal");
                return PlayerO;
            }

            // Second row
            // X
            if (this.BoardImages[1, 0].Source.ToString().Contains("x.png") && this.BoardImages[1, 1].Source.ToString().Contains("x.png") && this.BoardImages[1, 2].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("Horizontal");
                return PlayerX;
            }
            // O
            if (this.BoardImages[1, 0].Source.ToString().Contains("o.png") && this.BoardImages[1, 1].Source.ToString().Contains("o.png") && this.BoardImages[1, 2].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("Horizontal");
                return PlayerO;
            }

            // Third row
            // X
            if (this.BoardImages[2, 0].Source.ToString().Contains("x.png") && this.BoardImages[2, 1].Source.ToString().Contains("x.png") && this.BoardImages[2, 2].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("Horizontal");
                return PlayerX;
            }
            // O
            if (this.BoardImages[2, 0].Source.ToString().Contains("o.png") && this.BoardImages[2, 1].Source.ToString().Contains("o.png") && this.BoardImages[2, 2].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("Horizontal");
                return PlayerO;
            }


            // Check for winning cols:
            
            // First col
            // X
            if (this.BoardImages[0, 0].Source.ToString().Contains("x.png") && this.BoardImages[1, 0].Source.ToString().Contains("x.png") && this.BoardImages[2, 0].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("Vertical");
                return PlayerX;
            }
            // O
            if (this.BoardImages[0, 0].Source.ToString().Contains("o.png") && this.BoardImages[1, 0].Source.ToString().Contains("o.png") && this.BoardImages[2, 0].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("Vertical");
                return PlayerO;
            }

            // Second col
            // X
            if (this.BoardImages[0, 1].Source.ToString().Contains("x.png") && this.BoardImages[1, 1].Source.ToString().Contains("x.png") && this.BoardImages[2, 1].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("Vertical");
                return PlayerX;
            }
            // O
            if (this.BoardImages[0, 1].Source.ToString().Contains("o.png") && this.BoardImages[1, 1].Source.ToString().Contains("o.png") && this.BoardImages[2, 1].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("Vertical");
                return PlayerO;
            }

            // Third col
            // X
            if (this.BoardImages[0, 2].Source.ToString().Contains("x.png") && this.BoardImages[1, 2].Source.ToString().Contains("x.png") && this.BoardImages[2, 2].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("Vertical");
                return PlayerX;
            }
            // O
            if (this.BoardImages[0, 2].Source.ToString().Contains("o.png") && this.BoardImages[1, 2].Source.ToString().Contains("o.png") && this.BoardImages[2, 2].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("Vertical");
                return PlayerO;
            }


            // Check for winning cross bottom top
            // X
            if (this.BoardImages[2, 0].Source.ToString().Contains("x.png") && this.BoardImages[1, 1].Source.ToString().Contains("x.png") && this.BoardImages[0, 2].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("CrossBottomTop");
                return PlayerX;
            }
            // O
            if (this.BoardImages[2, 0].Source.ToString().Contains("o.png") && this.BoardImages[1, 1].Source.ToString().Contains("o.png") && this.BoardImages[0, 2].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("CrossBottomTop");
                return PlayerO;
            }


            // Check for winning cross top bottom
            // X
            if (this.BoardImages[0, 0].Source.ToString().Contains("x.png") && this.BoardImages[1, 1].Source.ToString().Contains("x.png") && this.BoardImages[2, 2].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("CrossTopBottom");
                return PlayerX;
            }
            // O
            if (this.BoardImages[0, 0].Source.ToString().Contains("o.png") && this.BoardImages[1, 1].Source.ToString().Contains("o.png") && this.BoardImages[2, 2].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("CrossTopBottom");
                return PlayerO;
            }

            return null;
        }
    }
}
