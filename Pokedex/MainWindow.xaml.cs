using Pokedex.Controls;
using Pokedex.Utils;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pokedex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient client = new HttpClient();
        private Pokemon pokemonToView = new Pokemon();
        private readonly string[] typeNames = ["NORMAL", "FIRE", "WATER", "ELECTRIC", "GRASS", "ICE", "FIGHTING", "POISON", "GROUND", "FLYING", "PSYCHIC", "BUG", "ROCK", "GHOST", "DRAGON", "DARK", "STEEL", "FAIRY"];
        private readonly string[] typeColors = ["#A8A77A", "#EE8130", "#6390F0", "#F7D02C", "#7AC74C", "#96D9D6", "#C22E28", "#A33EA1", "#E2BF65", "#A98FF3", "#F95587", "#A6B91A", "#B6A136", "#735797", "#6F35FC", "#705746", "#B7B7CE", "#D685AD"];
        public MainWindow()
        {
            InitializeComponent();

            client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");

            LoadingControl.Visibility = Visibility.Collapsed;

            GetNamesFromAPI();
        }

        private async Task GetNamesFromAPI()
        {
            PokemonList pokemonsFromAPI = await client.GetFromJsonAsync<PokemonList>("pokemon/?limit=100000&offset=0");

            List<string> pokemonNames = pokemonsFromAPI.List.Select(pokemon => pokemon.Name).ToList();
            pokemonNames.Sort();
            var capitalNames = pokemonNames.Select(CapitalFirstLetter);

            ItemsList.ItemsSource = capitalNames;
        }

        private async void DisplayPokemon(object sender, RoutedEventArgs e)
        {
            if (ItemsList.SelectedItem == null)
            {
                MessageBox.Show("No pokemon was selected");
                return;
            }
            string? selectedPokemonName = ItemsList.SelectedItem.ToString().ToLower();

            await GetPokemonFromAPI(selectedPokemonName);

            InfoCenter.Start(pokemonToView);
        }

        private async Task GetPokemonFromAPI(string name)
        {
            LoadingControl.Visibility = Visibility.Visible;

            try
            {
                int abilityIndex = 0;

                pokemonToView = await client.GetFromJsonAsync<Pokemon>("pokemon/" + name);

                await GetPokemonGeneralDescription(pokemonToView.SpeciesInfo.URL);

                pokemonToView.Name = CapitalFirstLetter(pokemonToView.Name);

                foreach (Type type in pokemonToView.Types)
                {
                    type.Info.Name = type.Info.Name.ToUpper();

                    int typeIndex = 0;

                    while (type.Info.Name != typeNames[typeIndex])
                    {
                        typeIndex++;
                    }
                    type.Info.Color = typeColors[typeIndex];
                }

                foreach (Move move in pokemonToView.Moves)
                {
                    move.Info.Name = CapitalFirstLetter(move.Info.Name);
                }

                foreach (Ability ability in pokemonToView.Abilities)
                {
                    ability.Info.Name = CapitalFirstLetter(ability.Info.Name);

                    await GetPokemonAbilityEffect(ability.Info.Url, abilityIndex);
                    abilityIndex++;

                    if (ability.IsHidden)
                    {
                        ability.Info.Name = ability.Info.Name + " (Hidden)";
                    }
                }
                if (pokemonToView.Sprites.Visuals.Gifs.FrontGif == null)
                {
                    pokemonToView.Sprites.Visuals.Gifs.FrontGif = pokemonToView.Sprites.FrontImage;
                }
                LoadingControl.Visibility = Visibility.Collapsed;

            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task GetPokemonAbilityEffect(string url, int index)
        {
            HttpClient abilityClient = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };

            try
            {
                AbilityInfo abilityInfo = await abilityClient.GetFromJsonAsync<AbilityInfo>("");

                foreach (EffectEntry entry in abilityInfo.Effect)
                {
                    if (entry.Language.name == "en")
                    {
                        pokemonToView.Abilities[index].Info.Effect = new List<EffectEntry>()
                        {
                            entry
                        };

                        break;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task GetPokemonGeneralDescription(string url)
        {
            HttpClient generalClient = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };

            try
            {
                SpeciesInfo GeneralInfo = await generalClient.GetFromJsonAsync<SpeciesInfo>("");

                foreach (GeneralInfo description in GeneralInfo.General)
                {
                    if (description.Language.name == "en")
                    {
                        pokemonToView.GeneralDescription = description.Genus;
                        break;
                    }
                }

                await GetPokemonEvolution(GeneralInfo.EvoChain.URL);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("Could not find general description" + ex.Message);
            }
        }

        private async Task GetPokemonEvolution(string url)
        {
            pokemonToView.Evolutions = new List<EvolutionDisplay>();
            HttpClient evolutionClient = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };

            try
            {
                EvoInfo evoInfo = await evolutionClient.GetFromJsonAsync<EvoInfo>("");

                foreach (Evolution evolution in evoInfo.Info.Evolutions)
                {
                    ExtractEvos(evolution);
                }

                EvolutionDisplay data;
                HttpResponseMessage response = await client.GetAsync("pokemon/" + evoInfo.Info.Link.Name);
                if (!response.IsSuccessStatusCode)
                {
                    data = new EvolutionDisplay() { Name = pokemonToView.Name, EvoSprite = pokemonToView.Sprites };
                }
                else
                {
                    data = await response.Content.ReadFromJsonAsync<EvolutionDisplay>();
                }

                pokemonToView.Evolutions.Add(data);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("Could not find evolutions: " + ex.Message);
            }
        }

        private async Task ExtractEvos(Evolution evolution)
        {
            if (evolution.Evolutions.Count > 0)
            {
                foreach (Evolution subEvolution in evolution.Evolutions)
                {
                    ExtractEvos(subEvolution);
                }
            }
            try
            {
                EvolutionDisplay evo = await client.GetFromJsonAsync<EvolutionDisplay>("pokemon/" + evolution.Link.Name);
                pokemonToView.Evolutions.Add(evo);
            }
            catch
            {
                return;
            }
        }

        public static string CapitalFirstLetter(string str)
        {
            return str[0].ToString().ToUpper() + str.Substring(1);
        }
    }
}