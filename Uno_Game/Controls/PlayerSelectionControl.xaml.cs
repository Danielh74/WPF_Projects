using System;
using System.Collections.Generic;
using System.Linq;
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
using Uno_Game.CustomEventArgs;
using Uno_Game.Enums;

namespace Uno_Game.Controls
{
    /// <summary>
    /// Interaction logic for PlayerSelectionControl.xaml
    /// </summary>
    public partial class PlayerSelectionControl : UserControl
    {
        public event EventHandler<PlayerSelectionEventArgs> PlayerNumberSelected;
        public PlayerSelectionControl()
        {
            InitializeComponent();
        }

        private void PlayerModeSelection(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch(button.Name)
            {
                case "TwoPlayersBtn":
                    OnPlayerNumberSelected(NumOfPlayers.Two);
                    break;
                case "ThreePlayersBtn":
                    OnPlayerNumberSelected(NumOfPlayers.Three);
                    break;
                case "FourPlayersBtn":
                    OnPlayerNumberSelected(NumOfPlayers.Four);
                    break;
                default:
                    OnPlayerNumberSelected(NumOfPlayers.Two);
                    break;
            }
        }

        private void OnPlayerNumberSelected(NumOfPlayers players)
        {
            PlayerNumberSelected?.Invoke(this, new PlayerSelectionEventArgs { PlayerMode = players });
        }
    }
}
