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
    public class ProjectQueryRepository : IProjectQueryRepository
    {
        private readonly string _conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public IEnumerable<Project> GetAllProjects()
        {
            var list = new List<Project>();

            using (SqlConnection con = new SqlConnection(_conn))
            {
                string query = "SELECT Id, ProjectCode, Name, IsActive FROM Projects";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Project
                    {
                        Id = (int)reader["id"],
                        ProjectCode = reader["ProjectCode"].ToString(),
                        Name = reader["Name"].ToString(),
                        IsActive = (bool)reader["IsActive"]
                    });
                }
            }

            return list;
        }

        public PagedResult<Project> GetProjects(int pageNumber, int pageSize)
        {
            var result = new PagedResult<Project>();
            var list = new List<Project>();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();

                string query = @"SELECT Id, Name, ProjectCode, IsActive
                         FROM Projects
                         ORDER BY Name
                         OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                         SELECT COUNT(*) FROM Projects";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Project
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                ProjectCode = reader["ProjectCode"].ToString(),
                                IsActive = Boolean.Parse(reader["IsActive"].ToString())
                            }) ;
                        }

                        reader.NextResult();

                        if (reader.Read())
                            result.TotalRecords = Convert.ToInt32(reader[0]);
                    }
                }
            }

            result.Items = list;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;

            return result;
        }
    }
}
