using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Core.Interfaces;
using TimeTracking.Core.Models;
using System.Configuration;

namespace TimeTracking.Data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public IEnumerable<Employee> GetAll()
        {
            var list = new List<Employee>();

            using (SqlConnection con = new SqlConnection(_conn))
            {
                string query = "SELECT Id, EmployeeCode, FullName, IsActive FROM Employees";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Employee
                    {
                        Id = (int)reader["id"],
                        EmployeeCode = reader["employeecode"].ToString(),
                        FullName = reader["fullname"].ToString(),
                        IsActive = (bool)reader["isactive"]
                    });
                }
            }

            return list;
        }
    }
}
