using EmployeeManager.Models;
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

namespace EmployeeManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        private ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();
        private readonly ICollectionView peopleView;
        private List<string> departmentList = new List<string>() { "R&D", "IT", "HR", "Analytics", "Managment", "QA", "Development", "Design" };
        public MainWindow()
        {
            InitializeComponent();

            btn_Update.Visibility = Visibility.Collapsed;
            peopleView = CollectionViewSource.GetDefaultView(People);
            PeopleTable.ItemsSource = peopleView;
            DepartmentList.ItemsSource = departmentList;

            LoadData();
        }
        private void LoadData()
        {
            if (!File.Exists("people.json"))
            {
                return;
            }
            try
            {
                string rawData = File.ReadAllText("people.json");
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

        //Checking if the inputs the user put are valid, if so adding the employee to the list and if not displays an error message
        private void AddEmployee(object sender, RoutedEventArgs e)
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

            if (DepartmentList.SelectedItem == null)
            {
                MessageBox.Show("No department was selected");
                return;
            }

            string? department = DepartmentList.SelectedItem.ToString();

            People.Add(new Person(id, name, age, department));

            SaveData();
            ClearForm();
        }

        private void SaveData()
        {
            try
            {
                string rawData = JsonSerializer.Serialize(People, options);
                File.WriteAllText("people.json", rawData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save data: {ex.Message}");
            }
        }

        // Displaying the employee info in the textboxes to edit.
        private void ShowDetailsToEdit(object sender, RoutedEventArgs e)
        {
            TB_ID.IsEnabled = false;
            btn_Update.Visibility = Visibility.Visible;
            btn_Add.Visibility = Visibility.Collapsed;

            if (PeopleTable.SelectedItem is Person selectedPerson)
            {
                TB_ID.Text = selectedPerson?.ID.ToString();
                TB_Name.Text = selectedPerson?.Name;
                TB_Age.Text = selectedPerson?.Age.ToString();
                DepartmentList.SelectedItem = selectedPerson?.Department;
            }
        }

        private void UpdateEmployee(object sender, RoutedEventArgs e)
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
            btn_Update.Visibility = Visibility.Collapsed;
            btn_Add.Visibility = Visibility.Visible;

            TB_ID.IsEnabled = true;
        }

        private void DeleteEmployee(object sender, RoutedEventArgs e)
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

            if (btn.DataContext is Person personToDelete)
            {
                People.Remove(personToDelete);
                SaveData();
                ClearForm();

                TB_ID.IsEnabled = true;
            }
        }

        private void ClearForm()
        {
            TB_ID.Clear();
            TB_Age.Clear();
            TB_Name.Clear();
            DepartmentList.SelectedItem = null;

            PeopleTable.SelectedItem = null;
        }

        // Resetting the form and updating the add and updaye buttons visibility when the clear button is clicked.
        private void HandleClearClick(object sender, RoutedEventArgs e)
        {
            ClearForm();
            btn_Update.Visibility = Visibility.Collapsed;
            btn_Add.Visibility = Visibility.Visible;
            TB_ID.IsEnabled = true;
        }

        // Filters peopleView based on the text entered in the TB_Filter TextBox when the key is up.
        private void Filter_KeyUp(object sender, KeyEventArgs e)
        {
            string filterString = TB_Filter.Text.ToLower();

            peopleView.Filter = o =>
            {
                if (o is Person personToFilter)
                {
                    return personToFilter.Name.ToLower().Contains(filterString);
                }
                return false;
            };
        }

        // Checking whether the ID the user put already exists in the system.
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

        //Changing the text color clearing the placeholder text in the TB_Filter TextBox when it is in focus.
        private void FilterInFocus(object sender, RoutedEventArgs e)
        {
            if (TB_Filter.Text != "Filter...")
            {
                return;
            }
            TB_Filter.Text = "";
            TB_Filter.Foreground = Brushes.Black;
        }

        //Resetting the placeholder text in the TB_Filter TextBox when it is out of focus.
        private void FilterOutOfFocus(object sender, RoutedEventArgs e)
        {
            TB_Filter.Text = "Filter...";
            TB_Filter.Foreground = Brushes.Gray;

            peopleView.Filter = o => true;
        }
    }
}