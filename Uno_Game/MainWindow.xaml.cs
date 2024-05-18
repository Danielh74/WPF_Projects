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
using Uno_Game.CustomEventArgs;
using Uno_Game.Enums;

namespace Uno_Game;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        PlayerSelectionWindow.PlayerNumberSelected += HandlePlayerNumberSelected;
    }

    private void HandlePlayerNumberSelected(object? sender, PlayerSelectionEventArgs e)
    {
        int[,] places =
            {
            { 2, 1 },
            { 0, 1 },
            { 1, 2 },
            { 1, 0 }
        };

        for (int j = 0; j < 7; j++)
        {
            Button card = new Button()
            {
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, -40, 0),
                BorderBrush = new SolidColorBrush(Colors.Transparent),
                Height = 125,
                Width = 75,
                Content = new Image() { Source = new BitmapImage(new Uri(@"\Resources\1_blue.png", UriKind.Relative)) }
            };
            Player1Space.Children.Add(card);
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
        PlayerSelectionWindow.Visibility = Visibility.Collapsed;
    }
    private void SetPlayerSpace(string player)
    {
        

        switch (player)
        {
            case "player2":
                for (int i = 0; i < 7; i++)
                {
                    Image cardImage = new Image()
                    {
                        Source = new BitmapImage(new Uri(@"\Resources\card_back.png", UriKind.Relative)),
                        Height = 100,
                        Width = 50,
                        Margin = new Thickness(0, 0, -15, 0),
                    };
                    cardImage.RenderTransformOrigin = new Point(0.5, 0.5);
                    ScaleTransform flipTrans = new ScaleTransform();
                    flipTrans.ScaleX = -1;
                    flipTrans.ScaleY = -1;
                    cardImage.RenderTransform = flipTrans;
                    Player2Space.Children.Add(cardImage);
                }
                break;
            case "player3":
                for (int i = 0; i < 7; i++)
                {
                    Image cardImage = new Image()
                    {
                        Source = new BitmapImage(new Uri(@"\Resources\card_back.png", UriKind.Relative)),
                        Height = 100,
                        Width = 50,
                        Margin = new Thickness(0, -65, 0, 0),
                        RenderTransformOrigin = new Point(0.5, 0.5),
                        RenderTransform = new RotateTransform(270)
                    };
                    Player3Space.Children.Add(cardImage);
                }
                break;
            case "player4":
                for (int i = 0; i < 7; i++)
                {
                    Image cardImage = new Image()
                    {
                        Source = new BitmapImage(new Uri(@"\Resources\card_back.png", UriKind.Relative)),
                        Height = 100,
                        Width = 50,
                        Margin = new Thickness(0, -65, 0, 0),
                        RenderTransformOrigin = new Point(0.5, 0.5),
                        RenderTransform = new RotateTransform(90)
                    };
                    Player4Space.Children.Add(cardImage);
                }
                break;
            default:
                break;
        }
        
    }
}