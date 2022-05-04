using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaMeetingsManagerApp.Models
{
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public Person(string name, string surname)
        {
            this.Name = name;
            this.Surname = surname;
        }
        public override bool Equals(object obj)
        {
            if(obj == null || !(obj is Person))
            {
                return false;
            }
            Person person = obj as Person;

            return this.Name.Equals(person.Name) && this.Surname.Equals(person.Surname);
        }

        public override string ToString()
        {
            return string.Format("| {0, -15} | {1, -15} |", this.Name, this.Surname); 
        }
    }
}
