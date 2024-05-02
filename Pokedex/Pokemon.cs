using Pokedex.Controls;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Pokedex
{
    public class Pokemon
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public int ID { get; set; }

        public string GeneralDescription { get; set; }

        private double _height;

        [JsonPropertyName("height")]
        public double Height
        {
            get => _height;
            set => _height = value / 10;
        }

        private double _weight;

        [JsonPropertyName("weight")]
        public double Weight
        {
            get => _weight;
            set => _weight = value / 10;
        }

        [JsonPropertyName("stats")]
        public List<Stat> Stats { get; set; }

        [JsonPropertyName("abilities")]
        public List<Ability> Abilities { get; set; }

        [JsonPropertyName("moves")]
        public List<Move> Moves { get; set; }

        [JsonPropertyName("sprites")]
        public Sprites Sprites { get; set; }

        [JsonPropertyName("forms")]
        public List<Form> Forms { get; set; }

        public List<EvolutionDisplay> Evolutions { get; set; }

        [JsonPropertyName("types")]
        public List<Type> Types { get; set; }

        [JsonPropertyName("species")]
        public SpeciesInfo SpeciesInfo { get; set; }
    }

    public class EvolutionDisplay
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sprites")]
        public Sprites EvoSprite { get; set; }
    }

    public class SpeciesInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string URL { get; set; }

        [JsonPropertyName("evolution_chain")]
        public EvoURL EvoChain { get; set; }

        [JsonPropertyName("genera")]
        public List<GeneralInfo> General { get; set; }


        public class EvoURL
        {
            [JsonPropertyName("url")]
            public string URL { get; set; }
        }
    }

    public class GeneralInfo
    {
        [JsonPropertyName("genus")]
        public string Genus { get; set; }

        [JsonPropertyName("language")]
        public Language Language { get; set; }
    }
    public class Language
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("url")]
        public string url { get; set; }
    }

    public class Ability
    {
        [JsonPropertyName("ability")]
        public AbilityInfo Info { get; set; }

        [JsonPropertyName("is_hidden")]
        public bool IsHidden { get; set; }

    }
    public class AbilityInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("effect_entries")]
        public List<EffectEntry> Effect { get; set; }
    }
    public class EffectEntry
    {
        [JsonPropertyName("effect")]
        public string Effect { get; set; }

        [JsonPropertyName("language")]
        public Language Language { get; set; }

        [JsonPropertyName("short_effect")]
        public string Short { get; set; }
    }

    public class Sprites()
    {
        [JsonPropertyName("front_default")]
        public string FrontImage { get; set; }

        [JsonPropertyName("other")]
        public PokemonVisuals Visuals { get; set; }

        public class PokemonVisuals
        {
            [JsonPropertyName("showdown")]
            public Gif Gifs { get; set; }

            public class Gif
            {
                [JsonPropertyName("front_default")]
                public string FrontGif { get; set; }
            }
        }
    }

    public class Form
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Move
    {
        [JsonPropertyName("move")]
        public MoveInfo Info { get; set; }

        public class MoveInfo
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }
        }
    }

    public class Stat
    {
        [JsonPropertyName("stat")]
        public StatInfo Info { get; set; }

        [JsonPropertyName("base_stat")]
        public int BaseStat { get; set; }

        public class StatInfo
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }
        }
    }

    public class Type
    {
        [JsonPropertyName("type")]
        public TypeInfo Info { get; set; }

        public class TypeInfo
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            public string Color { get; set; }
        }
    }


    public class EvoInfo
    {
        [JsonPropertyName("chain")]
        public Evolution Info { get; set; }
    }
    public class Evolution
    {
        [JsonPropertyName("evolves_to")]
        public List<Evolution> Evolutions { get; set; }

        [JsonPropertyName("species")]
        public SpeciesInfo Link { get; set; }
    }

    public class PokemonList
    {
        [JsonPropertyName("results")]
        public List<Pokemon> List { get; set; }
    }
}