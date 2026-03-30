using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Core.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string EmployeeCode { get; set; }

        public string FullName { get; set; }

        public bool IsActive { get; set; }
    }
}
