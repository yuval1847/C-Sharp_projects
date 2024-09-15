using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace WPF_Prog4_CustomTextBox_Learning.View.UserControls
{
    /// <summary>
    /// Interaction logic for DesignedTextBox.xaml
    /// </summary>
    public partial class DesignedTextBox : UserControl
    {
        public DesignedTextBox()
        {
            InitializeComponent();
        }
        private string placeholder;
        public string Placeholder 
        { 
            get{return placeholder;}
            set{placeholder = value;
                contentHolder.Text = placeholder;
            } 
        }
        private void customTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(customTB.Text)) { contentHolder.Visibility = Visibility.Visible; }
            else { contentHolder.Visibility = Visibility.Hidden;}
        }

        private void eraseBtn_Click(object sender, RoutedEventArgs e)
        {
            customTB.Text = string.Empty;
        }
    }
}
