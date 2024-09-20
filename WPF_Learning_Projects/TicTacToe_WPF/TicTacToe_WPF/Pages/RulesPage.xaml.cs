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
    /// Interaction logic for RulesPage.xaml
    /// </summary>
    public partial class RulesPage : UserControl
    {
        private ContentControl mainContentControl;

        public RulesPage(ContentControl mainContentControl)
        {
            InitializeComponent();
            this.mainContentControl = mainContentControl;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.mainContentControl.Content = new Pages.StartPage(this.mainContentControl);
        }
    }
}
