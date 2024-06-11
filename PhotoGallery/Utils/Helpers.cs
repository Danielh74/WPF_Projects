using PhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoGallery.Utils
{
    public static class Helpers
    {
        public static BitmapImage SetLikeButton(bool isLiked)
        {
            if (isLiked)
            {
                return new BitmapImage(new Uri("../Resources/full_heart.png", UriKind.Relative));
            }
            else
            {
                return new BitmapImage(new Uri("../Resources/empty_heart.png", UriKind.Relative));
            }
        }

        public static string GetSafeFileName(string uri)
        {
            int lastIndex = uri.LastIndexOf('/');
            string safeFileName = uri.Substring(lastIndex + 1);

            return safeFileName;
        }

        public static List<User> LoadUsers()
        {
            string rawJson = File.ReadAllText("UsersInvantory.json");
            return JsonSerializer.Deserialize<List<User>>(rawJson);
        }
        public static void ShowPlaceHolder(TextBox textbox, string placeHolder)
        {
            if (textbox.Text != string.Empty)
            {
                return;
            }
            textbox.Text = placeHolder;
            textbox.Foreground = Brushes.Gray;
        }
        public static void RemovePlaceHolder(TextBox textbox, string placeHolder)
        {
            if (textbox.Text != placeHolder)
            {
                return;
            }
            textbox.Text = string.Empty;
            textbox.Foreground = Brushes.Black;
        }
        public static void ResetText(TextBox textbox, string placeHolder)
        {
            
            textbox.Text = placeHolder;
            textbox.Foreground = Brushes.Gray;
        }
    }
}
