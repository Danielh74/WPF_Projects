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
        public string Name { get; set; } = "Tic-Tac-Toe";
        public BitmapImage Image => new BitmapImage(new Uri("\\Resources\\tic-tac-toe.png", UriKind.Relative));

        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
