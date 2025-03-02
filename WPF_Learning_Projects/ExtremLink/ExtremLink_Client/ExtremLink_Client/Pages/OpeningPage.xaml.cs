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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ExtremLink_Client.Pages
{
    /// <summary>
    /// Interaction logic for OpeningPage.xaml
    /// </summary>
    public partial class OpeningPage : UserControl
    {
        private ContentControl contentMain;
        private DispatcherTimer timer;
        private string[] loadingSteps = new string[]
        {
            "Initializing...",
            "Checking for updates...",
            "Loading user preferences...",
            "Preparing remote control modules...",
            "Starting ExtremLink..."
        };
        private int currentStep = 0;
        private bool initializationCompleted;

        public ContentControl ContentMain
        {
            get { return contentMain; }
            set { this.contentMain = value; }
        }
        public event EventHandler InitializationComplete;

        public OpeningPage(ContentControl contentMain)
        {
            InitializeComponent();
            InitializeTimer();
            this.ContentMain = contentMain;
            Loaded += OpeningPageControl_Loaded;
            this.initializationCompleted = false;
        }

        // Opening animation
        private void OpeningPageControl_Loaded(object sender, RoutedEventArgs e)
        {
            // The function gets nothing.
            // The function create a StoryBoard object to oragnaize the openning animation where
            // all the animations will be excuted by a timeline.
            Storyboard openingAnimation = (Storyboard)FindResource("OpeningAnimation");
            openingAnimation.Completed += this.OpeningAnimation_Completed;
            openingAnimation.Begin();
        }

        private void OpeningAnimation_Completed(object sender, EventArgs e)
        {
            // The functin gets nothing.
            // The function calls to all the animations by order.
            this.InitializeTimer();
        }

        private void InitializeTimer()
        {
            // The function gets nothing.
            // The function creats a timer for the animation with a delay of 2 seconds (between each tick)
            // and starts it.
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += this.Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // The function gets nothing.
            // The function performs the animation of the change text.
            if (this.currentStep < this.loadingSteps.Length)
            {
                LoadingText.Text = loadingSteps[currentStep];
                currentStep++;
            }
            else
            {
                timer.Stop();
                this.OnInitializationComplete();
            }
        }

        protected void OnInitializationComplete()
        {
            // The function gets nothing.
            // The function invokes the InitializationComplete event, waits for couple of seconds
            // and moves to the next page.
            if (!this.initializationCompleted)
            {
                this.initializationCompleted = true;

                if (InitializationComplete != null)
                {
                    this.InitializationComplete.Invoke(this, EventArgs.Empty);
                }

                //this.ContentMain.Content = new LoginPage(this.ContentMain);
                this.ContentMain.Content = new HomePage(this.ContentMain);
            }
        }
    }
}
