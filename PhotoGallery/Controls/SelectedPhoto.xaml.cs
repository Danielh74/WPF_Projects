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

namespace PhotoGallery.Controls
{
    /// <summary>
    /// Interaction logic for SelectedPhoto.xaml
    /// </summary>
    public partial class SelectedPhoto : UserControl
    {
        public event EventHandler PhotoFavorited;
        public event EventHandler PhotoDeleted;


        public SelectedPhoto()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void OnPhotoFavorited(object sender, RoutedEventArgs e)
        {
            PhotoFavorited?.Invoke(this, EventArgs.Empty);
        }

        private void OnPhotoDeleted(object sender, RoutedEventArgs e)
        {
            PhotoDeleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
