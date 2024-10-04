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
    /// Interaction logic for CustomTextBox.xaml
    /// </summary>
    public partial class CustomTextBox : UserControl
    {

        private string placeholder;
        private string text;
        private bool isHide;

        public string Placeholder
        {
            get { return placeholder; }
            set { placeholder = value; contentHolder.Text = placeholder; }
        }
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }
        public bool IsHide
        {
            get { return this.isHide; }
            set { this.isHide = value; }
        }


        public CustomTextBox()
        {
            InitializeComponent();
        }


        private void customTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(customTB.Text)) { contentHolder.Visibility = Visibility.Visible; }
            else { contentHolder.Visibility = Visibility.Hidden; }

            this.Text = customTB.Text;

            if (this.IsHide)
            {
                this.HideTextWithStars();
            }
        }

        private void HideTextWithStars()
        {
            // The function gets nothing.
            // The function change the text of the custom textbox to starts('*' -> shift+8).
            int caretIndex = customTB.CaretIndex;
            customTB.Text = new string('*', Text.Length);
            customTB.CaretIndex = caretIndex;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Set focus to the TextBox when the user clicks on the Grid.
            customTB.Focus();
        }

        private void eraseBtn_Click(object sender, RoutedEventArgs e)
        {
            // The function gets nothing.
            // The function works when the erase button is
            // clicked and it erase the content of the custom textbox.
            customTB.Text = string.Empty;
            this.Text = "";
        }
    }
}
