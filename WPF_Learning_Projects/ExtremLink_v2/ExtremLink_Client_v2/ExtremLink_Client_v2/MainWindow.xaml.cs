using ExtremLink_Client_v2.Classes;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtremLink_Client_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            SoundManager.SoundManagerInstance.PlaySound(EPlaylist.Entrance);
            InitializeComponent();
            ContentMain.Content = new Pages.OpeningPage(ContentMain);
        }
    }
}