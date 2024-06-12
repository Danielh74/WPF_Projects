using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_Game.CustomEventArgs
{
    public class TurnWinEventArgs : EventArgs
    {
        public TurnWinEventArgs(bool isPlayerOneTurn)
        {
            IsPlayerOneTurn = isPlayerOneTurn;
        }

        public bool IsPlayerOneTurn { get; set; }
    }
}
