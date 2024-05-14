using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Uno_Game
{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "Uno Card Game";

        public BitmapImage Image => new BitmapImage(new Uri("\\Resources\\uno_logo.png", UriKind.Relative));

        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
