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
using ProjectGallery.CustomEventArgs;

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

            ViewProjectList(Projects.List, ProjectPanel);
            ViewProjectList(Projects.ExtraList, ExtraPanel);

            projectDescription.SetMainWindow(this);

            projectDescription.Visibility = Visibility.Collapsed;
            aboutMeSection.Visibility = Visibility.Collapsed;
        }

        private void ViewProjectList(IProjectMeta[] projectList, Panel panel)
        {
            foreach (IProjectMeta project in projectList)
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
                    FontWeight = FontWeights.DemiBold,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 0)
                };

                projectBtn.MouseDown += (sender, e) =>
                {
                    projectDescription.Visibility = Visibility.Visible;
                    ProjectClicked?.Invoke(this, new ProjectRedirectEventArgs() { Project = project });
                };

                projectBtn.Children.Add(projectImg);
                projectBtn.Children.Add(projectName);

                panel.Children.Add(projectBtn);
            }
        }
        private void About_Click(object sender, MouseButtonEventArgs e)
        {
           aboutMeSection.Visibility = Visibility.Visible;
        }
    }
}