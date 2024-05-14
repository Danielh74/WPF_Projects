using PhotoGallery.CustomEventArgs;
using PhotoGallery.Models;
using PhotoGallery.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : UserControl
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public event EventHandler<LoginEventArgs> UserRegistered;
        public event EventHandler ChangingForm;

        List<User> userList;

        public RegisterPage()
        {
            InitializeComponent();

            userList = Helpers.LoadUsers();
        }

        private void OnUserRegister(object sender, RoutedEventArgs e)
        {
            string emailFormat = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            string passwordPattern = @"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z]).{8,}$";

            if (Email_TB.Text == "" || Password_TB.Text == "" || UserName_TB.Text == "")
            {
                ErrorMessage.Text = "Please input email, password and username";
                return;
            }

            if (!Regex.IsMatch(Email_TB.Text, emailFormat))
            {
                ErrorMessage.Text = "The email is an incorrect format";
                return;
            }

            if (!Regex.IsMatch(Password_TB.Text, passwordPattern))
            {
                ErrorMessage.Text = "The password has to have at least:\n- 8 characters\n- 1 digit\n- 1 uppercase letter\n- 1 lowercase letter ";
                return;
            }

            if (userList.Exists(user => user.Email == Email_TB.Text))
            {
                ErrorMessage.Text = "Email is already registered";
                return;
            }

            User newUser = new User()
            {
                UserName = char.ToUpper(UserName_TB.Text[0]) + UserName_TB.Text.Substring(1),
                Email = Email_TB.Text,
                Password = Password_TB.Text,
                Gallery = new List<Photo>()
            };

            userList.Add(newUser);

            string updatedList = JsonSerializer.Serialize(userList, options);
            File.WriteAllText("UsersInvantory.json", updatedList);

            UserRegistered?.Invoke(this, new LoginEventArgs { Email = Email_TB.Text, Password = Password_TB.Text, UserList = userList });

            UserName_TB.Text = "";
            Email_TB.Text = "";
            Password_TB.Text = "";
        }

        private void ChangeForm(object sender, RoutedEventArgs e)
        {
            ChangingForm?.Invoke(this, EventArgs.Empty);

            UserName_TB.Text = "";
            Email_TB.Text = "";
            Password_TB.Text = "";
        }
    }
}
