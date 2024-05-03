using PhotoGallery.Controls;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoGallery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Photo> PhotoGallery;
        public MainWindow()
        {
            InitializeComponent();

            SelectedPhotoControl.Visibility = Visibility.Collapsed;

            LoadPhotos();
        }

        private void LoadPhotos()
        {
            string rawJson = File.ReadAllText("PhotosInvantory.json");
            PhotoGallery = JsonSerializer.Deserialize<List<Photo>>(rawJson);

            foreach (Photo photo in PhotoGallery)
            {
                BitmapImage image = new BitmapImage(new Uri(photo.Uri));
                Image photoToDisplay = new Image 
                { 
                    Source = image,
                    MaxHeight = 150,
                    MaxWidth = 150,
                    Margin = new Thickness(5),
                    Stretch =  Stretch.UniformToFill,
                };
                photoToDisplay.MouseLeftButtonDown += OnMouseLeftButtonDown;
                PhotosPanel.Children.Add(photoToDisplay);
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //PhotosPanel.Children.Clear();
            Image selectedImage = (Image)sender;
            SelectedPhotoControl.DataContext = selectedImage;
            SelectedPhotoControl.Visibility = Visibility.Visible;
        }
    }
    public class Photo
    {
        public string Uri { get; set; }
        public bool Liked { get; set; }
    }
}