using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;
using System.Xml.Linq;
using Tic_Tac_Toe.Enums;

namespace Tic_Tac_Toe.Controls
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public EventHandler<GameEndEventArgs> GameEnded;

        private const string PlayerOneContent = "X";
        private const string PlayerTwoContent = "O";

        private string _activePlayer;

        private string p1Name = "";
        private string p2Name = "";

        private readonly Button[,] board = new Button[3, 3];

        private readonly Random _rnd = new Random();

        private bool isGameActive = false;
        private GameType _gameType = GameType.PvP;



        public Board()
        {
            InitializeComponent();
            InitializeGameGrid();

            PropertyChanged += HandlePropertyChange;

            DataContext = this;
        }

        private void HandlePropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            if (ActivePlayer == p1Name)
            {
                indicatorBorder.Style = (Style)FindResource("PlayerOneBorderStyle");
            }
            else
            {
                indicatorBorder.Style = (Style)FindResource("PlayerTwoBorderStyle");
            }
        }

        private void OnGameEnd(GameResult result)
        {
            GameEnded?.Invoke(this, new GameEndEventArgs(result));
        }
        public string ActivePlayer
        {
            get => _activePlayer;
            set
            {
                _activePlayer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPlayerTurn)));
            }
        }

        public string CurrentPlayerTurn =>
            ActivePlayer == p1Name ? $"{p1Name} ({PlayerOneContent})"
            : ActivePlayer == p2Name ? $"{p2Name} ({PlayerTwoContent})"
            : "";

        private void InitializeGameGrid()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Button btn = new Button()
                    {
                        Style = (Style)FindResource("BoardButtonStyle")
                    };

                    btn.Click += Button_Click;

                    Grid.SetRow(btn, i);
                    Grid.SetColumn(btn, j);

                    board[i, j] = btn;

                    GameGrid.Children.Add(btn);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isGameActive || (_gameType == GameType.PvC && ActivePlayer != p1Name) || _gameType == GameType.CvC)
            {
                return;
            }

            Button btn = sender as Button;
            if (btn == null)
            {
                return;
            }

            if (btn.Content == null)
            {
                btn.Content = ActivePlayer == p1Name ? PlayerOneContent : PlayerTwoContent;

                if (ProcessEndGame())
                {
                    return;
                }

                ActivePlayer = ActivePlayer == p1Name ? p2Name : p1Name;

                if (_gameType == GameType.PvC && ActivePlayer != p1Name)
                {
                    ComputerMove();
                }
            }
        }

        private void ComputerMove()
        {
            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, e) =>
            {
                timer.Stop();

                Button btn;
                do
                {
                    int row = _rnd.Next(3);
                    int col = _rnd.Next(3);
                    btn = board[row, col];
                } while (btn.Content != null);


                btn.Content = ActivePlayer == p1Name ? PlayerOneContent : PlayerTwoContent;

                if (ProcessEndGame())
                {
                    return;
                }

                ActivePlayer = ActivePlayer == p1Name ? p2Name : p1Name;

                if (_gameType == GameType.CvC && !IsBoardFull())
                {
                    ComputerMove();
                }
            };
            timer.Start();
        }

        private bool ProcessEndGame()
        {
            if (CheckForWinner())
            {
                GameResult result = ActivePlayer == p1Name ? GameResult.PlayerOneWins : GameResult.PlayerTwoWins;

                OnGameEnd(result);

                DelayBoardRestart();


                if (_gameType == GameType.CvC || result == GameResult.PlayerTwoWins)
                {
                    ComputerMove();
                }
                return true;
            }

            if (IsBoardFull())
            {
                GameResult result = GameResult.Draw;

                OnGameEnd(result);

                DelayBoardRestart();

                if (_gameType == GameType.CvC)
                {
                    ComputerMove();
                }
                return true;
            }

            return false;
        }

        public void StartNewGame(GameType gameType, string name1, string name2)
        {
            isGameActive = true;

            _gameType = gameType;
            p1Name = name1;
            p2Name = name2;

            ActivePlayer = p1Name;

            foreach (Button btn in board)
            {
                btn.Content = null;
            }

            if (_gameType == GameType.CvC)
            {
                ComputerMove();
            }
        }

        private bool IsBoardFull()
        {
            foreach (Button button in board)
            {
                if (button.Content == null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckForWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                if (AreButtonsEqual(board[i, 0], board[i, 1], board[i, 2]))
                {
                    return true;
                }

                if (AreButtonsEqual(board[0, i], board[1, i], board[2, i]))
                {
                    return true;
                }
            }

            if (AreButtonsEqual(board[0, 0], board[1, 1], board[2, 2]))
            {
                return true;
            }
            if (AreButtonsEqual(board[0, 2], board[1, 1], board[2, 0]))
            {
                return true;
            }

            return false;
        }

        private bool AreButtonsEqual(Button b1, Button b2, Button b3) =>
             b1.Content != null && b1.Content == b2.Content && b2.Content == b3.Content;

        private void DelayBoardRestart()
        {
            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += (sender, e) =>
            {
                foreach (Button btn in board)
                {
                    btn.Content = null;
                }
                timer.Stop();

            };
            timer.Start();
        }
    }
}
