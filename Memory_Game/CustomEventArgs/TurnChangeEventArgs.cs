using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_Game.CustomEventArgs
{
    public class TurnChangeEventArgs : EventArgs
    {
        public TurnChangeEventArgs(bool isPlayerOneTurn)
        {
            IsPlayerOneTurn = isPlayerOneTurn;
        }

        public bool IsPlayerOneTurn { get; set; }
    }
}
