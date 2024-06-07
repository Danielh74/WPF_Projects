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
        public AboutMeSection()
        {
            InitializeComponent();
        }
        private void Back_Click(object sender, MouseButtonEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
