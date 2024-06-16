using System;
using System.Collections.Generic;
using System.Configuration;
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
using JokeApp.CustomEventArgs;

namespace JokeApp.Controls
{
    /// <summary>
    /// Interaction logic for JokeSettings.xaml
    /// </summary>
    public partial class JokeSettings : UserControl
    {
        public event EventHandler<SettingsChangeEventArgs> SettingsChanged;

        private List<string> categories = new List<string>();
        private List<string> blacklist = new List<string>();
        private string jokeType = null;

        public JokeSettings()
        {
            InitializeComponent();
            DisableCheckboxes();
            Any.IsChecked = true;
            single.IsChecked = true;
            twopart.IsChecked = true;
            categories.Add("Any");
        }

        private void CloseControl(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        //If the checked button's name is "Any" putting "Any" in the categories list, else enabling the other categories checkboxes to choose from.
        private void CategoryRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            RadioButton radioBtn = sender as RadioButton;
            if (radioBtn != null)
            {
                if (radioBtn.Name == "Any")
                {
                    categories.Clear();
                    categories.Add("Any");
                    DisableCheckboxes();

                }
                else
                {
                    categories.Clear();
                    EnableCheckboxes();
                }
            }
        }

        private void DisableCheckboxes()
        {
            foreach (CheckBox checkBox in categoryPanel.Children)
            {
                checkBox.IsChecked = false;
                checkBox.IsEnabled = false;
            }
        }

        private void EnableCheckboxes()
        {
            foreach (CheckBox checkBox in categoryPanel.Children)
            {
                checkBox.IsEnabled = true;
            }
        }

        //Adding the selected category to the list.
        private void CategorySelected(object sender, RoutedEventArgs e)
        {
            CheckBox category = sender as CheckBox;

            categories.Add(category.Name);
        }

        //Removing the unselected category from the list.
        private void CategoryUnselected(object sender, RoutedEventArgs e)
        {
            CheckBox category = sender as CheckBox;

            categories.Remove(category.Name);
        }

        //Adding the selected flag to the list.
        private void FlagSelected(object sender, RoutedEventArgs e)
        {
            CheckBox flag = sender as CheckBox;

            blacklist.Add(flag.Name);
        }

        //Removing the unselected category from the list.
        private void FlagUnselected(object sender, RoutedEventArgs e)
        {
            CheckBox flag = sender as CheckBox;

            blacklist.Remove(flag.Name);
        }

        //Checking if all the mendatory boxes have been checked and sending the changes to the api url.
        private void SetChanges(object sender, RoutedEventArgs e)
        {

            if (categories.Count == 0)
            {
                categoryBorder.BorderBrush = Brushes.Red;
                return;
            }
            if (single.IsChecked == false && twopart.IsChecked == false)
            {
                typeBorder.BorderBrush = Brushes.Red;
                categoryBorder.BorderBrush = Brushes.Black;
                return;
            }
            else
            {
                if (single.IsChecked == true && twopart.IsChecked == true)
                {
                    jokeType = null;
                }
                else if (single.IsChecked == true && twopart.IsChecked == false)
                {
                    jokeType = "single";
                }
                else if (single.IsChecked == false && twopart.IsChecked == true)
                {
                    jokeType = "twopart";
                }

                typeBorder.BorderBrush = Brushes.Black;
                Visibility = Visibility.Collapsed;
                SettingsChanged?.Invoke(sender, new SettingsChangeEventArgs() { Categories = categories, Blacklist = blacklist, JokeType = jokeType });
            }
        }
    }
}
