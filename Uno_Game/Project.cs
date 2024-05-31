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
        string desc = "Introducing our Uno game WPF app: a straightforward and enjoyable way to play Uno on your computer! With easy-to-understand controls and a clean interface, you can jump right into the action. Play against computer opponents, all while enjoying the classic Uno experience in a digital format. It's the perfect way to unwind and have fun anytime, anywhere!";
        public string Name { get; set; } = "Uno Card Game";
        public BitmapImage Image => new BitmapImage(new Uri("\\Resources\\uno_logo.png", UriKind.Relative));
        public string Description { get => desc; }
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
