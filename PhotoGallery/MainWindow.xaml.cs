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
        Photo selectedPhoto;
        string selectedPhotoSafeUri;
        int selectedPhotoIndex;
        WindowType currentType;

        public MainWindow()
        {
            InitializeComponent();

            SelectedPhotoControl.Visibility = Visibility.Collapsed;
            RegisterPageControl.Visibility = Visibility.Collapsed;
            MainPanel.Visibility = Visibility.Collapsed;

            SelectedPhotoControl.PhotoFavorited += HandlePhotoFavorited;
            SelectedPhotoControl.PhotoDeleted += HandlePhotoDeleted;
            SelectedPhotoControl.PhotoChanged += HandlePhotoChanged;

            LoginPageControl.UserLoggedIn += HandleUserLogin;
            LoginPageControl.ChangingForm += HandleFormChange;

            RegisterPageControl.UserRegistered += HandleUserLogin;
            RegisterPageControl.ChangingForm += HandleFormChange;

            currentType = WindowType.Home;
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

        private void HandleUserLogin(object? sender, LoginEventArgs e)
        {
            userList = e.UserList;

            currentUser = userList.Find(user => user.Email == e.Email && user.Password == e.Password);
            if (currentUser != null)
            {
                LoginPageControl.Visibility = Visibility.Collapsed;
                RegisterPageControl.Visibility = Visibility.Collapsed;
                GalleryPanel.Visibility = Visibility.Visible;
                currentUserIndex = userList.IndexOf(currentUser);
                MainPanel.Visibility = Visibility.Visible;
                LoadWindow("home");
            }
            else
            {
                return;
            }
        }

        public void LoadWindow(string option)
        {
            GalleryPanel.Children.Clear();

            switch (option)
            {
                case "home":
                    foreach (Photo photo in currentUser.Gallery)
                    {
                        BitmapImage image = new BitmapImage(new Uri(photo.Uri, UriKind.RelativeOrAbsolute));
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
                            BitmapImage image = new BitmapImage(new Uri(photo.Uri, UriKind.RelativeOrAbsolute));
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
            Image clickedImage = (Image)sender;

            selectedPhotoSafeUri = Helpers.GetSafeFileName(clickedImage.Source.ToString());

            selectedPhotoIndex = currentUser.Gallery.FindIndex(photo => photo.SafeUri == selectedPhotoSafeUri);

            selectedPhoto = currentUser.Gallery[selectedPhotoIndex];

            SelectedPhotoControl.LikeButton.Source = Helpers.SetLikeButton(selectedPhoto.IsFavorite);

            SelectedPhotoControl.DataContext = clickedImage;
            SelectedPhotoControl.Visibility = Visibility.Visible;
        }

        private void HandlePhotoChanged(object? sender, PhotoChangeEventArgs e)
        {
            if (e.Next)
            {
                if (selectedPhotoIndex + 1 >= currentUser.Gallery.Count)
                {
                    return;
                }
                else
                {
                    selectedPhotoIndex += 1;
                }
            }
            else if (e.Previous)
            {
                if (selectedPhotoIndex - 1 < 0)
                {
                    return;
                }
                else
                {
                    selectedPhotoIndex -= 1;
                }
            }

            selectedPhoto = currentUser.Gallery[selectedPhotoIndex];
            SelectedPhotoControl.LikeButton.Source = Helpers.SetLikeButton(selectedPhoto.IsFavorite);
            SelectedPhotoControl.DataContext = GalleryPanel.Children[selectedPhotoIndex];
        }

        private void HandlePhotoFavorited(object? sender, EventArgs e)
        {
            selectedPhoto.IsFavorite = !selectedPhoto.IsFavorite;

            SelectedPhotoControl.LikeButton.Source = Helpers.SetLikeButton(selectedPhoto.IsFavorite);

            if (!selectedPhoto.IsFavorite && currentType == WindowType.Favorites)
            {
                SelectedPhotoControl.Visibility = Visibility.Collapsed;
            }

            userList[currentUserIndex].Gallery = currentUser.Gallery;
            string ListToJson = JsonSerializer.Serialize(userList, options);
            File.WriteAllText("UsersInvantory.json", ListToJson);

            if (currentType == WindowType.Favorites)
            {
                LoadWindow("favorites");
            }
        }

        private void HandlePhotoDeleted(object? sender, EventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete?", "Delete Photo", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                currentUser.Gallery.Remove(selectedPhoto);

                SelectedPhotoControl.Visibility = Visibility.Collapsed;

                userList[currentUserIndex].Gallery = currentUser.Gallery;
                string ListToJson = JsonSerializer.Serialize(userList, options);
                File.WriteAllText("UsersInvantory.json", ListToJson);

                if (currentType == WindowType.Favorites)
                {
                    LoadWindow("favorites");
                }
                else
                {
                    LoadWindow("home");
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
            File.WriteAllText("UsersInvantory.json", ListToJson);

            LoadWindow("home");

        }

        private void ChangeWindow_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "FavBtn":
                    currentType = WindowType.Favorites;
                    LoadWindow("favorites");
                    break;
                case "AboutBtn":
                    LoadWindow("about");
                    break;
                case "LogOutBtn":
                    currentUser = null;
                    MainPanel.Visibility = Visibility.Collapsed;
                    LoginPageControl.Visibility = Visibility.Visible;
                    break;
                default:
                    currentType = WindowType.Home;
                    LoadWindow("home");
                    break;
            }
        }
    }
}