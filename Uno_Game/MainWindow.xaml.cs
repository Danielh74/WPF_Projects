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
                SetPlayerSpace(180,  new Thickness(0, 0, -15, 0),Player2Space);
                break;
            case NumOfPlayers.Three:
                SetPlayerSpace(180, new Thickness(0, 0, -15, 0), Player2Space);
                SetPlayerSpace( 270, new Thickness(0, -65, 0, 0), Player3Space);
                break;
            case NumOfPlayers.Four:
                SetPlayerSpace(180, new Thickness(0, 0, -15, 0), Player2Space);
                SetPlayerSpace(270, new Thickness(0, 0, 0, -65), Player3Space);
                SetPlayerSpace(90, new Thickness(0, -65, 0, 0), Player4Space);
                break;
        }
        PlayerSelectionWindow.Visibility = Visibility.Collapsed;
    }
    private void SetPlayerSpace(int rotation, Thickness thickness, StackPanel playerSpace)
    {
        for (int j = 0; j < 7; j++)
        {
            Image cardImage = new Image()
            {
                Source = new BitmapImage(new Uri(@"\Resources\card_back.png", UriKind.Relative)),
                Height = 100,
                Width = 50,
                Margin = thickness,
                RenderTransform = new RotateTransform(rotation)
            };
            playerSpace.Children.Add(cardImage);
        }
    }
}