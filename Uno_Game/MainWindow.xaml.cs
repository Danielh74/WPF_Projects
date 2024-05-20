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

namespace Uno_Game;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    List<string> deck = new List<string>();
    int remainingCardsInPile;
    string[] colors = ["green", "red", "blue", "yellow"];
    string currentColor;
    string currentNumber;
    string setCard;
    bool isGameActive = false;
    bool isClockWise;
    Player activePlayer;
    Player playerOne;
    Player playerTwo;
    Player playerThree;
    Player playerFour;
    //StackPanel activePlayer;
    List<string> player2Deck = new List<string>();
    List<string> player3Deck = new List<string>();
    List<string> player4Deck = new List<string>();

    public MainWindow()
    {
        InitializeComponent();

        InitializeDeck();

        GameBoard.DataContext = this;

        PlayerSelectionWindow.PlayerNumberSelected += HandlePlayerNumberSelected;
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

    public string SetCard
    {
        get => setCard;
        set
        {
            setCard = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SetCard)));
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

    private void HandlePlayerNumberSelected(object? sender, PlayerSelectionEventArgs e)
    {
        playerOne = new Player("Player 1", Player1Space, null);
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
                Content = new Image() { Source = new BitmapImage(new Uri($@"\Resources\{deck[remainingCardsInPile - 1]}.png", UriKind.Relative)) }
            };
            card.Click += PlayerTurn;
            playerOne.Hand.Children.Add(card);
            remainingCardsInPile -= 1;
        }

        switch (e.PlayerMode)
        {
            case NumOfPlayers.Two:
                playerTwo = new Player("Player 2", Player2Space, player2Deck);
                CardsDeal(playerTwo);
                break;
            case NumOfPlayers.Three:
                playerTwo = new Player("Player 2", Player2Space, player2Deck);
                playerThree = new Player("Player 3", Player3Space, player3Deck);
                CardsDeal(playerTwo);
                CardsDeal(playerThree);
                break;
            case NumOfPlayers.Four:
                playerTwo = new Player("Player 2", Player2Space, player2Deck);
                playerThree = new Player("Player 3", Player3Space, player3Deck);
                playerFour = new Player("Player 4", Player4Space, player4Deck);
                CardsDeal(playerTwo);
                CardsDeal(playerThree);
                CardsDeal(playerFour);
                break;
        }
        activePlayer = playerTwo;

        setCard = deck[remainingCardsInPile - 1];
        UpCard.Source = new BitmapImage(new Uri($@"\Resources\{setCard}.png", UriKind.Relative));
        remainingCardsInPile -= 1;
        CurrentColor = FindCurrentColor(SetCard);
        CurrentNumber = FindCurrentNumber(SetCard);
        PlayerSelectionWindow.Visibility = Visibility.Collapsed;

        GameStart();
    }

    private string FindCurrentColor(string setCard)
    {
        int underscoreIndex = setCard.IndexOf('_');
        string color = SetCard.Substring(underscoreIndex + 1);
        return color[0].ToString().ToUpper() + color.Substring(1);
    }
    private string FindCurrentNumber(string setCard)
    {
        int underscoreIndex = setCard.IndexOf('_');
        return SetCard.Substring(0, underscoreIndex);
    }

    private void CardsDeal(Player player)
    {
        for (int i = 0; i < 7; i++)
        {
            Image cardImage = new Image()
            {
                Uid = deck[remainingCardsInPile - 1],
                Source = new BitmapImage(new Uri(@"\Resources\card_back.png", UriKind.Relative)),
                Height = 100,
                Width = 50,
                Margin = new Thickness(0, 0, -15, 0),
            };
            player.Hand.Children.Add(cardImage);
            player.Deck.Add(deck[remainingCardsInPile - 1]);
            remainingCardsInPile -= 1;
        }
    }

    private void InitializeDeck()
    {
        for (int i = 0; i < 4; i++)
        {
            deck.Add($"0_{colors[i]}");
            deck.Add($"+2_{colors[i]}");
            deck.Add($"+2_{colors[i]}");
            deck.Add($"skip_{colors[i]}");
            deck.Add($"skip_{colors[i]}");
            deck.Add($"reverse_{colors[i]}");
            deck.Add($"reverse_{colors[i]}");
            deck.Add($"+4_black");
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

    private void PlayerTurn(object sender, RoutedEventArgs e)
    {
        if (!isGameActive || activePlayer != playerOne)
        {
            return;
        }

        Button button = sender as Button;
        if (!button.Uid.Contains(CurrentColor.ToLower()) && !button.Uid.Contains(CurrentNumber))
        {
            return;
        }
        Image buttonImage = (Image)button.Content;
        UpCard.Source = buttonImage.Source;
        SetCard = button.Uid;
        CurrentColor = FindCurrentColor(SetCard);
        CurrentNumber = FindCurrentNumber(SetCard);
        Player1Space.Children.Remove(button);

        ActivePlayer = isClockWise ? playerFour : playerThree;
        if (ActivePlayer != playerOne)
        {
            ComputerMove(ActivePlayer);
        }
    }

    private void GameStart()
    {
        isGameActive = true;
        isClockWise = true;
        ActivePlayer = playerTwo;
        ComputerMove(ActivePlayer);
    }

    private void ComputerMove(Player player)
    {
        DispatcherTimer timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(5)
        };
        timer.Tick += (sender, e) =>
        {
            timer.Stop();

            var card = player.Deck.Find(c => c.Contains(CurrentColor.ToLower()) || c.Contains(CurrentNumber));

            if (card != null)
            {
                UpCard.Source = new BitmapImage(new Uri($@"\Resources\{card}.png", UriKind.Relative));
                SetCard = card;
                CurrentColor = FindCurrentColor(SetCard);
                CurrentNumber = FindCurrentNumber(SetCard);
                player.Hand.Children.RemoveAt(player.Hand.Children.Count - 1);
                player.Deck.Remove(card);
            }
            else
            {
                Image cardImage = new Image()
                {
                    Uid = deck[remainingCardsInPile - 1],
                    Source = new BitmapImage(new Uri(@"\Resources\card_back.png", UriKind.Relative)),
                    Height = 100,
                    Width = 50,
                    Margin = new Thickness(0, 0, -15, 0),
                };
                player.Hand.Children.Add(cardImage);
                player.Deck.Add(deck[remainingCardsInPile - 1]);
                remainingCardsInPile -= 1;
            }

            switch (ActivePlayer.Name)
            {
                case "Player 2":
                    ActivePlayer = isClockWise ? playerThree : playerFour;
                    break;
                case "Player 3":
                    ActivePlayer = isClockWise ? playerOne : playerTwo;
                    break;
                case "Player 4":
                    ActivePlayer = isClockWise ? playerTwo : playerOne;
                    break;
            }

            if (ActivePlayer != playerOne)
            {
                ComputerMove(ActivePlayer);
            }
        };
        timer.Start();
    }
}