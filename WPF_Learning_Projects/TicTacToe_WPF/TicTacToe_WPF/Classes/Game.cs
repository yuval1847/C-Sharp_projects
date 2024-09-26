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

        public void SetWinningLine(string direction, string? hAlingment, string? vAlingment)
        {
            // The function gets string of the line direction("Horizontal", "Vertical", "CrossTopBottom", "CrossBottomTop")
            // and additional 2 strings which reprenent the image alignment according to the winning position on the board.
            // The function set the image of the winning line.
            if (direction == "CrossTopBottom" || direction == "CrossBottomTop")
            {
                this.WinningLine.Source = new BitmapImage(new Uri($"pack://application:,,,/TicTacToe_WPF;component/Images/line{direction}.png"));
            }
            else if (direction == "Horizontal") {
                // vertical aligment
                this.WinningLine.Source = new BitmapImage(new Uri($"pack://application:,,,/TicTacToe_WPF;component/Images/line{direction+hAlingment}.png"));
                
            }
            else if (direction == "Vertical"){
                // horizontal aligment
                this.WinningLine.Source = new BitmapImage(new Uri($"pack://application:,,,/TicTacToe_WPF;component/Images/line{direction+vAlingment}.png"));
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
                this.SetWinningLine("Horizontal", "Top", "Center");
                return PlayerX;
            }
            // O
            else if (this.BoardImages[0, 0].Source.ToString().Contains("o.png") && this.BoardImages[0, 1].Source.ToString().Contains("o.png") && this.BoardImages[0, 2].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("Horizontal", "Top", "Center");
                return PlayerO;
            }

            // Second row
            // X
            else if (this.BoardImages[1, 0].Source.ToString().Contains("x.png") && this.BoardImages[1, 1].Source.ToString().Contains("x.png") && this.BoardImages[1, 2].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("Horizontal", "Center", "Center");
                return PlayerX;
            }
            // O
            else if (this.BoardImages[1, 0].Source.ToString().Contains("o.png") && this.BoardImages[1, 1].Source.ToString().Contains("o.png") && this.BoardImages[1, 2].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("Horizontal", "Center", "Center");
                return PlayerO;
            }

            // Third row
            // X
            else if (this.BoardImages[2, 0].Source.ToString().Contains("x.png") && this.BoardImages[2, 1].Source.ToString().Contains("x.png") && this.BoardImages[2, 2].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("Horizontal", "Bottom", "Center");
                return PlayerX;
            }
            // O
            else if (this.BoardImages[2, 0].Source.ToString().Contains("o.png") && this.BoardImages[2, 1].Source.ToString().Contains("o.png") && this.BoardImages[2, 2].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("Horizontal", "Bottom", "Center");
                return PlayerO;
            }


            // Check for winning cols:
            
            // First col
            // X
            else if (this.BoardImages[0, 0].Source.ToString().Contains("x.png") && this.BoardImages[1, 0].Source.ToString().Contains("x.png") && this.BoardImages[2, 0].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("Vertical", "Center", "Left");
                return PlayerX;
            }
            // O
            else if (this.BoardImages[0, 0].Source.ToString().Contains("o.png") && this.BoardImages[1, 0].Source.ToString().Contains("o.png") && this.BoardImages[2, 0].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("Vertical", "Center", "Left");
                return PlayerO;
            }

            // Second col
            // X
            else if (this.BoardImages[0, 1].Source.ToString().Contains("x.png") && this.BoardImages[1, 1].Source.ToString().Contains("x.png") && this.BoardImages[2, 1].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("Vertical", "Center", "Center");
                return PlayerX;
            }
            // O
            else if (this.BoardImages[0, 1].Source.ToString().Contains("o.png") && this.BoardImages[1, 1].Source.ToString().Contains("o.png") && this.BoardImages[2, 1].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("Vertical", "Center", "Center");
                return PlayerO;
            }

            // Third col
            // X
            else if (this.BoardImages[0, 2].Source.ToString().Contains("x.png") && this.BoardImages[1, 2].Source.ToString().Contains("x.png") && this.BoardImages[2, 2].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("Vertical", "Center", "Right");
                return PlayerX;
            }
            // O
            else if (this.BoardImages[0, 2].Source.ToString().Contains("o.png") && this.BoardImages[1, 2].Source.ToString().Contains("o.png") && this.BoardImages[2, 2].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("Vertical", "Center", "Right");
                return PlayerO;
            }


            // Check for winning cross bottom top
            // X
            else if (this.BoardImages[2, 0].Source.ToString().Contains("x.png") && this.BoardImages[1, 1].Source.ToString().Contains("x.png") && this.BoardImages[0, 2].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("CrossBottomTop", null, null);
                return PlayerX;
            }
            // O
            else if (this.BoardImages[2, 0].Source.ToString().Contains("o.png") && this.BoardImages[1, 1].Source.ToString().Contains("o.png") && this.BoardImages[0, 2].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("CrossBottomTop", null, null);
                return PlayerO;
            }


            // Check for winning cross top bottom
            // X
            else if (this.BoardImages[0, 0].Source.ToString().Contains("x.png") && this.BoardImages[1, 1].Source.ToString().Contains("x.png") && this.BoardImages[2, 2].Source.ToString().Contains("x.png"))
            {
                this.SetWinningLine("CrossTopBottom", null, null);
                return PlayerX;
            }
            // O
            else if (this.BoardImages[0, 0].Source.ToString().Contains("o.png") && this.BoardImages[1, 1].Source.ToString().Contains("o.png") && this.BoardImages[2, 2].Source.ToString().Contains("o.png"))
            {
                this.SetWinningLine("CrossTopBottom", null, null);
                return PlayerO;
            }

            return null;
        }
    }
}
