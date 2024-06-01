using System.ComponentModel;
using System.Numerics;
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
using Uno_Game.CustomEventArgs;
using Uno_Game.Enums;
using Uno_Game.Helpers;

namespace Uno_Game;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    List<string> deck = new List<string>();
    int remainingCardsInPile;
    readonly string[] colors = ["green", "red", "blue", "yellow"];
    string currentColor;
    string currentNumber;
    string upCardName;
    bool isClockWise = true;
    Player activePlayer;
    Player playerOne;
    Player playerTwo;
    Player playerThree;
    Player playerFour;
    List<Player> turns;
    int currentTurn;
    int nextTurn;
    List<string> player1Deck;
    List<string> player2Deck;
    List<string> player3Deck;
    List<string> player4Deck;

    public MainWindow()
    {
        InitializeComponent();

        Player1Space.Visibility = Visibility.Collapsed;
        GameEndWindow.Visibility = Visibility.Collapsed;

        GameBoard.DataContext = this;

        PlayerSelectionWindow.PlayerNumberSelected += HandlePlayerNumberSelected;
        GameEndWindow.GameRestart += HandleGameRestart;
        GameEndWindow.GameClosed += (object? sender, EventArgs e) => { Close(); };
        PropertyChanged += HandleActivePlayerChanged;
    }

    public string CurrentColor
    {
        get => currentColor;
        set
        {
            currentColor = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentColor)));
        }
    }

    public string CurrentNumber
    {
        get => currentNumber;
        set
        {
            currentNumber = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentNumber)));
        }
    }

    public string UpCardName
    {
        get => upCardName;
        set
        {
            upCardName = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UpCardName)));
        }
    }

    public Player ActivePlayer
    {
        get => activePlayer;
        set
        {
            activePlayer = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActivePlayer)));
        }
    }

    private void HandlePlayerNumberSelected(object? sender, PlayerSelectionEventArgs e)
    {
        InitializeDeck();

        PlayerSelectionWindow.Visibility = Visibility.Collapsed;

        player1Deck = new List<string>();
        playerOne = new Player("Player 1", Player1Hand, player1Deck);
        InitializePlayerDeck(playerOne);
        for (int j = 0; j < 7; j++)
        {
            Button card = new Button()
            {
                Uid = deck[remainingCardsInPile - 1],
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, -40, 0),
                BorderBrush = new SolidColorBrush(Colors.Transparent),
                Height = 125,
                Width = 75,
                Content = new Image() { Source = new BitmapImage(new Uri($@".\Resources\{deck[remainingCardsInPile - 1]}.png", UriKind.Relative)) }
            };
            card.Click += PlayerTurn;
            playerOne.Hand.Children.Add(card);
            playerOne.Deck.Add(deck[remainingCardsInPile - 1]);
            remainingCardsInPile -= 1;
        }

        switch (e.PlayerMode)
        {
            case NumOfPlayers.Two:
                player2Deck = new List<string>();
                playerTwo = new Player("Player 2", Player2Hand, player2Deck);
                CardsDeal(playerTwo);
                turns = [playerTwo, playerOne];
                break;
            case NumOfPlayers.Three:
                player2Deck = new List<string>();
                player3Deck = new List<string>();
                playerTwo = new Player("Player 2", Player2Hand, player2Deck);
                playerThree = new Player("Player 3", Player3Hand, player3Deck);
                CardsDeal(playerTwo, playerThree);
                turns = [playerTwo, playerThree, playerOne];
                break;
            case NumOfPlayers.Four:
                player2Deck = new List<string>();
                player3Deck = new List<string>();
                player4Deck = new List<string>();
                playerTwo = new Player("Player 2", Player2Hand, player2Deck);
                playerThree = new Player("Player 3", Player3Hand, player3Deck);
                playerFour = new Player("Player 4", Player4Hand, player4Deck);
                CardsDeal(playerTwo, playerThree, playerFour);
                turns = [playerTwo, playerThree, playerOne, playerFour];
                break;
        }
        activePlayer = playerTwo;

        do
        {
            UpCardName = deck[remainingCardsInPile - 1];
        }
        while (deck[remainingCardsInPile - 1].Contains("black"));

        UpCardImage.Source = new BitmapImage(new Uri($@".\Resources\{upCardName}.png", UriKind.Relative));
        remainingCardsInPile -= 1;
        CurrentColor = Utils.FindCurrentColor(UpCardName);
        CurrentNumber = Utils.FindCurrentNumber(UpCardName);

        GameStart();
    }

    private void InitializePlayerDeck(Player player)
    {
        player.Deck.Clear();
        player.Hand.Children.Clear();
    }

    private void InitializeDeck()
    {
        deck.Clear();

        for (int i = 0; i < 4; i++)
        {
            deck.Add($"0_{colors[i]}");
            deck.Add($"plus2_{colors[i]}");
            deck.Add($"plus2_{colors[i]}");
            deck.Add($"skip_{colors[i]}");
            deck.Add($"skip_{colors[i]}");
            deck.Add($"reverse_{colors[i]}");
            deck.Add($"reverse_{colors[i]}");
            deck.Add($"plus4_black");
            deck.Add($"wild_black");
        }

        for (int i = 1; i <= 9; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                deck.Add($"{i}_{colors[j]}");
                deck.Add($"{i}_{colors[j]}");
            }
        }

        ShuffleDeck(deck);
        remainingCardsInPile = deck.Count;
    }

    private void ShuffleDeck(List<string> deck)
    {
        Random rnd = new Random();
        int count = deck.Count - 1;

        while (count > 0)
        {
            int rndPlace = rnd.Next(count);
            string rndCard = deck[rndPlace];
            deck[rndPlace] = deck[count];
            deck[count] = rndCard;
            count--;
        }
    }

    private void CardsDeal(params Player[] players)
    {
        foreach (Player player in players)
        {
            InitializePlayerDeck(player);

            for (int i = 0; i < 7; i++)
            {
                Image cardImage = new Image()
                {
                    Uid = deck[remainingCardsInPile - 1],
                    Source = new BitmapImage(new Uri(@".\Resources\card_back.png", UriKind.Relative)),
                    Height = 100,
                    Width = 50,
                    Margin = new Thickness(0, 0, -15, 0),
                };
                player.Hand.Children.Add(cardImage);
                player.Deck.Add(deck[remainingCardsInPile - 1]);
                remainingCardsInPile -= 1;
            }
        }
    }

    private void GameStart()
    {
        isClockWise = true;
        ActivePlayer = playerTwo;
        ComputerMove(ActivePlayer);
    }

    private void ComputerMove(Player player)
    {
        DispatcherTimer timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(3)
        };
        timer.Tick += (sender, e) =>
        {
            timer.Stop();

            var card = player.Deck.Find(c =>
            c.Contains(CurrentColor.ToLower()) ||
            c.Contains(CurrentNumber) ||
            c.Contains("black"));

            if (card != null)
            {
                UpCardImage.Source = new BitmapImage(new Uri($@".\Resources\{card}.png", UriKind.Relative));
                UpCardName = card;
                CurrentColor = Utils.FindCurrentColor(UpCardName);
                CurrentNumber = Utils.FindCurrentNumber(UpCardName);
                player.Hand.Children.RemoveAt(player.Hand.Children.Count - 1);
                player.Deck.Remove(card);

                if (!int.TryParse(currentNumber, out int numberCard))
                {
                    HandleSpecialCardSet();
                }
                else
                {
                    nextTurn = AdvanceTurn(1);
                }
            }
            else
            {
                ChackPileEmpty(1);

                Image cardImage = new Image()
                {
                    Uid = deck[remainingCardsInPile - 1],
                    Source = new BitmapImage(new Uri(@".\Resources\card_back.png", UriKind.Relative)),
                    Height = 100,
                    Width = 50,
                    Margin = new Thickness(0, 0, -15, 0),
                };
                player.Hand.Children.Add(cardImage);
                player.Deck.Add(deck[remainingCardsInPile - 1]);
                remainingCardsInPile -= 1;

                nextTurn = AdvanceTurn(1);
            }

            if (Utils.ChackGameWon(activePlayer))
            {
                GameEndWindow.DataContext = activePlayer;
                GameEndWindow.Visibility = Visibility.Visible;
                return;
            }

            ActivePlayer = turns[nextTurn];
            currentTurn = nextTurn;

            if (ActivePlayer != playerOne)
            {
                ComputerMove(ActivePlayer);
            }
            else
            {
                if (player1Deck.Find(c => c.Contains(CurrentColor.ToLower()) || c.Contains(CurrentNumber)) == null)
                {
                    DrawPile.Click += HumanPlayerDraw;
                }
            }
        };
        timer.Start();
    }

    private void HandleActivePlayerChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (ActivePlayer.Name)
        {
            case "Player 1":
                ResetPlayerText();
                HighlightPlayerText(Player1Text);
                break;
            case "Player 2":
                ResetPlayerText();
                HighlightPlayerText(Player2Text);
                break;
            case "Player 3":
                ResetPlayerText();
                HighlightPlayerText(Player3Text);
                break;
            case "Player 4":
                ResetPlayerText();
                HighlightPlayerText(Player4Text);
                break;
        }
    }

    private void HighlightPlayerText(TextBlock playerText)
    {
        playerText.FontSize = 16;
        playerText.FontWeight = FontWeights.Bold;
        playerText.Foreground = Brushes.Black;
    }

    private void ResetPlayerText()
    {
        for (int i = 1; i <= 4; i++)
        {
            TextBlock playerText = FindName($"Player{i}Text") as TextBlock;
            if (playerText != null)
            {
                playerText.FontSize = 12;
                playerText.FontWeight = FontWeights.Regular;
                playerText.Foreground = Brushes.LightGray;
            }
        }
    }

    private int AdvanceTurn(int amount)
    {
        if (isClockWise)
        {
            return (currentTurn + amount) % turns.Count;
        }
        else
        {
            if ((currentTurn - amount) % turns.Count < 0)
            {
                return ((currentTurn - amount) % turns.Count) + turns.Count;
            }
            else
            {
                return (currentTurn - amount) % turns.Count;
            }
        }
    }

    private void PlayerTurn(object sender, RoutedEventArgs e)
    {
        if (activePlayer != playerOne)
        {
            return;
        }

        Button button = sender as Button;
        if (button.Uid.Contains(CurrentColor.ToLower()) ||
            button.Uid.Contains(CurrentNumber) ||
            button.Uid.Contains("plus4") ||
            button.Uid.Contains("wild"))
        {
            Image buttonImage = (Image)button.Content;
            UpCardImage.Source = buttonImage.Source;
            UpCardName = button.Uid;
            CurrentColor = Utils.FindCurrentColor(UpCardName);
            CurrentNumber = Utils.FindCurrentNumber(UpCardName);
            Player1Hand.Children.Remove(button);
            playerOne.Deck.Remove(button.Uid);

            currentTurn = turns.IndexOf(ActivePlayer);

            if (!int.TryParse(currentNumber, out int numberCard))
            {
                HandleSpecialCardSet();
            }
            else
            {
                nextTurn = AdvanceTurn(1);
            }

            if (Utils.ChackGameWon(activePlayer))
            {
                GameEndWindow.DataContext = activePlayer;
                GameEndWindow.Visibility = Visibility.Visible;
                return;
            }

            ActivePlayer = turns[nextTurn];
            currentTurn = nextTurn;

            if (ActivePlayer != playerOne)
            {
                ComputerMove(ActivePlayer);
            }
        }
        else
        {
            return;
        }
    }

    private void HandleSpecialCardSet()
    {
        if (currentNumber == "skip")
        {
            nextTurn = AdvanceTurn(2);
        }
        else if (currentNumber == "reverse")
        {
            isClockWise = !isClockWise;

            nextTurn = AdvanceTurn(1);
        }
        else if (currentNumber == "plus2")
        {
            ChackPileEmpty(2);

            SpecialCardDraw(2);

            nextTurn = AdvanceTurn(2);
        }
        else if (currentNumber == "plus4")
        {
            ChackPileEmpty(4);

            SpecialCardDraw(4);

            if (activePlayer != playerOne)
            {
                Random rnd = new Random();
                string color = colors[rnd.Next(colors.Length)];
                CurrentColor = char.ToUpper(color[0]) + color.Substring(1);
                nextTurn = AdvanceTurn(2);
            }
            else
            {
                Player1Space.Visibility = Visibility.Visible;
            }
        }
        else if (currentNumber == "wild")
        {
            if (activePlayer != playerOne)
            {
                Random rnd = new Random();
                string color = colors[rnd.Next(colors.Length)];
                CurrentColor = char.ToUpper(color[0]) + color.Substring(1);
                nextTurn = AdvanceTurn(1);
            }
            else
            {
                Player1Space.Visibility = Visibility.Visible;
            }
        }
    }

    private void HumanPlayerDraw(object sender, RoutedEventArgs e)
    {
        ChackPileEmpty(1);

        Button card = new Button()
        {
            Uid = deck[remainingCardsInPile - 1],
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(1),
            Margin = new Thickness(0, 0, -40, 0),
            BorderBrush = new SolidColorBrush(Colors.Transparent),
            Height = 125,
            Width = 75,
            Content = new Image() { Source = new BitmapImage(new Uri($@".\Resources\{deck[remainingCardsInPile - 1]}.png", UriKind.Relative)) }
        };
        card.Click += PlayerTurn;
        playerOne.Hand.Children.Add(card);
        playerOne.Deck.Add(deck[remainingCardsInPile - 1]);
        remainingCardsInPile -= 1;

        DrawPile.Click -= HumanPlayerDraw;

        nextTurn = AdvanceTurn(1);
        ActivePlayer = turns[nextTurn];
        currentTurn = nextTurn;
    }

    private void SpecialCardDraw(int times)
    {
        if (turns[AdvanceTurn(1)] != playerOne)
        {
            for (int i = 0; i < times; i++)
            {
                Image cardImage = new Image()
                {
                    Uid = deck[remainingCardsInPile - 1],
                    Source = new BitmapImage(new Uri(@".\Resources\card_back.png", UriKind.Relative)),
                    Height = 100,
                    Width = 50,
                    Margin = new Thickness(0, 0, -15, 0),
                };
                turns[AdvanceTurn(1)].Hand.Children.Add(cardImage);
                turns[AdvanceTurn(1)].Deck.Add(deck[remainingCardsInPile - 1]);
                remainingCardsInPile -= 1;
            }
        }
        else
        {
            for (int i = 0; i < times; i++)
            {
                Button card = new Button()
                {
                    Uid = deck[remainingCardsInPile - 1],
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(0, 0, -40, 0),
                    BorderBrush = new SolidColorBrush(Colors.Transparent),
                    Height = 125,
                    Width = 75,
                    Content = new Image() { Source = new BitmapImage(new Uri($@".\Resources\{deck[remainingCardsInPile - 1]}.png", UriKind.Relative)) }
                };
                card.Click += PlayerTurn;
                playerOne.Hand.Children.Add(card);
                playerOne.Deck.Add(deck[remainingCardsInPile - 1]);
                remainingCardsInPile -= 1;
            }
        }
    }

    private void ColorChange(object sender, RoutedEventArgs e)
    {
        Button colorBtn = sender as Button;
        CurrentColor = colorBtn.Name.ToString();
        Player1Space.Visibility = Visibility.Collapsed;
        if (currentNumber == "plus4")
        {
            nextTurn = AdvanceTurn(2);
            ActivePlayer = turns[nextTurn];
            currentTurn = nextTurn;
            if (ActivePlayer != playerOne)
            {
                ComputerMove(ActivePlayer);
            }
        }
        else
        {
            nextTurn = AdvanceTurn(1);
            ActivePlayer = turns[nextTurn];
            currentTurn = nextTurn;
            if (ActivePlayer != playerOne)
            {
                ComputerMove(ActivePlayer);
            }
        }
    }

    private void ChackPileEmpty(int condition)
    {
        if (remainingCardsInPile < condition)
        {
            InitializeDeck();
        }
    }

    private void HandleGameRestart(object? sender, EventArgs e)
    {
        Player1Space.Visibility = Visibility.Collapsed;
        PlayerSelectionWindow.Visibility = Visibility.Visible;
    }
}