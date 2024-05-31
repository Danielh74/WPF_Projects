using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Pokedex
{
    public class Project : IProjectMeta
    {
        string desc = "Discover our Pokédex WPF app: your ultimate digital guide to Pokémon!\nEasily browse and explore detailed information on all your favorite Pokémon with a clean, user-friendly interface.\nJust scroll through our immense stock of Pokemon and choose whatever you choose to see everything from stats to evolutions.\nPerfect for fans and trainers of all ages!";
        public string Name { get; set; } = "Pokédex";
        public BitmapImage Image => new BitmapImage(new Uri("\\Resources\\pokedex.png", UriKind.Relative));
        public string Description { get => desc; }
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
