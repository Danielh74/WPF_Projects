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

namespace Pong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HashSet<Key> pressedKeys = new HashSet<Key>();
        private double playerSpeed = 3;
        private double ballSpeedX = 3;
        private double ballSpeedY = 3;
        public MainWindow()
        {
            InitializeComponent();

            SizeChanged += HandleWindowSizeChanged;

            startScreen.GameStarted += GameStart;
        }

        private void GameStart(object? sender, EventArgs e)
        {
            startScreen.Visibility = Visibility.Collapsed;

            CompositionTarget.Rendering += OnRendering;

            ResetScreen();

            KeyDown += HandleKeyDown;
            KeyUp += HandleKeyUp;
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            pressedKeys.Add(e.Key);
        }

        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys.Remove(e.Key);
        }

        private void OnRendering(object? sender, EventArgs e)
        {
            MoveBall();
            CheckPlayerMovement();
        }

        private void CheckPlayerMovement()
        {
            if (pressedKeys.Contains(Key.W) && Canvas.GetTop(player1) > 0)
                Canvas.SetTop(player1, Canvas.GetTop(player1) - playerSpeed);

            if (pressedKeys.Contains(Key.S) && Canvas.GetTop(player1) < (gameScreen.ActualHeight - player1.Height))
                Canvas.SetTop(player1, Canvas.GetTop(player1) + playerSpeed);

            if (pressedKeys.Contains(Key.Up) && Canvas.GetTop(player2) > 0)
                Canvas.SetTop(player2, Canvas.GetTop(player2) - playerSpeed);

            if (pressedKeys.Contains(Key.Down) && Canvas.GetTop(player2) < (gameScreen.ActualHeight - player2.Height))
                Canvas.SetTop(player2, Canvas.GetTop(player2) + playerSpeed);
        }
        private void MoveBall()
        {
            Canvas.SetTop(ball, Canvas.GetTop(ball) + ballSpeedY);
            Canvas.SetLeft(ball, Canvas.GetLeft(ball) + ballSpeedX);

            if (Canvas.GetTop(ball) <= 0 || Canvas.GetTop(ball) >= gameScreen.ActualHeight - ball.Height)
            {
                ballSpeedY = -ballSpeedY;
            }

            if (Canvas.GetLeft(ball) <= Canvas.GetLeft(player1) + player1.Width &&
                Canvas.GetTop(ball) + ball.Height >= Canvas.GetTop(player1) &&
                Canvas.GetTop(ball) <= Canvas.GetTop(player1) + player1.Height)
            {
                ballSpeedX = -ballSpeedX;
                Canvas.SetLeft(ball, Canvas.GetLeft(player1) + player1.Width);
            }

            if (Canvas.GetLeft(ball) + ball.Width >= Canvas.GetLeft(player2) &&
                Canvas.GetTop(ball) + ball.Height >= Canvas.GetTop(player2) &&
                Canvas.GetTop(ball) <= Canvas.GetTop(player2) + player2.Height)
            {
                ballSpeedX = -ballSpeedX;
                Canvas.SetLeft(ball, Canvas.GetLeft(player2) - ball.Width);
            }

            if (Canvas.GetLeft(ball) < 0 || Canvas.GetLeft(ball) + ball.Width > gameScreen.ActualWidth)
            {
                ResetBall();
            }
        }

        private void ResetScreen()
        {
            Canvas.SetLeft(player1, 20);
            Canvas.SetTop(player1, (gameScreen.ActualHeight - player1.Height) / 2);

            Canvas.SetLeft(player2, gameScreen.ActualWidth - (player2.Width + 20));
            Canvas.SetTop(player2, (gameScreen.ActualHeight - player2.Height) / 2);

            ResetBall();
        }

        private void ResetBall()
        {
            Canvas.SetLeft(ball, (gameScreen.ActualWidth - ball.Width) / 2);
            Canvas.SetTop(ball, (gameScreen.ActualHeight - ball.Height) / 2);
        }

        private void HandleWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResetScreen();
        }
    }
}