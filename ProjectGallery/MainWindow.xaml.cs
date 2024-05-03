using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjectGallery.Controls;
using Common;

namespace ProjectGallery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IProjectMeta[] projects = [
            new Tic_Tac_Toe.Project(),
            new Memory_Game.Project(),
            new PersonManager.Project(),
            new JokeApp.Project(),
            new Pokedex.Project(),
            new PhotoGallery.Project(),
        ];
        public MainWindow()
        {
            InitializeComponent();
            InitializeProjectButtons();
        }

        private void InitializeProjectButtons()
        {
            foreach (IProjectMeta project in projects)
            {
                ProjectButton button = new ProjectButton(project);
                button.Margin = new Thickness(10);
                ProjectPanel.Children.Add(button);
            }
        }
    }
}