using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PhotoGallery
{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "Photo Gallery";

        public BitmapImage Image => new BitmapImage(new Uri(@"\Resources\photoGallery.png", UriKind.Relative));

        public void Run()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();
        }
    }
}
