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

namespace TicTacToe_WPF.Pages
{
    /// <summary>
    /// Interaction logic for WinnerDeclerationPage.xaml
    /// </summary>
    public partial class WinnerDeclerationPage : UserControl
    {
        public WinnerDeclerationPage(string? winner)
        {
            InitializeComponent();
            if (winner == "TIE")
            {
                winnerTitle.Text = "TIE!";
            }
            else
            {
                winnerTitle.Text = $"The winner is: {winner}";
            }
        }
    }
}
