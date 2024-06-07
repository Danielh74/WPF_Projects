using Common;
using ProjectGallery.CustomEventArgs;
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

namespace ProjectGallery.Controls
{
    /// <summary>
    /// Interaction logic for ProjectDescription.xaml
    /// </summary>
    public partial class ProjectDescription : UserControl
    {
        MainWindow mainWindow;
        IProjectMeta displayedProject;
        public ProjectDescription()
        {
            InitializeComponent();
        }

        private void HandleClick(object? sender, ProjectRedirectEventArgs e)
        {
            DataContext = e.Project;
            displayedProject = e.Project;
        }

        private void Back_Click(object sender, MouseButtonEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
        public void SetMainWindow(MainWindow window)
        {
            mainWindow = window;
            mainWindow.ProjectClicked += HandleClick;
        }

        private void StartGame(object sender, MouseButtonEventArgs e)
        {
            displayedProject.Run();
        }
    }
}
