using Microsoft.Win32;
using PhotoGallery.Controls;
using System.Collections;
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

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        List<Photo> photoList;
        Image selectedImage;

        public MainWindow()
        {
            InitializeComponent();

            SelectedPhotoControl.Visibility = Visibility.Collapsed;

            SelectedPhotoControl.PhotoLiked += HandlePhotoLiked;
            
            LoadPhotos();
        }

        private void HandlePhotoLiked(object? sender, EventArgs e)
        {
            foreach (Photo photo in photoList)
            {
                if(photo.Uri == selectedImage.Source.ToString())
                {
                    photo.Liked = !photo.Liked;

                    SetLikeButton(photo.Liked);

                    break;
                }
            }

            string ListToJson = JsonSerializer.Serialize(photoList, options);
            File.WriteAllText("PhotosInvantory.json", ListToJson);
        }

        public void LoadPhotos()
        {
            GalleryPanel.Children.Clear();
            string rawJson = File.ReadAllText("PhotosInvantory.json");
            photoList = JsonSerializer.Deserialize<List<Photo>>(rawJson);

            foreach (Photo photo in photoList)
            {
                BitmapImage image = new BitmapImage(new Uri(photo.Uri));
                Image photoToDisplay = new Image
                {
                    Source = image,
                    MaxHeight = 150,
                    MaxWidth = 150,
                    Margin = new Thickness(5),
                    Stretch = Stretch.UniformToFill,
                };
                photoToDisplay.MouseEnter += (sender, e) => { photoToDisplay.Opacity = 0.5; };
                photoToDisplay.MouseLeave += (sender, e) => { photoToDisplay.Opacity = 1; };
                photoToDisplay.MouseLeftButtonDown += PhotoClick;
                GalleryPanel.Children.Add(photoToDisplay);
            }
        }

        private void PhotoClick(object sender, MouseButtonEventArgs e)
        {
            
            selectedImage = (Image)sender;
            foreach (Photo photo in photoList)
            {
                if (photo.Uri == selectedImage.Source.ToString())
                {
                    SetLikeButton(photo.Liked);
                }
            }

            SelectedPhotoControl.DataContext = selectedImage;
            SelectedPhotoControl.Visibility = Visibility.Visible;
        }

        private void AddToGallery_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == false)
            {
                return;
            }
            Photo photoToAdd = new Photo()
            {
                Uri = fileDialog.FileName,
                Liked = false
            };

            if (IsVideoFormat(photoToAdd.Uri))
            {
                MessageBox.Show("Photo is already in the gallery");
                return;
            }

            if (photoToAdd.Uri.Contains(".mp4"))
            {
                MessageBox.Show("Cannot add videos to gallery");
                return;
            }

            photoList.Add(photoToAdd);

            string ListToJson = JsonSerializer.Serialize(photoList, options);
            File.WriteAllText("PhotosInvantory.json", ListToJson);
            LoadPhotos();

        }

        private bool IsVideoFormat(string uri)
        {
            string[] videoExtensions = [".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm"];

            string mediaToCheck = System.IO.Path.GetExtension(uri);
            if (mediaToCheck != null && Array.IndexOf(videoExtensions, mediaToCheck.ToLower()) == -1)
            {
                return false;
            }
            return true;
        }

        private void SetLikeButton(bool isLiked)
        {
            if (isLiked)
            {
                SelectedPhotoControl.LikeButton.Source = new BitmapImage(new Uri(@"\Resources\full_heart.png", UriKind.Relative));
            }
            else
            {
                SelectedPhotoControl.LikeButton.Source = new BitmapImage(new Uri(@"\Resources\empty_heart.png", UriKind.Relative));

            }
        }
    }


}