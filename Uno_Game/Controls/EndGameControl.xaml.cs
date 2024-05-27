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

namespace Uno_Game.Controls
{
    /// <summary>
    /// Interaction logic for EndGameControl.xaml
    /// </summary>
    public partial class EndGameControl : UserControl
    {
        public event EventHandler<EventArgs> GameRestart;
        public EndGameControl()
        {
            InitializeComponent();
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            EndGamePanel.Visibility = Visibility.Collapsed;
           
           GameRestart?.Invoke(this, EventArgs.Empty);
        }
    }
}
