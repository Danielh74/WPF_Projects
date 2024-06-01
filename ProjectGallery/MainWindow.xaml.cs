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
        public event EventHandler<ProjectRedirectEventArgs> ProjectClicked;
        public MainWindow()
        {
            InitializeComponent();
            InitializeProjectButtons();
            projectDescription.SetMainWindow(this);
            projectDescription.Visibility = Visibility.Collapsed;
        }

        private void InitializeProjectButtons()
        {
            foreach (IProjectMeta project in Projects.List)
            {
                StackPanel projectBtn = new StackPanel()
                {
                    Margin = new Thickness(10)
                };
                Image projectImg = new Image()
                {
                    Source = project.Image,
                    Height = 60,
                    Width = 60,
                    Cursor = Cursors.Hand
                };
                TextBlock projectName = new TextBlock()
                {
                    Text = project.Name,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0,5,0,0)
                };

                projectBtn.MouseDown += (sender, e) =>
                {
                    projectDescription.Visibility = Visibility.Visible;
                    ProjectClicked?.Invoke(this, new ProjectRedirectEventArgs() { Project = project });
                };

                projectBtn.Children.Add(projectImg);
                projectBtn.Children.Add(projectName);

                ProjectPanel.Children.Add(projectBtn);
            }
        }
    }

    public class ProjectRedirectEventArgs : EventArgs
    {
        public IProjectMeta Project { get; set; }
        

        
    }
}