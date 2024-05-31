using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Common;

namespace Memory_Game
{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "Memory Game";
        public BitmapImage Image => new BitmapImage(new Uri("\\Resources\\pokemon_memory.png", UriKind.Relative));
        public string Description { get; set; } = "";
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
