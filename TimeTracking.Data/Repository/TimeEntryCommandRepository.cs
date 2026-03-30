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
    public class TimeEntryCommandRepository : ITimeEntryCommandRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void Insert(TimeEntry entry)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO TimeEntries
                          (EmployeeId,ProjectId,EntryDate,Hours,Notes,Source, CreatedAt)
                          VALUES
                          (@EmployeeId,@ProjectId,@EntryDate,@Hours,@Notes,@Source, @CreatedAt)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@EmployeeId", entry.EmployeeId);
                cmd.Parameters.AddWithValue("@ProjectId", entry.ProjectId);
                cmd.Parameters.AddWithValue("@EntryDate", entry.EntryDate);
                cmd.Parameters.AddWithValue("@Hours", entry.Hours);
                cmd.Parameters.AddWithValue("@Notes", entry.Notes);
                cmd.Parameters.AddWithValue("@Source", entry.Source);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(TimeEntry entry)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE TimeEntries
                           SET EmployeeId=@EmployeeId,
                               ProjectId=@ProjectId,
                               EntryDate=@EntryDate,
                               Hours=@Hours,
                               Notes=@Notes,
                               Source=@Source
                           WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Id", entry.Id);
                cmd.Parameters.AddWithValue("@EmployeeId", entry.EmployeeId);
                cmd.Parameters.AddWithValue("@ProjectId", entry.ProjectId);
                cmd.Parameters.AddWithValue("@EntryDate", entry.EntryDate);
                cmd.Parameters.AddWithValue("@Hours", entry.Hours);
                cmd.Parameters.AddWithValue("@Notes", entry.Notes);
                cmd.Parameters.AddWithValue("@Source", entry.Source);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM TimeEntries WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
