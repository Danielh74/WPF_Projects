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
        string desc = "Welcome to our Photo Gallery WPF app: your all-in-one solution for managing and enjoying your photo collection!\nWith a sleek and intuitive interface, users can effortlessly scroll through their personalized photo galleries and add new photos with just a few clicks.\nEach user has their own gallery, allowing for a tailored and organized experience.\nMark your favorite photos for quick access and relive cherished memories anytime.\nNeed to clean up your collection? Deleting unwanted photos is quick and simple.\nOur app combines functionality and ease of use, making it perfect for organizing and viewing your photos with minimal effort.\n New users can register with ease and create their own gallery and existing users just log in to enjoy the app.";
        public string Name { get; set; } = "Photo Gallery";
        public BitmapImage Image => new BitmapImage(new Uri(@"\Resources\photoGallery.png", UriKind.Relative));
        public string Description { get=> desc; }
        public void Run()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();
        }
    }
}
