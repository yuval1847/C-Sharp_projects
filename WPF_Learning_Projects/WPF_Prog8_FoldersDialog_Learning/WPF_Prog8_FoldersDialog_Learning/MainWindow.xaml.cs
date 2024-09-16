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

using WinForms = System.Windows.Forms;

namespace WPF_Prog8_FoldersDialog_Learning
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
            WinForms.FolderBrowserDialog foldersDialog = new WinForms.FolderBrowserDialog();
            WinForms.DialogResult dialogResult = foldersDialog.ShowDialog();
            if (dialogResult == WinForms.DialogResult.OK)
            {
                pathLbl.Text = foldersDialog.SelectedPath;
            }
            else
            {
                pathLbl.Text = "No folder selected!";
            }
        }
    }
}
