using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Uno_Game
{
    public class Player
    {
        public string Name { get; set; }
        public List<string> Deck { get; set; }
        public StackPanel Hand { get; set; }

        public Player(string name,StackPanel hand, List<string> deck) 
        {
            Name = name;
            Hand = hand;
            Deck = deck;
        }
    }
}
