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
        public string Name { get; set; } = "Joke App";
        public BitmapImage Image => new BitmapImage(new Uri("\\Resources\\joke.png", UriKind.Relative));
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
