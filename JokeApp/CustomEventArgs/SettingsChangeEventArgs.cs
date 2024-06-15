using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JokeApp.CustomEventArgs
{
    public class SettingsChangeEventArgs : EventArgs
    {
        public List<string> Categories { get; set; }
        public List<string> Blacklist { get; set; }
        public string JokeType { get; set; }
    }
}
