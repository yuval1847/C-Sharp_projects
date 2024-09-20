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

namespace TicTacToe_WPF.Pages
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : UserControl
    {
        private ContentControl mainContentControl;

        public StartPage(ContentControl mainContentControl)
        {
            InitializeComponent();
            this.mainContentControl = mainContentControl;
        }
        private void RulesBtn_Click(object sender, RoutedEventArgs e)
        {
            this.mainContentControl.Content = new Pages.RulesPage(this.mainContentControl);
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            this.mainContentControl.Content = new Pages.GamePage(this.mainContentControl);

        }
    }
}
