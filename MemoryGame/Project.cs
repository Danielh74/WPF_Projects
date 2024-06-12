using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Common;

namespace MemoryGame
{
    public class Project : IProjectMeta
    {
        string desc = "Introducing our Pokémon-themed Memory Card Game WPF app: an exciting and fun way to train your brain with your favorite Pokémon characters!\nMatch pairs of cards featuring colorful Pokémon images, and enjoy smooth gameplay with a clean, user-friendly interface.\nPerfect for fans of all ages, our app offers various difficulty levels to challenge your memory skills with options to choose between 12, 16 and 20 cards.\n Relive the magic of Pokémon while sharpening your mind in this engaging and nostalgic memory card game!\nThe game can only be played with 2 players so compete with a friend to see who has the sharpest mind!";
        public string Name { get; set; } = "Memory Game 2";
        public BitmapImage Image => new BitmapImage(new Uri("\\Resources\\pokemon_memory.png", UriKind.Relative));
        public string Description { get => desc; }
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
