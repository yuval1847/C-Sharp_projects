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
        private Game game;

        public GamePage(ContentControl mainContentControl)
        {
            InitializeComponent();
            this.mainContentControl = mainContentControl;

            Button[,] board = { { TopLeftBtn, TopCenterBtn, TopRightBtn },
                                { CenterLeftBtn, CenterCenterBtn, CenterRightBtn},
                                { BottomLeftBtn, BottomCenterBtn, BottomRightBtn} };
            Image[,] boardImages = { { TopLeftImage, TopCenterImage, TopRightImage },
                                     { CenterLeftImage, CenterCenterImage, CenterRightImage},
                                     { BottomLeftImage, BottomCenterImage, BottomRightImage} };
            game = new Game(new Player("x"), new Player("o"), board, boardImages, winningLine);
        }

        // 3x3 Board
        // Top Buttons
        private void TopLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(TopLeftImage);
            Player? winnerPlayer = this.game.CheckBoardStatus(); 
            if(winnerPlayer != null)
            {

            }
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
           
        }
        private void TopCenterBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(TopCenterImage);
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }
        private void TopRightBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(TopRightImage);
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }

        // Center Buttons
        private void CenterLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(CenterLeftImage);
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }
        private void CenterCenterBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(CenterCenterImage);
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }
        private void CenterRightBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(CenterRightImage);
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }

        // Bottom buttons
        private void BottomLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(BottomLeftImage);
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }
        private void BottomCenterBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(BottomCenterImage);
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }
        private void BottomRightBtn_Click(object sender, RoutedEventArgs e)
        {
            game.ChangeButtonImage(BottomRightImage);
            turnTextBox.Text = $"Turn of player: {this.game.CurrentPlayer.Type}";
        }
    }
}
