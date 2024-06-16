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

namespace Pong.Controls
{
    /// <summary>
    /// Interaction logic for StartScreen.xaml
    /// </summary>
    public partial class StartScreen : UserControl
    {
        public event EventHandler GameStarted;
        public StartScreen()
        {
            InitializeComponent();

            Focusable = true;
            Loaded += (sender, e) => Focus();

            KeyDown += HandleKeyDown;
        }

        //Starts game when the space bar is pressed.
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                GameStarted?.Invoke(this, e);
            }
        }
    }
}
