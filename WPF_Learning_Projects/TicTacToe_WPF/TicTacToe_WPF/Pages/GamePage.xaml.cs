using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using TicTacToe_WPF.Classes;

namespace TicTacToe_WPF.Pages
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : UserControl
    {
        private ContentControl mainContentControl;
        private object communicatorSide; // Server or Client
        private Game game;

        public GamePage(ContentControl mainContentControl, object communicatorSide)
        {
            InitializeComponent();
            this.mainContentControl = mainContentControl;
            this.communicatorSide = communicatorSide;

            Button[,] board = { { TopLeftBtn, TopCenterBtn, TopRightBtn },
                                { CenterLeftBtn, CenterCenterBtn, CenterRightBtn},
                                { BottomLeftBtn, BottomCenterBtn, BottomRightBtn} };
            Image[,] boardImages = { { TopLeftImage, TopCenterImage, TopRightImage },
                                     { CenterLeftImage, CenterCenterImage, CenterRightImage},
                                     { BottomLeftImage, BottomCenterImage, BottomRightImage} };
            game = new Game(new Player("x"), new Player("o"), board, boardImages, winningLine);
        }
        public void Pause(int milliseconds)
        {
            // The function gets integers which represent the milliseconds amount.
            // The function create a delay for the given time period.
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(milliseconds);
            timer.Tick += (sender, args) =>
            {
                timer.Stop(); // Stop the timer once the time is up
            };

            timer.Start();

            // Block until the timer stops (which pauses the execution)
            while (timer.IsEnabled)
            {
                // This keeps the UI responsive while blocking the code execution
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            }
        }
        // 3x3 Board
        // Top Buttons
        private void TopLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(TopLeftImage);
            string? winnerPlayerType = this.game.CheckBoardStatus(); 
            if(winnerPlayerType != null)
            {
                this.Pause(3000);
                this.mainContentControl.Content = new WinnerDeclerationPage(winnerPlayerType);
            }
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
           
        }
        private void TopCenterBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(TopCenterImage);
            string? winnerPlayerType = this.game.CheckBoardStatus();
            if (winnerPlayerType != null)
            {
                this.Pause(3000);
                this.mainContentControl.Content = new WinnerDeclerationPage(winnerPlayerType);
            }
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }
        private void TopRightBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(TopRightImage);
            string? winnerPlayerType = this.game.CheckBoardStatus();
            if (winnerPlayerType != null)
            {
                this.Pause(3000);
                this.mainContentControl.Content = new WinnerDeclerationPage(winnerPlayerType);
            }
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }

        // Center Buttons
        private void CenterLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(CenterLeftImage);
            string? winnerPlayerType = this.game.CheckBoardStatus();
            if (winnerPlayerType != null)
            {
                this.Pause(3000);
                this.mainContentControl.Content = new WinnerDeclerationPage(winnerPlayerType);
            }
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }
        private void CenterCenterBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(CenterCenterImage);
            string? winnerPlayerType = this.game.CheckBoardStatus();
            if (winnerPlayerType != null)
            {
                this.Pause(3000);
                this.mainContentControl.Content = new WinnerDeclerationPage(winnerPlayerType);
            }
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }
        private void CenterRightBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(CenterRightImage);
            string? winnerPlayerType = this.game.CheckBoardStatus();
            if (winnerPlayerType != null)
            {
                this.Pause(3000);
                this.mainContentControl.Content = new WinnerDeclerationPage(winnerPlayerType);
            }
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }

        // Bottom buttons
        private void BottomLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(BottomLeftImage);
            string? winnerPlayerType = this.game.CheckBoardStatus();
            if (winnerPlayerType != null)
            {
                this.Pause(3000);
                this.mainContentControl.Content = new WinnerDeclerationPage(winnerPlayerType);
            }
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }
        private void BottomCenterBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(BottomCenterImage);
            string? winnerPlayerType = this.game.CheckBoardStatus();
            if (winnerPlayerType != null)
            {
                this.Pause(3000);
                this.mainContentControl.Content = new WinnerDeclerationPage(winnerPlayerType);
            }
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }
        private void BottomRightBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(BottomRightImage);
            string? winnerPlayerType = this.game.CheckBoardStatus();
            if (winnerPlayerType != null)
            {
                this.Pause(3000);
                this.mainContentControl.Content = new WinnerDeclerationPage(winnerPlayerType);
            }
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }
    }
}
