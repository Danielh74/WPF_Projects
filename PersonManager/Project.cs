using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace EmployeeManager
{
    public class Project : IProjectMeta
    {
        string desc = "Introducing the Employee Manager WPF App – your go-to tool for managing employees data efficiently and effortlessly.\nIn this app you can add employees with their details, edit them or delete them with a simple click of a button!\nHere you will find a sleek list interface where you can view all your saved employees and if you want to look for a specific indevidual its not a problem! you can use the search box to find specific employees instantly.\nSo come and try it now.";
        public string Name { get; set; } = "Employee Manager";
        public BitmapImage Image => new BitmapImage(new Uri("\\Resources\\EmployeeManager.png", UriKind.Relative));
        public string Description { get => desc; }
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
