using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonManager.Models
{
    public class Person
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public Person(int id, string name, int age)
        {
            ID = id;
            Name = name;
            Age = age;
        }
    }
}
