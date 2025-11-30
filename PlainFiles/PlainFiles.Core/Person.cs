using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainFiles.Core
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public decimal Balance { get; set; }

        public Person(int id, string name, string lastName, string phone, decimal balance)
        {
            ID = id;
            Name = name;
            LastName = lastName;
            Phone = phone;
            Balance = balance;
        }
    }

}
