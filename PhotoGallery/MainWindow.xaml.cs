using Microsoft.Win32;
using PhotoGallery.Controls;
using PhotoGallery.CustomEventArgs;
using PhotoGallery.Models;
using PhotoGallery.Utils;
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
        User currentUser;
        int currentUserIndex;
        List<User> userList;
        Image selectedImage;
        WindowType currentType;

        public MainWindow()
        {
            InitializeComponent();

            SelectedPhotoControl.Visibility = Visibility.Collapsed;
            RegisterPageControl.Visibility = Visibility.Collapsed;
            MainPanel.Visibility = Visibility.Collapsed;

            SelectedPhotoControl.PhotoFavorited += HandlePhotoFavorited;
            SelectedPhotoControl.PhotoDeleted += HandlePhotoDeleted;

            LoginPageControl.UserLoggedIn += HandleUserLogin;
            LoginPageControl.ChangingForm += HandleFormChange;

            RegisterPageControl.UserRegistered += HandleUserLogin;
            RegisterPageControl.ChangingForm += HandleFormChange;

            currentType = WindowType.Home;
        }

        private void HandleUserLogin(object? sender, LoginEventArgs e)
        {
            LoadUsers();

            currentUser = userList.Find(user => user.Email == e.Email && user.Password == e.Password);
            if (currentUser != null)
            {
                LoginPageControl.Visibility = Visibility.Collapsed;
                RegisterPageControl.Visibility = Visibility.Collapsed;
                GalleryPanel.Visibility = Visibility.Visible;
                currentUserIndex = userList.IndexOf(currentUser);
                MainPanel.Visibility = Visibility.Visible;
                LoadPhotos("home");
            }
            else
            {
                return;
            }
        }

        private void HandleFormChange(object? sender, EventArgs e)
        {
            if (RegisterPageControl.Visibility == Visibility.Visible)
            {
                RegisterPageControl.Visibility = Visibility.Collapsed;
                LoginPageControl.Visibility = Visibility.Visible;
            }
            else
            {
                RegisterPageControl.Visibility = Visibility.Visible;
                LoginPageControl.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadUsers()
        {
            string rawJson = File.ReadAllText("PhotosInvantory.json");
            userList = JsonSerializer.Deserialize<List<User>>(rawJson);
        }

        public void LoadPhotos(string option)
        {
            GalleryPanel.Children.Clear();

            switch (option)
            {
                case "home":
                    foreach (Photo photo in currentUser.Gallery)
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

                    if (GalleryPanel.Children.Count == 0)
                    {
                        TextBlock emptyGalleryMessage = new TextBlock()
                        {
                            Text = "Your gallery is empty"
                        };

                        GalleryPanel.Children.Add(emptyGalleryMessage);
                    }

                    break;

                case "favorites":
                    foreach (Photo photo in currentUser.Gallery)
                    {
                        if (photo.IsFavorite)
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
                    if (GalleryPanel.Children.Count == 0)
                    {
                        TextBlock noFavMessage = new TextBlock()
                        {
                            Text = "No favorite photos"
                        };

                        GalleryPanel.Children.Add(noFavMessage);
                    }
                    break;

                case "about":
                    StackPanel stackPanel = new StackPanel()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    TextBlock textBlock = new TextBlock()
                    {
                        Text = "This is the about section"
                    };
                    stackPanel.Children.Add(textBlock);
                    GalleryPanel.Children.Add(stackPanel);
                    break;

                default:
                    break;
            }
        }

        private void PhotoClick(object sender, MouseButtonEventArgs e)
        {
            selectedImage = (Image)sender;

            string selectedImageSafeUri = Helpers.GetSafeFileName(selectedImage.Source.ToString());

            foreach (Photo photo in currentUser.Gallery)
            {
                if (photo.SafeUri == selectedImageSafeUri)
                {
                    SelectedPhotoControl.LikeButton.Source = Helpers.SetLikeButton(photo.IsFavorite);
                    break;
                }
            }

            SelectedPhotoControl.DataContext = selectedImage;
            SelectedPhotoControl.Visibility = Visibility.Visible;
        }

        private void HandlePhotoFavorited(object? sender, EventArgs e)
        {
            string selectedImageSafeUri = Helpers.GetSafeFileName(selectedImage.Source.ToString());
            foreach (Photo photo in currentUser.Gallery)
            {
                if (photo.SafeUri == selectedImageSafeUri)
                {
                    photo.IsFavorite = !photo.IsFavorite;

                    SelectedPhotoControl.LikeButton.Source = Helpers.SetLikeButton(photo.IsFavorite);

                    break;
                }
            }
            userList[currentUserIndex].Gallery = currentUser.Gallery;
            string ListToJson = JsonSerializer.Serialize(userList, options);
            File.WriteAllText("PhotosInvantory.json", ListToJson);

            if (currentType == WindowType.Favorites)
            {
                LoadPhotos("favorites");
            }
        }

        private void HandlePhotoDeleted(object? sender, EventArgs e)
        {
            string selectedImageSafeUri = Helpers.GetSafeFileName(selectedImage.Source.ToString());
            foreach (Photo photo in currentUser.Gallery)
            {
                if (photo.SafeUri == selectedImageSafeUri)
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete?", "Delete Photo", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        currentUser.Gallery.Remove(photo);

                        SelectedPhotoControl.Visibility = Visibility.Collapsed;

                        userList[currentUserIndex].Gallery = currentUser.Gallery;
                        string ListToJson = JsonSerializer.Serialize(userList, options);
                        File.WriteAllText("PhotosInvantory.json", ListToJson);

                        if (currentType == WindowType.Favorites)
                        {
                            LoadPhotos("favorites");
                        }
                        else
                        {
                            LoadPhotos("home");
                        }
                    }

                    break;
                }
            }
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
                SafeUri = fileDialog.SafeFileName,
                IsFavorite = false
            };

            if (Helpers.IsVideoFormat(photoToAdd.Uri))
            {
                MessageBox.Show("Cannot add videos to gallery");
                return;
            }

            if (currentUser.Gallery.Any(photo => photo.SafeUri == photoToAdd.SafeUri))
            {
                MessageBox.Show("Photo is already exists in gallery");
                return;
            }

            currentUser.Gallery.Add(photoToAdd);

            userList[currentUserIndex].Gallery = currentUser.Gallery;
            string ListToJson = JsonSerializer.Serialize(userList, options);
            File.WriteAllText("PhotosInvantory.json", ListToJson);

            LoadPhotos("home");

        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            currentType = WindowType.Home;
            LoadPhotos("home");
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            currentType = WindowType.Favorites;
            LoadPhotos("favorites");
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPhotos("about");
        }
    }
}
