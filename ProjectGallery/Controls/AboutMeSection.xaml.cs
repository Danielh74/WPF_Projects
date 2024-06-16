using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        string aboutMe = "Hello and welcome to my WPF showcase project! My name is Daniel Hazan, and I'm currently honing my skills as a full-stack developer at HuckerU College. I have a passion for crafting innovative solutions and aspire to create top-notch products in the future.\r\n\r\nIn this final WPF project, you'll discover 8 mini-projects that highlight my journey and skills in WPF development. Each project showcases different aspects of WPF, from UI design to functionality, aiming to provide a glimpse into my growth and capabilities as a developer.\r\n\r\nThank you for taking the time to visit! If you're interested in learning more or have any questions, please feel free to contact me using the information below. I look forward to connecting with you!";
        public AboutMeSection()
        {
            InitializeComponent();
            DataContext = aboutMe;
        }
        private void BackToHome(object sender, MouseButtonEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void NavigateToContact(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true});
            e.Handled = true;
        }
    }
}
