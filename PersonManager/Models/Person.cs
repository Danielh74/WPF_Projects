using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Models
{
    public class Person
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Department { get; set; }

        public Person(int id, string name, int age, string department)
        {
            ID = id;
            Name = name;
            Age = age;
            Department = department;
        }
    }
}
