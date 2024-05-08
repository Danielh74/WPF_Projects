using PhotoGallery.CustomEventArgs;
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

namespace PhotoGallery.Controls
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        public event EventHandler<LoginEventArgs> UserLogin;
        public LoginPage()
        {
            InitializeComponent();
        }

        private void OnUserLogin(object sender, RoutedEventArgs e)
        {
            UserLogin?.Invoke(this, new LoginEventArgs() { Email = Email_TB.Text, Password = Password_TB.Text});
        }
    }
}
