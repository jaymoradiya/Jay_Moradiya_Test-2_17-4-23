using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_2.Entity
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Department { get; set; }

    }
}