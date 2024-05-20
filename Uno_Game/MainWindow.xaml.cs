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
    string setCard;
    bool isGameActive = false;
    bool isClockWise;
    StackPanel activePlayer;
    string[] player2Deck = new string[7];
    string[] player3Deck = new string[7];
    string[] player4Deck = new string[7];

    public MainWindow()
    {
        InitializeComponent();

        InitializeDeck();

        GameBoard.DataContext = this;

        PlayerSelectionWindow.PlayerNumberSelected += HandlePlayerNumberSelected;
        PropertyChanged += HandleActivePlayerChanged;

        GameStart();
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
    public string SetCard
    {
        get => setCard;
        set
        {
            setCard = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SetCard)));
        }
    }

    public StackPanel ActivePlayer
    {
        get => activePlayer;
        set
        {
            activePlayer = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPlayerTurn)));
        }
    }
    public string CurrentPlayerTurn =>
            ActivePlayer == Player1Space ? $"Player 1"
            : ActivePlayer == Player2Space ? $"Player 2"
            : ActivePlayer == Player3Space ? $"Player 3"
            : ActivePlayer == Player4Space ? $"Player 4"
            : "";
   
    private void HandleActivePlayerChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (ActivePlayer.Name)
        {
            case "Player1Space":

                break;
            case "Player2Space":

                break;
            case "Player3Space":

                break;
            case "Player4Space":

                break;
        }
    }

    private void HandlePlayerNumberSelected(object? sender, PlayerSelectionEventArgs e)
    {
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
            Player1Space.Children.Add(card);
            remainingCardsInPile -= 1;
        }

        switch (e.PlayerMode)
        {
            case NumOfPlayers.Two:
                SetPlayerSpace("player2");
                break;
            case NumOfPlayers.Three:
                SetPlayerSpace("player2");
                SetPlayerSpace("player3");
                break;
            case NumOfPlayers.Four:
                SetPlayerSpace("player2");
                SetPlayerSpace("player3");
                SetPlayerSpace("player4");
                break;
        }
        setCard = deck[remainingCardsInPile - 1];
        UpCard.Source = new BitmapImage(new Uri($@"\Resources\{setCard}.png", UriKind.Relative));
        remainingCardsInPile -= 1;
        CurrentColor = FindCurrentColor(SetCard);
        PlayerSelectionWindow.Visibility = Visibility.Collapsed;
    }

    private string FindCurrentColor(string setCard)
    {
        int underscoreIndex = setCard.IndexOf('_');
        string color = SetCard.Substring(underscoreIndex + 1);
        return color[0].ToString().ToUpper() + color.Substring(1); ;
    }

    private void CardsDeal(StackPanel playerHand, string[] playerDeck)
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
            playerHand.Children.Add(cardImage);

            playerDeck[i] = deck[remainingCardsInPile - 1];
            remainingCardsInPile -= 1;
        }
    }
    private void SetPlayerSpace(string player)
    {
        switch (player)
        {
            case "player2":
                CardsDeal(Player2Space, player2Deck);
                break;
            case "player3":
                CardsDeal(Player3Space, player3Deck);
                break;
            case "player4":
                CardsDeal(Player4Space, player4Deck);
                break;
            default:
                break;
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
        if (!isGameActive || activePlayer != Player1Space)
        {
            return;
        }

        Button button = sender as Button;
        if (!button.Uid.Contains(CurrentColor.ToLower()))
        {
            return;
        }
        Image buttonImage = (Image)button.Content;
        UpCard.Source = buttonImage.Source;
        Player1Space.Children.Remove(button);
        ActivePlayer = isClockWise ? Player4Space : Player3Space;
        if (ActivePlayer != Player1Space)
        {
            ComputerMove(ActivePlayer);
        }
    }

    private void GameStart()
    {
        isGameActive = true;
        isClockWise = true;
        ActivePlayer = Player2Space;
        if (ActivePlayer != Player1Space)
        {
            ComputerMove(ActivePlayer);
        }
    }

    private void ComputerMove(StackPanel playersPanel)
    {
        DispatcherTimer timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(5)
        };
        timer.Tick += (sender, e) =>
        {
            timer.Stop();

            foreach (Image card in playersPanel.Children.OfType<Image>())
            {
                if (card.Uid.Contains(CurrentColor.ToLower()))
                {
                    UpCard.Source = new BitmapImage(new Uri($@"\Resources\{card.Uid}.png", UriKind.Relative));
                    playersPanel.Children.Remove(card);
                    break;
                }
            }

            switch (ActivePlayer.Name)
            {
                case "Player2Space":
                    ActivePlayer = isClockWise ? Player3Space : Player4Space;
                    break;
                case "Player3Space":
                    ActivePlayer = isClockWise ? Player1Space : Player2Space;
                    break;
                case "Player4Space":
                    ActivePlayer = isClockWise ? Player2Space : Player1Space;
                    break;
            }

            if (ActivePlayer != Player1Space)
            {
                ComputerMove(ActivePlayer);
            }
        };
        timer.Start();
    }
}