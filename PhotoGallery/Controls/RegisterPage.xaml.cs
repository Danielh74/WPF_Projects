using PhotoGallery.CustomEventArgs;
using PhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        public RegisterPage()
        {
            InitializeComponent();
        }

        private void OnUserRegister(object sender, RoutedEventArgs e)
        {
            string rawJson = File.ReadAllText("PhotosInvantory.json");
            List<User> userList = JsonSerializer.Deserialize<List<User>>(rawJson);

            User newUser = new User()
            {
                UserName = UserName_TB.Text,
                Email = Email_TB.Text,
                Password = Password_TB.Text,
                Gallery = new List<Photo>()
            };
            
            userList.Add(newUser);

            string updatedList = JsonSerializer.Serialize(userList,options);
            File.WriteAllText("PhotosInvantory.json", updatedList);

            UserRegistered?.Invoke(this,new LoginEventArgs { Email = Email_TB.Text, Password = Password_TB.Text });

            UserName_TB.Text = "";
            Email_TB.Text = "";
            Password_TB.Text = "";
        }

        private void ChangeForm(object sender, RoutedEventArgs e)
        {
            ChangingForm?.Invoke(this, EventArgs.Empty);
        }
    }
}
