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
        string aboutMe = "Hello! I'm Daniel Hazan, a FullStack development student at HackerU.\nI'm passionate about creating seamless web applications using technologies like JavaScript, HTML, CSS, Node.js, and React.\nAdditionally, I have experience building WPF applications.\nAt HackerU, I've developed my skills through hands-on projects and rigorous coursework.\nI'm driven by a love for innovation and continuous learning, and I enjoy collaborating on tech projects.\nThis is my WPF project, feel free to check it out!";
        public MainWindow()
        {
            InitializeComponent();
            InitializeProjectButtons();
            projectDescription.SetMainWindow(this);
            projectDescription.Visibility = Visibility.Collapsed;
            aboutMeSection.Visibility = Visibility.Collapsed;
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

        private void About_Click(object sender, MouseButtonEventArgs e)
        {
            aboutMeSection.DataContext = aboutMe;
           aboutMeSection.Visibility = Visibility.Visible;
        }
    }
}