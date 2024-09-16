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

namespace WPF_Prog6_MsgBoxes_Learning
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

        private void AlertBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Alert MessageBox", "Info", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ErrorBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Error MessageBox", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void InfoBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Information MessageBox", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void QuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Question MessageBox", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
