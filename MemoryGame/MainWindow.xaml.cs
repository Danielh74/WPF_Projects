using MemoryGame.CustomEventArgs;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace MemoryGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool isGameActive = false;
        private int playerOneScore = 0;
        private int playerTwoScore = 0;

        public MainWindow()
        {
            InitializeComponent();

            gameGrid.TurnWon += HandleTurnWon;
            gameGrid.GameEnded += HandleGameEnd;
            winnerAlert.Visibility = Visibility.Collapsed;
            DataContext = this;
        }

        public int PlayerOneScore
        {
            get => playerOneScore;
            set
            {
                playerOneScore = value;
                OnPropertyChanged(nameof(PlayerOneScore));
            }
        }
        public int PlayerTwoScore
        {
            get => playerTwoScore;
            set
            {
                playerTwoScore = value;
                OnPropertyChanged(nameof(PlayerTwoScore));
            }
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void BoardSizeSelection(object sender, RoutedEventArgs e)
        {
            if (isGameActive)
            {
                gameGrid.ResetGameBoard();
            }

            if (sender == btn3)
            {
                gameGrid.InitializeGameBoard(3);
            }
            else if (sender == btn4)
            {
                gameGrid.InitializeGameBoard(4);
            }
            else if (sender == btn5)
            {
                gameGrid.InitializeGameBoard(5);
            }
            isGameActive = true;
        }
        private void HandleTurnWon(object? sender, TurnWinEventArgs e)
        {
            if (e.IsPlayerOneTurn)
            {
                PlayerOneScore++;
                p1_score.Text = PlayerOneScore.ToString();
            }
            else
            {
                PlayerTwoScore++;
                p2_score.Text = PlayerTwoScore.ToString();
            }

            HandleWinnerAlert("It's A Match!", Brushes.Yellow);
        }

        private void HandleGameEnd(object? sender, EventArgs e)
        {

            if (PlayerOneScore > playerTwoScore)
            {
                HandleWinnerAlert("Player1 Wins", Brushes.Blue);
            }
            else if (PlayerTwoScore > playerOneScore)
            {
                HandleWinnerAlert("Player2 Wins", Brushes.Red);
            }
            else
            {
                HandleWinnerAlert("It's A Draw", Brushes.Black);
            }

            isGameActive = false;
        }

        private void HandleWinnerAlert(string text, Brush color)
        {
            winnerAlert.Visibility = Visibility.Visible;
            winnerAlert.textBox.Foreground = color;
            winnerAlert.textBox.Text = text;

            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, e) =>
            {
                winnerAlert.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();
        }

        private void ResetGame(object sender, RoutedEventArgs e)
        {
            isGameActive = false;
            PlayerOneScore = 0;
            PlayerTwoScore = 0;
            gameGrid.ResetGameBoard();
        }
        private void ExitGame(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}