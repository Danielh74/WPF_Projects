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

        //Checks if the inputs are correct, if so ligging the user else displaying an error message accordingly.
        private void OnUserLogin(object sender, RoutedEventArgs e)
        {
            if (Email_TB.Text == "Email" || Password_TB.Text == "Password")
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
            
            ResetForm();
        }

        //Changing the page to the register page.
        private void ChangeForm(object sender, RoutedEventArgs e)
        {
            ResetForm();
            ChangingForm?.Invoke(this, EventArgs.Empty);
        }

        //Removing the placeholer of the textbox that is in focus.
        private void TextboxInFocus(object sender, RoutedEventArgs e)
        {
            TextBox textboxInFocus = sender as TextBox;
            switch (textboxInFocus.Name)
            {
                case "Email_TB":
                    Helpers.RemovePlaceHolder(textboxInFocus, "Email");
                    break;
                case "Password_TB":
                    Helpers.RemovePlaceHolder(textboxInFocus, "Password");
                    break;
            }
        }

        //Adding a placeholder to the textbox that got out of focus if is empty.
        private void TextboxOutOfFocus(object sender, RoutedEventArgs e)
        {
            TextBox textboxInFocus = sender as TextBox;
            switch (textboxInFocus.Name)
            {
                case "Email_TB":
                    Helpers.AddPlaceHolder(textboxInFocus, "Email");
                    break;
                case "Password_TB":
                    Helpers.AddPlaceHolder(textboxInFocus, "Password");
                    break;
            }
        }

        private void ResetForm()
        {
            Helpers.ResetText(Email_TB, "Email");
            Helpers.ResetText(Password_TB, "Password");
        }
    }
}
