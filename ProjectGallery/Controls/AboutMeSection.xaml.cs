using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectGallery.Controls
{
    /// <summary>
    /// Interaction logic for AboutMeSection.xaml
    /// </summary>
    public partial class AboutMeSection : UserControl
    {
        string aboutMe = "Hello! I'm Daniel Hazan, a FullStack development student at HackerU.\nI'm passionate about creating seamless web applications using technologies like JavaScript, HTML, CSS, Node.js, and React.\nAdditionally, I have experience building WPF applications.\nAt HackerU, I've developed my skills through hands-on projects and rigorous coursework.\nI'm driven by a love for innovation and continuous learning, and I enjoy collaborating on tech projects.\nThis is my WPF project, feel free to check it out!";

        public AboutMeSection()
        {
            InitializeComponent();
            DataContext = aboutMe;
        }
        private void Back_Click(object sender, MouseButtonEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
