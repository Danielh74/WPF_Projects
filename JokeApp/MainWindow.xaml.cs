using JokeApp.CustomEventArgs;
using System.Net.Http;
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

namespace JokeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient client = new HttpClient();
        private const string baseURL = "https://v2.jokeapi.dev/joke";
        private string fullURL = "https://v2.jokeapi.dev/joke/Any";
        public MainWindow()
        {
            InitializeComponent();

            settingsControl.SettingsChanged += ChangeSettings;

            settingsControl.Visibility = Visibility.Collapsed;
        }

        private void ChangeSettings(object? sender, SettingsChangeEventArgs e)
        {
            string fullCategories = string.Join(",", e.Categories);
            string fullBlackList = "";
            if(e.Blacklist != null)
            {
                fullBlackList = string.Join(",", e.Blacklist);
            }

            if (e.Blacklist == null && e.JokeType == null)
            {
                fullURL = $"{baseURL}/{fullCategories}";
            }
            else if (e.Blacklist == null && e.JokeType != null)
            {
                fullURL = $"{baseURL}/{fullCategories}?type={e.JokeType}";
            }
            else if (e.Blacklist != null && e.JokeType == null)
            {
                fullURL = $"{baseURL}/{fullCategories}?blacklistFlags={fullBlackList}";
            }
            else if (e.Blacklist != null && e.JokeType != null)
            {
                fullURL = $"{baseURL}/{fullCategories}?blacklistFlags={fullBlackList}&type={e.JokeType}";
            }

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            TB_Joke.Text = "Loading joke...";

            try
            {
                string joke = await GetJoke();
                JokeTDO jokeObj = JsonSerializer.Deserialize<JokeTDO>(joke);

                if (jokeObj == null)
                {
                    TB_Joke.Text = "No joke to show";
                    return;
                }

                if (jokeObj.Type == "single")
                {
                    TB_Joke.Text = jokeObj.Joke;
                }
                else
                {
                    TB_Joke.Text = jokeObj.Setup + "\n------\n" + jokeObj.Delivery;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task<string> GetJoke()
        {
            string response = await client.GetStringAsync($"{fullURL}");
            return response;
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            settingsControl.Visibility = Visibility.Visible;
        }
    }
}