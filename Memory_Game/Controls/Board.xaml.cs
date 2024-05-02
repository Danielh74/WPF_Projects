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

namespace Memory_Game.Controls
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : UserControl
    {
        public EventHandler GameEnded;
        public EventHandler<TurnWinEventArgs> TurnWon;

        public bool isPlayerOneTurn = true;

        public BitmapImage[,] cards;
        private string[] pokemon = { "Blastoise", "Bulbasaur", "Charizard", "Charmander", "Dragonite", "Eevee", "Magikarp", "Mewtwo", "Pikachu", "Squirtle" };
        private readonly string imagesPath = @"C:\Users\Danie\Desktop\Studies\.NET\.NET_course\WPF\Projects\Memory_Game\Images";
        private BitmapImage firstClickedCard;
        private BitmapImage secondClickedCard;
        private BitmapImage cardBackground;
        ImageBrush img;
        private Button firstClickedButton;
        public Board()
        {
            InitializeComponent();
        }

        public void InitializeGameBoard(int gridSize)
        {
            cardBackground = new BitmapImage(new Uri(@"C:\Users\Danie\Desktop\Studies\.NET\.NET_course\WPF\Projects\Memory_Game\Images\card_background.png", UriKind.Absolute));
            img = new ImageBrush(cardBackground);
            cards = new BitmapImage[4, gridSize];
            Random rnd = new Random();
            int randomCol;
            int randomRow;
            for (int i = 0; i < gridSize * 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    do
                    {
                        randomCol = rnd.Next(0, gridSize);
                        randomRow = rnd.Next(0, 4);
                    } while (!IsCellEmpty(randomRow, randomCol));

                    cards[randomRow, randomCol] = new BitmapImage(new Uri($"{imagesPath}\\{pokemon[i]}.png", UriKind.Absolute));
                }
            }

            for (int row = 0; row < 4; row++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = GridLength.Auto;
                gameGrid.RowDefinitions.Add(rowDef);

                for (int col = 0; col < gridSize; col++)
                {
                    ColumnDefinition colDef = new ColumnDefinition();
                    colDef.Width = GridLength.Auto;
                    gameGrid.ColumnDefinitions.Add(colDef);

                    Button button = new Button();

                    button.Background = img;
                    button.Width = 100;
                    button.Height = 120;
                    button.Margin = new Thickness(10);

                    button.Click += RevealImage;

                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);

                    gameGrid.Children.Add(button);
                }
            }

            isPlayerOneTurn = true;
        }

        private void RevealImage(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int row = Grid.GetRow(btn);
            int col = Grid.GetColumn(btn);

            ImageBrush imageBrush = new ImageBrush(cards[row, col]);
            btn.Background = imageBrush;

            if (firstClickedCard == null)
            {
                firstClickedCard = cards[row, col];
                firstClickedButton = btn;
            }
            else if (secondClickedCard == null && firstClickedCard != cards[row, col])
            {
                secondClickedCard = cards[row, col];

                if (firstClickedCard.ToString() == secondClickedCard.ToString())
                {
                    OnTurnWin(isPlayerOneTurn);

                    gameGrid.Children.Remove(btn);
                    gameGrid.Children.Remove(firstClickedButton);
                }
                else
                {
                    ResetCardsDelayed();
                }
                firstClickedCard = null;
                secondClickedCard = null;
                firstClickedButton = null;

                if (gameGrid.Children.Count == 0)
                {
                    OnGameEnd();
                }

                isPlayerOneTurn = !isPlayerOneTurn;
            }
        }
        private async void ResetCardsDelayed()
        {
            await Task.Delay(1000);

            foreach (Button button in gameGrid.Children)
            {
                button.Background = img;
            }
        }

        private void OnTurnWin(bool isPlayerOneTurn)
        {
            TurnWon?.Invoke(this, new TurnWinEventArgs(isPlayerOneTurn));
        }

        private void OnGameEnd()
        {
            GameEnded?.Invoke(this, new EventArgs());
        }

        public void ResetGameBoard()
        {
            gameGrid.Children.Clear();
        }

        private bool IsCellEmpty(int row, int col)
        {
            return cards[row, col] == null;
        }
    }
}
