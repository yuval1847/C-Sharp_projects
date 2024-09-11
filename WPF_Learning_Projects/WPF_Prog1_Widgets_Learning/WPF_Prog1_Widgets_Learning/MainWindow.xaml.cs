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

namespace WPF_Prog1_Widgets_Learning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int clicksNum;
        public MainWindow()
        {
            InitializeComponent();
            this.clicksNum = 0;
        }

        private void btnChangeLblPos_Click(object sender, RoutedEventArgs e)
        {
            if (clicksNum % 8 == 0){ lblHelloWorld.HorizontalAlignment = HorizontalAlignment.Left; lblHelloWorld.VerticalAlignment = VerticalAlignment.Top;}
            else if(clicksNum % 8 == 1) { lblHelloWorld.HorizontalAlignment = HorizontalAlignment.Center;}        
            else if (clicksNum % 8 == 2) { lblHelloWorld.HorizontalAlignment = HorizontalAlignment.Right; }
            else if (clicksNum % 8 == 3) { lblHelloWorld.HorizontalAlignment = HorizontalAlignment.Left; lblHelloWorld.VerticalAlignment = VerticalAlignment.Center; }

            else if (clicksNum % 8 == 4) { lblHelloWorld.HorizontalAlignment = HorizontalAlignment.Right;}
            else if (clicksNum % 8 == 5) { lblHelloWorld.HorizontalAlignment = HorizontalAlignment.Left; lblHelloWorld.VerticalAlignment = VerticalAlignment.Bottom; }
            else if (clicksNum % 8 == 6) { lblHelloWorld.HorizontalAlignment = HorizontalAlignment.Center; }
            else if (clicksNum % 8 == 7) { lblHelloWorld.HorizontalAlignment = HorizontalAlignment.Right; }
            this.clicksNum++;
        }
    }
}
