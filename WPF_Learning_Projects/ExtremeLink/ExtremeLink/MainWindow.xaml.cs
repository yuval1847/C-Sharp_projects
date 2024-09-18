using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtremeLink
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer, _timerLoadingBar;
        private string _text = "ExtremLink";
        private int _currentIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            StartMatrixAnimation();
            StartLoadingAnimation();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard imageStoryboard = (Storyboard)this.Resources["ImageFadeAndMoveStoryboard"];
            imageStoryboard.Begin();
        }

        // The fade in animation
        private void StartMatrixAnimation()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(150); // Adjust speed as needed
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_currentIndex < _text.Length)
            {
                // Create matrix effect only for characters that haven't been revealed yet
                var random = new Random();
                var matrixText = string.Empty;

                // Loop through the text and generate matrix effect
                for (int i = 0; i < _text.Length; i++)
                {
                    if (i < _currentIndex)
                    {
                        // Show the actual characters
                        matrixText += _text[i];
                    }
                    else
                    {
                        // Show spaces after the end of the actual text to avoid extra characters
                        matrixText += ' ';
                    }
                }

                // Update the TextBlock with the generated text
                MatrixTextBlock.Text = matrixText;

                // Move to the next character in the actual text
                _currentIndex++;
            }
            else
            {
                // Ensure that the final text is the actual _text without extra characters
                MatrixTextBlock.Text = _text;
                _timer.Stop(); // Stop the animation when the full text is revealed
            }
        }

        // The loading progress bar animation
        private void StartLoadingAnimation()
        {
            // The function gets nothing.
            // The function start the loading bar animation.
            _timerLoadingBar = new DispatcherTimer();
            _timerLoadingBar.Interval = TimeSpan.FromMilliseconds(20);
            _timerLoadingBar.Tick += LoadingTimerTick;
            _timerLoadingBar.Start();
        }

        private void LoadingTimerTick(object sender, EventArgs e)
        {
            // The function gets nothing.
            // The function operate the progress bar loading animation
            if(loadingProgressBar.Value < 200)
            {
                loadingProgressBar.Value += 1;
            }
            else
            {
                _timerLoadingBar.Stop();
            }
        }
    }
}
