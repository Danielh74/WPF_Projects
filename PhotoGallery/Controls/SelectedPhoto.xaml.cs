﻿using PhotoGallery.CustomEventArgs;
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
        public event EventHandler<PhotoChangeEventArgs> PhotoChanged;


        public SelectedPhoto()
        {
            InitializeComponent();
        }

        private void CloseControl(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        // Raises the event indicating that a photo has been favorited.
        private void OnPhotoFavorited(object sender, RoutedEventArgs e)
        {
            PhotoFavorited?.Invoke(this, EventArgs.Empty);
        }

        // Raises the  event indicating that a photo has been deleted.
        private void OnPhotoDeleted(object sender, RoutedEventArgs e)
        {
            PhotoDeleted?.Invoke(this, EventArgs.Empty);
        }

        // Displaying the next or previous photo according to which button has been pressed.
        private void OnPhotoChanged(object sender, RoutedEventArgs e)
        {
            Button btnClicked = sender as Button;

            switch (btnClicked.Name)
            {
                case "PreviousBtn":
                    PhotoChanged?.Invoke(this, new PhotoChangeEventArgs() { Previous = true, Next = false });
                    break;
                case "NextBtn":
                    PhotoChanged?.Invoke(this, new PhotoChangeEventArgs() { Previous = false, Next = true });
                    break;
                default:
                    break;
            }
        }
    }
}
