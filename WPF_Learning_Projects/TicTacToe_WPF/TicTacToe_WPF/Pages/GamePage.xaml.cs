﻿using System;
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

            //Button[,] board = { { TopLeftBtn, TopCenterBtn, TopRightBtn },
            //                    { CenterLeftBtn, CenterCenterBtn, CenterRightBtn},
            //                    { BottomLeftBtn, BottomCenterBtn, BottomRightBtn} };
            Player[] players = { new Player("x"), new Player("o") };
            game = new Game(players);
        }

        // 3x3 Board
        // Top Buttons
        private void TopLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void TopCenterBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void TopRightBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        // Center Buttons
        private void CenterLeftBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CenterCenterBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CenterRightBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        // Bottom buttons
        private void BottomLeftBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BottomCenterBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BottomRightBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}