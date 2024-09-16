using Microsoft.Win32;
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

namespace WPF_Prog7_FilesDialog_Learning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // A filter to the file dialog so that only txt files can be chosen!
            fileDialog.Filter = "txt file | *.txt";
            
            bool? success = fileDialog.ShowDialog();
            if (success == true)
            {
                filePathLbl.Text = $"file name: {fileDialog.SafeFileName}\nfull path: {fileDialog.FileName}";
            }
            else
            {
                filePathLbl.Text = "No file chosen";
            }
        }
    }
}
