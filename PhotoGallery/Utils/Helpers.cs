using PhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PhotoGallery.Utils
{
    public static class Helpers
    {
        public static bool IsVideoFormat(string uri)
        {
            string[] videoExtensions = [".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm"];

            string mediaToCheck = System.IO.Path.GetExtension(uri);
            if (mediaToCheck != null && Array.IndexOf(videoExtensions, mediaToCheck.ToLower()) == -1)
            {
                return false;
            }
            return true;
        }

        public static BitmapImage SetLikeButton(bool isLiked)
        {
            if (isLiked)
            {
                return new BitmapImage(new Uri(@"\Resources\full_heart.png", UriKind.Relative));
            }
            else
            {
                return new BitmapImage(new Uri(@"\Resources\empty_heart.png", UriKind.Relative));

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
            string rawJson = File.ReadAllText("PhotosInvantory.json");
            return JsonSerializer.Deserialize<List<User>>(rawJson);
        }
    }
}
