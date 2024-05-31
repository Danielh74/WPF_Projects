using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PersonManager
{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "Project Manager";
        public BitmapImage Image => new BitmapImage(new Uri("\\Resources\\PersonManager.png", UriKind.Relative));
        public string Description { get; set; } = "";
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
