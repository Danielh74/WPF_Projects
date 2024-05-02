using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace Pokedex.Controls
{
    /// <summary>
    /// Interaction logic for InfoCenter.xaml
    /// </summary>
    public partial class InfoCenter : UserControl
    {
        public InfoCenter()
        {
            InitializeComponent();
            Visibility = Visibility.Collapsed;
        }

        public void Start(Pokemon presentedPokemon)
        {
            Visibility = Visibility.Visible;
            DisplayPokemon(presentedPokemon);
        }

        private void DisplayPokemon(Pokemon pokemon)
        {
            DataContext = pokemon;
            PokemonMoves.ItemsSource = pokemon.Moves;
        }
    }
}
