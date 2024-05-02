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
        private int playerOneScore = 0;
        private int playerTwoScore = 0;

        public event PropertyChangedEventHandler? PropertyChanged;
        public MainWindow()
        {
            InitializeComponent();
            MyMessage.Visibility = Visibility.Collapsed;
            DataContext = this;

            MyBoard.GameEnded += HandleGameEnded;
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
        
        private void NewGame_Click(object sender, RoutedEventArgs e)
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
                p1_name.Text = $"Player1";
                p2_name.Text = "Computer";

                ActiveGameMode(Btn_PvC);
            }
            else if (sender == Btn_CvC)
            {
                gameType = GameType.CvC;
                p1_name.Text = "Computer1";
                p2_name.Text = "Computer2";

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

            MyBoard.StartNewGame(gameType, p1_name.Text, p2_name.Text);
        }
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
                    button.Background = Brushes.LightGray;
                }

            }
        }
        private void HandleGameEnded(object? sender, GameEndEventArgs e)
        {
            switch (e.GameResult)
            {
                case GameResult.PlayerOneWins:
                    PlayerOneScore++;
                    MyMessage.textBox.Text = p1_name.Text + " Wins!";
                    MyMessage.textBox.Foreground = Brushes.Blue;
                    break;

                case GameResult.PlayerTwoWins:
                    PlayerTwoScore++;
                    MyMessage.textBox.Text = p2_name.Text + " Wins!";
                    MyMessage.textBox.Foreground = Brushes.Red;
                    break;

                case GameResult.Draw:
                    MyMessage.textBox.Text = "It's a draw!";
                    MyMessage.textBox.Foreground = Brushes.Black;
                    break;
            }

            MyMessage.Visibility = Visibility.Visible;
            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, e) =>
            {
                MyMessage.Visibility = Visibility.Collapsed;
                timer.Stop();

            };
            timer.Start();
           
        }
    }
}