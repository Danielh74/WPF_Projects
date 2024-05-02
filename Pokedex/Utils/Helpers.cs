using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Utils
{
    public class Helpers
    {
        public static string CapitalFirstLetter(string str)
        {
            return str[0].ToString().ToUpper() + str.Substring(1);
        }
        public static List<string> SortNames(List<Pokemon> pokemonList)
        {
            List<string> names = new List<string>();
            foreach (Pokemon pokemon in pokemonList)
            {
                pokemon.Name = CapitalFirstLetter(pokemon.Name);
                names.Add(pokemon.Name);
            }
            names.Sort();
            return names;
        }
    }
}
