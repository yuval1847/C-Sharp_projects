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

namespace ExtremLink_Client.CustomWidgets
{
    /// <summary>
    /// Interaction logic for CustomButton.xaml
    /// </summary>
    public partial class CustomButton : UserControl
    {
        private string placeholder;

        public string Placeholder
        {
            get { return placeholder; }
            set { placeholder = value; customBtn.Content = placeholder; }
        }
        public event RoutedEventHandler CustomClick;

        public CustomButton()
        {
            InitializeComponent();
            customBtn.Click += this.customBtn_Click;
        }

        private void customBtn_Click(object sender, RoutedEventArgs e)
        {
            // The function gets nothing.
            // The function invoke a function to operate when click.
            CustomClick?.Invoke(this, e);
        }
    }
}
