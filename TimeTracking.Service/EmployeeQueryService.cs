using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Core.Interfaces;
using TimeTracking.Core.Models;

namespace TimeTracking.Service
{
    public class EmployeeQueryService : IEmployeeQueryService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeQueryService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _repo.GetAll();
        }
    }
}
