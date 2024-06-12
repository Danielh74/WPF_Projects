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
using MemoryGame.CustomEventArgs;

namespace MemoryGame.Controls
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : UserControl
    {
        public EventHandler GameEnded;
        public EventHandler<TurnWinEventArgs> TurnWon;

        public bool isPlayerOneTurn = true;

        private List<string> pokemon = new List<string>() { "Blastoise", "Bulbasaur", "Charizard", "Charmander", "Dragonite", "Eevee", "Magikarp", "Mewtwo", "Pikachu", "Squirtle" };
        private List<string> randomizePokemon;
        private string uriString = @"C:\Users\Danie\Desktop\Studies\.NET\.NET_course\WPF\Projects\Memory_Game\Resources";
        private Button firstClickedCard;
        private Button secondClickedCard;
        private Image cardBackground;
        public Board()
        {
            InitializeComponent();
        }

        public void InitializeGameBoard(int gridSize)
        {
            RandomizePokemonList(gridSize * 4);

            cardBackground = new Image()
            {
                Source = new BitmapImage(new Uri($"./Resources/card_background.png",UriKind.Relative))
            };
            
            int pokemonIndex = 0;

            for (int rows = 0; rows < 4; rows++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = GridLength.Auto;
                gameGrid.RowDefinitions.Add(row);

                for (int cols = 0; cols < gridSize; cols++)
                {
                    ColumnDefinition col = new ColumnDefinition();
                    col.Width = GridLength.Auto;
                    gameGrid.ColumnDefinitions.Add(col);

                    Button button = new Button()
                    {
                        Uid = randomizePokemon[pokemonIndex],
                        Background = new ImageBrush() { ImageSource = cardBackground.Source },
                        Width = 100,
                        Height = 120,
                        Margin = new Thickness(10)
                    };

                    button.Click += RevealImage;

                    Grid.SetRow(button, rows);
                    Grid.SetColumn(button, cols);

                    gameGrid.Children.Add(button);
                    pokemonIndex++;
                }
            }

            isPlayerOneTurn = true;
        }

        private void RevealImage(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int row = Grid.GetRow(btn);
            int col = Grid.GetColumn(btn);

            btn.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri($"{uriString}/{btn.Uid}.png", UriKind.Absolute)) };

            if (firstClickedCard == null)
            {
                firstClickedCard = btn;
            }
            else if (secondClickedCard == null && firstClickedCard != btn)
            {
                secondClickedCard = btn;

                if (firstClickedCard.Uid == secondClickedCard.Uid)
                {
                    OnTurnWin(isPlayerOneTurn);

                    gameGrid.Children.Remove(firstClickedCard);
                    gameGrid.Children.Remove(secondClickedCard);
                }
                else
                {
                    ResetCardsDelayed();
                }
                firstClickedCard = null;
                secondClickedCard = null;

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
                button.Background = new ImageBrush() { ImageSource = cardBackground.Source };
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

        private void RandomizePokemonList(int size)
        {
            randomizePokemon = Enumerable.Repeat<string>(null, size).ToList();
            Random rnd = new Random();
            int count = size / 2;

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int rndPlace = rnd.Next(0, randomizePokemon.Count - 1);
                    while (randomizePokemon[rndPlace] != null)
                    {
                        rndPlace = rnd.Next(randomizePokemon.Count);
                    }
                    randomizePokemon[rndPlace] = pokemon[i];
                }
            }
        }
    }
}
