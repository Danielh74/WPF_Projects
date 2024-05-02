using PersonManager.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string filePath = @"C:\Users\Danie\Desktop\Studies\.NET\.NET_course\WPF\Projects\PersonManager\people.json";
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        private ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();
        private readonly ICollectionView peopleView;

        public MainWindow()
        {
            InitializeComponent();

            peopleView = CollectionViewSource.GetDefaultView(People);
            PeopleTable.ItemsSource = peopleView;

            LoadData();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TB_ID.Text == "" || !int.TryParse(TB_ID.Text, out int id) || IdExists())
            {
                if (IdExists())
                {
                    MessageBox.Show("ID already exists");
                }
                else
                {
                    MessageBox.Show("ID value is invalid");
                }

                return;
            }

            if (TB_Name.Text.Length == 0)
            {
                MessageBox.Show("Name value is invalid");
                return;
            }

            if (TB_Age.Text.Length == 0 || !int.TryParse(TB_Age.Text, out int age))
            {
                MessageBox.Show("Age value is invalid");
                return;
            }

            string name = TB_Name.Text;

            People.Add(new Person(id, name, age));

            SaveData();
            ClearForm();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PeopleTable.SelectedItem is Person selectedPerson
                && int.TryParse(TB_Age.Text, out int age)
                && TB_Name.Text.Length > 0)
            {
                selectedPerson.Name = TB_Name.Text;
                selectedPerson.Age = age;
            }
            else
            {
                MessageBox.Show("Cannot update item: Invalid inputs");
                return;
            }

            SaveData();
            ClearForm();

            TB_ID.IsEnabled = true;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();

            TB_ID.IsEnabled = true;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        // For Delete buttons for every item
        private void HandleDeleteClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete?", "Delete item", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) 
            {
                return;
            }

            Button btn = sender as Button;
            if (btn == null) 
            {
                return;
            }

            if(btn.DataContext is Person personToDelete) 
            {
                People.Remove(personToDelete);
                SaveData();
                ClearForm();

                TB_ID.IsEnabled = true;
            }
        }

        private void PeopleTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TB_ID.IsEnabled = false;

            if (PeopleTable.SelectedItem is Person selectedPerson)
            {
                TB_ID.Text = selectedPerson?.ID.ToString();
                TB_Name.Text = selectedPerson?.Name;
                TB_Age.Text = selectedPerson?.Age.ToString();
            }
        }

        private void Filter_KeyUp(object sender, KeyEventArgs e)
        {
            string filterString = TB_Filter.Text.ToLower();

            peopleView.Filter = o =>
            {
                if(o is Person personToFilter)
                {
                    return personToFilter.Name.ToLower().Contains(filterString);
                }
                return false;
            };
        }

        private void LoadData()
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            try
            {
                string rawData = File.ReadAllText(filePath);
                List<Person>? result = JsonSerializer.Deserialize<List<Person>>(rawData);
                if (result == null)
                {
                    return;
                }
                else
                {
                    foreach (Person person in result)
                    {
                        People.Add(person);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load data: {ex.Message}");
            }
        }

        private void SaveData()
        {
            try
            {
                string rawData = JsonSerializer.Serialize(People, options);
                File.WriteAllText(filePath, rawData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save data: {ex.Message}");
            }
        }

        private void ClearForm()
        {
            TB_ID.Clear();
            TB_Age.Clear();
            TB_Name.Clear();

            PeopleTable.SelectedItem = null;
        }

        private bool IdExists()
        {
            foreach (Person person in People)
            {
                if (person.ID.ToString() == TB_ID.Text)
                {
                    return true;
                }
            }
            return false;
        }

        private void FilterInFocus(object sender, RoutedEventArgs e)
        {
            if(TB_Filter.Text != "Filter...")
            {
                return;
            }
            TB_Filter.Text = "";
            TB_Filter.Foreground = Brushes.Black;
        }

        private void FilterOutOfFocus(object sender, RoutedEventArgs e)
        {
            TB_Filter.Text = "Filter...";
            TB_Filter.Foreground = Brushes.Gray;

            peopleView.Filter = o => true;
        }
    }
}