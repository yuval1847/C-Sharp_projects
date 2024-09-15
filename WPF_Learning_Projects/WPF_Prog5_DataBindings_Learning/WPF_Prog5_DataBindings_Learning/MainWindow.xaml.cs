﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WPF_Prog5_DataBindings_Learning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private string sharedtxt;
        public string SharedTxt
        { 
            get { return sharedtxt; }
            set { sharedtxt = value;
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SharedTxt"));
            } 
        }

        
    }
}
