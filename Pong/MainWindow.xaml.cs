﻿using System.ComponentModel;
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
using System.Xml.Linq;

namespace Pong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private HashSet<Key> pressedKeys = new HashSet<Key>();
        private double playerSpeed = 3;
        private double ballSpeedX = 4;
        private double ballSpeedY = 4;
        private int playerOneScore;
        private int playerTwoScore;


        public MainWindow()
        {
            InitializeComponent();

            SizeChanged += HandleWindowSizeChanged;

            startScreen.GameStarted += GameStart;

            scorePanel.DataContext = this;
        }

        public int PlayerOneScore 
        { 
            get => playerOneScore;
            set
            {
                playerOneScore = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlayerOneScore)));
            }
        }
        public int PlayerTwoScore
        {
            get => playerTwoScore;
            set
            {
                playerTwoScore = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlayerTwoScore)));
            }
        }

        private void GameStart(object? sender, EventArgs e)
        {
            startScreen.Visibility = Visibility.Collapsed;
            PlayerOneScore = 0;
            PlayerTwoScore = 0;
            Canvas.SetLeft(scorePanel, (gameScreen.ActualWidth - scorePanel.ActualWidth) / 2);
            CompositionTarget.Rendering += OnRendering;

            ResetScreen();

            KeyDown += HandleKeyDown;
            KeyUp += HandleKeyUp;
        }

        //Adding which key is pressed to the hashset list to keep track of the keys that are pressed simultaneously
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            pressedKeys.Add(e.Key);
        }

        //Removing which key is unpressed to the hashset list to keep track of the keys that are pressed
        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys.Remove(e.Key);
        }

        //Moving the ball and the players if the keys are pressed every frame render.
        private void OnRendering(object? sender, EventArgs e)
        {
            MoveBall();
            CheckPlayerMovement();
        }

        //Checking which key is pressed to move the corresponding player.
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

            if (Canvas.GetLeft(ball) < 0)
            {
                PlayerTwoScore++;
                ResetBall();
            }

            if (Canvas.GetLeft(ball) + ball.Width > gameScreen.ActualWidth)
            {
                PlayerOneScore++;
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

        //Reset the screen and position the elements correctly when the screen changes size.
        private void HandleWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResetScreen();
        }

        private void ExitGame(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RestartGame(object sender, RoutedEventArgs e)
        {
            ResetScreen();
            PlayerOneScore = 0;
            PlayerTwoScore = 0;
        }
    }
}