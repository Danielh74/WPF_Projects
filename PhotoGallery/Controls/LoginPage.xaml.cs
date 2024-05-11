using PhotoGallery.CustomEventArgs;
using PhotoGallery.Models;
using PhotoGallery.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public event EventHandler<LoginEventArgs> UserLoggedIn;
        public event EventHandler ChangingForm;

        List<User> userList;

        public LoginPage()
        {
            InitializeComponent();

            userList = Helpers.LoadUsers();
        }

        private void OnUserLogin(object sender, RoutedEventArgs e)
        {
            if (Email_TB.Text == "" || Password_TB.Text == "")
            {
                ErrorMessage.Text = "Please input email and password";
                return;
            }

            if (!userList.Exists(user => user.Email == Email_TB.Text))
            {
                ErrorMessage.Text = "Email not registered";
                return;
            }

            if (!userList.Exists(user => user.Email == Email_TB.Text && user.Password == Password_TB.Text))
            {
                ErrorMessage.Text = "Password is incorrect";
                return;
            }

            UserLoggedIn?.Invoke(this, new LoginEventArgs() { Email = Email_TB.Text, Password = Password_TB.Text, UserList = userList });
            Email_TB.Text = "";
            Password_TB.Text = "";
        }

        private void ChangeForm(object sender, RoutedEventArgs e)
        {
            ChangingForm?.Invoke(this, EventArgs.Empty);
        }
    }
}
