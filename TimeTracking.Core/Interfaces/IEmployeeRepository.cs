using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Core.Models;

namespace TimeTracking.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
    }
}
