using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Tic_Tac_Toe
{
    public class Project : IProjectMeta
    {
        string desc = "Introducing our Tic Tac Toe WPF app: a classic game brought to life on your computer!\nEnjoy simple, intuitive gameplay with easy-to-use controls.\nChallenge a friend or play against the computer for a quick dose of fun or even watch the computer play against itself.\nWith the leaderboard system you can see who wins the most games and is the strategic king!.\nIt's the perfect way to pass the time and test your strategic skills!";
        public string Name { get; set; } = "Tic-Tac-Toe";
        public BitmapImage Image => new BitmapImage(new Uri("\\Resources\\tic-tac-toe.png", UriKind.Relative));
        public string Description { get=> desc; }
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
