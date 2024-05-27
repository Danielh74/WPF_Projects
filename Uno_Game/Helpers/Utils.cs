using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno_Game.Helpers
{
    public static class Utils
    {
        public static string FindCurrentColor(string upCardName)
        {
            int underscoreIndex = upCardName.IndexOf('_');
            string color = upCardName.Substring(underscoreIndex + 1);
            return char.ToUpper(color[0]) + color.Substring(1);
        }
        public static string FindCurrentNumber(string upCardName)
        {
            int underscoreIndex = upCardName.IndexOf('_');
            return upCardName.Substring(0, underscoreIndex);
        }
        public static bool ChackGameWon(Player activePlayer)
        {
            if (activePlayer.Deck.Count == 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
