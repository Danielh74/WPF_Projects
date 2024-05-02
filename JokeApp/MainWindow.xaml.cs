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
        public MainWindow()
        {
            InitializeComponent();
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
            string response = await client.GetStringAsync("https://v2.jokeapi.dev/joke/Any");
            return response;
        }
    }
}