using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Common;

namespace JokeApp
{
    public class Project : IProjectMeta
    {
        string desc = "Welcome to my Joke app! This is your ultimate companion for instant laughter! This sleek WPF application delivers a seamless experience where humor meets customization.\nWith just one click, generate a joke out of hundreds in our invantory and if the style of humor is not your cup of tea you can change the settings to suit your humor.\nChoose between categories of jokes and even flag triggering and offending topics in the blacklist so you wont need to see them and even choose between single or two part jokes.\nEverything you need to get a good laugh out of you!.";
        public string Name { get; set; } = "Joke App";
        public BitmapImage Image => new BitmapImage(new Uri("\\Resources\\joke.png", UriKind.Relative));
        public string Description { get => desc; }
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
