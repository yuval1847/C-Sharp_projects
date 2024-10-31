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

namespace ExtremLink_Client.Pages
{
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void signUpBtn_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsAllParametersRight())
            {

            }
        }

        private bool IsAllParametersRight()
        {
            // The function gets nothing.
            // The function returns true if all the sign-up parameter are fit the 
            // requirments, otherwise, it returns false and show textboxes of the problems.
            
            
            return true;
        }
    }
}
