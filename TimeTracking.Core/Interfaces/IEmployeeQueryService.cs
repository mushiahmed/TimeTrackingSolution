using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeTracking.Core.Models;

namespace TimeTracking.Core.Interfaces
{
    public interface IEmployeeQueryService
    {
        IEnumerable<Employee> GetEmployees();
    }
}