using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Pong
{
    public class Project : IProjectMeta
    {
        string desc = "Enjoy the classic arcade fun with our Retro Pong Game WPF app!\nPlay the original Pong with smooth controls and retro graphics.\nPlay against a friend with one keyboard, one with the W/S keys and the other with the Up/Down keys to control the paddles\nPerfect for reliving nostalgia or experiencing Pong for the first time!";
        public string Name { get; set; } = "Pong";
        public BitmapImage Image => new BitmapImage(new Uri("\\Resources\\pong.png", UriKind.Relative));
        public string Description { get => desc; }
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
