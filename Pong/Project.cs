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
        string desc = "";
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
