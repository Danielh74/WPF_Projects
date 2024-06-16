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
using System.Xml.Linq;
using Tic_Tac_Toe.Controls;
using Tic_Tac_Toe.Enums;

namespace Tic_Tac_Toe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int playerOneScore = 0;
        private int playerTwoScore = 0;
       
        public MainWindow()
        {
            InitializeComponent();
            endGameMessage.Visibility = Visibility.Collapsed;
            gameBoard.Visibility = Visibility.Collapsed;
            leaderboard.Visibility = Visibility.Collapsed;
            DataContext = this;

            gameBoard.GameEnded += HandleGameEnded;
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
        
        private void StartGame(object sender, RoutedEventArgs e)
        {
            GameType gameType;

            if (sender == Btn_PvP)
            {
                if (p1_text.Text == "" || p2_text.Text == "")
                {
                    MessageBox.Show("Players names are empty");
                    playerNameBoxes.Visibility = Visibility.Visible;

                    return;
                }
                gameType = GameType.PvP;
                p1_name.Text = $"{p1_text.Text}";
                p2_name.Text = $"{p2_text.Text}";

                ActiveGameMode(Btn_PvP);
            }
            else if (sender == Btn_PvC)
            {
                gameType = GameType.PvC;
                p1_name.Text = $"Player 1";
                p2_name.Text = "Computer";

                ActiveGameMode(Btn_PvC);
            }
            else if (sender == Btn_CvC)
            {
                gameType = GameType.CvC;
                p1_name.Text = "Computer 1";
                p2_name.Text = "Computer 2";

                ActiveGameMode(Btn_CvC);
            }
            else
            {
                Close();
                return;
            }

            if(sender != Btn_PvP)
            {
                p1_text.Text = "";
                p2_text.Text = "";
            }

            playerNameBoxes.Visibility = Visibility.Collapsed;
            p1_name.Foreground = Brushes.Blue;
            p2_name.Foreground = Brushes.Red;
            PlayerOneScore = 0;
            PlayerTwoScore = 0;

            gameBoard.Visibility = Visibility.Visible;
            leaderboard.Visibility = Visibility.Visible;
            gameBoard.StartNewGame(gameType, p1_name.Text, p2_name.Text);
        }

        // Checks which game mode is active pvp, pvc, cvc and highlight the corresponding button to display the active mode.
        private void ActiveGameMode(Button activeButton)
        {
            Button[] buttons = { Btn_PvP, Btn_PvC, Btn_CvC };
            foreach (Button button in buttons)
            {
                if (button == activeButton)
                {

                    activeButton.Background = Brushes.LightBlue;
                }
                else
                {
                    button.ClearValue(BackgroundProperty);
                    button.Style = (Style)FindResource("GameModeButtonStyle");
                }
            }
        }

        //Checks who won and display a winner message accordingly.
        private void HandleGameEnded(object? sender, GameEndEventArgs e)
        {
            switch (e.GameResult)
            {
                case GameResult.PlayerOneWins:
                    PlayerOneScore++;
                    endGameMessage.textBox.Text = p1_name.Text + " Wins!";
                    endGameMessage.textBox.Foreground = Brushes.Blue;
                    break;

                case GameResult.PlayerTwoWins:
                    PlayerTwoScore++;
                    endGameMessage.textBox.Text = p2_name.Text + " Wins!";
                    endGameMessage.textBox.Foreground = Brushes.Red;
                    break;

                case GameResult.Draw:
                    endGameMessage.textBox.Text = "It's a draw!";
                    endGameMessage.textBox.Foreground = Brushes.Black;
                    break;
            }
            endGameMessage.Visibility = Visibility.Visible;

            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, e) =>
            {
                endGameMessage.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();
        }

        private void CloseGame(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}