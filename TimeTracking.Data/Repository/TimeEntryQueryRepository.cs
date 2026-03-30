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
    public class TimeEntryQueryRepository : ITimeEntryQueryRepository
    {
        private readonly string _conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public PagedResult<TimeEntry> GetTimeEntries(
            int? employeeId,
            int? projectId,
            DateTime? startDate,
            DateTime? endDate,
            int page,
            int pageSize)
        {
            var result = new PagedResult<TimeEntry>();
            var list = new List<TimeEntry>();

            using (var con = new SqlConnection(_conn))
            {
                con.Open();

                var sql = @"
                SELECT e.FullName,
                       p.Name,
                       te.EntryDate,
                       te.Hours,
                       te.Notes,
                        te.Source,
                        te.Id
                FROM TimeEntries te
                JOIN Employees e ON te.EmployeeId = e.Id
                JOIN Projects p ON te.ProjectId = p.Id
                WHERE (@EmployeeId IS NULL OR te.EmployeeId=@EmployeeId)
                AND (@ProjectId IS NULL OR te.ProjectId=@ProjectId)
                AND (@StartDate IS NULL OR te.EntryDate>=@StartDate)
                AND (@EndDate IS NULL OR te.EntryDate<=@EndDate)
                ORDER BY te.EntryDate DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(*)
                FROM TimeEntries te
                WHERE (@EmployeeId IS NULL OR te.EmployeeId=@EmployeeId)
                AND (@ProjectId IS NULL OR te.ProjectId=@ProjectId)
                AND (@StartDate IS NULL OR te.EntryDate>=@StartDate)
                AND (@EndDate IS NULL OR te.EntryDate<=@EndDate)";

                var cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@EmployeeId", (object)employeeId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ProjectId", (object)projectId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@StartDate", (object)startDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new TimeEntry
                        {

                            EmployeeName = reader.GetString(0),
                            ProjectName = reader.GetString(1),
                            EntryDate = reader.GetDateTime(2),
                            Hours = reader.GetDecimal(3),
                            Notes = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            Source = reader.IsDBNull(5) ? "" : reader.GetString(5),
                            Id = reader.GetInt32(6)
                        });
                    }

                    reader.NextResult();

                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }

            result.Items = list;
            result.PageNumber = page;
            result.PageSize = pageSize;

            return result;
        }

        public TimeEntry GetTimeEntryById(int id)
        {
            TimeEntry timeEntry = null;

            using (var con = new SqlConnection(_conn))
            {
                con.Open();

                var sql = @"
                    SELECT e.FullName,p.Name,te.EntryDate,te.Hours,te.Notes,te.Source,te.Id,te.ProjectId,te.EmployeeId
                    FROM TimeEntries te
                    JOIN Employees e ON te.EmployeeId = e.Id
                    JOIN Projects p ON te.ProjectId = p.Id
                    WHERE te.Id = @Id";

                var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        timeEntry = new TimeEntry
                        {
                            EmployeeName = reader["FullName"].ToString(),
                            ProjectName = reader["Name"].ToString(),
                            EntryDate = DateTime.Parse(reader["EntryDate"].ToString()),
                            Hours = decimal.Parse(reader["Hours"].ToString()),
                            Notes = reader["Notes"].ToString(),
                            Source = reader["Source"].ToString(),
                            Id = int.Parse(reader["Id"].ToString()),
                            ProjectId = int.Parse(reader["ProjectId"].ToString()),
                            EmployeeId = int.Parse(reader["EmployeeId"].ToString())
                        };
                    }
                }
            }

            return timeEntry;
        }

        public PagedResult<EmployeeWeeklySummary> GetEmployeeWeeklySummary(DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize)
        {
            var result = new PagedResult<EmployeeWeeklySummary>();
            var list = new List<EmployeeWeeklySummary>();
            int startRow = ((pageNumber - 1) * pageSize) + 1;
            int endRow = pageNumber * pageSize;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                string sql = @"
                    WITH WeeklySummary AS
                    (
                        SELECT 
                            e.id EmployeeId,
                            e.fullname EmployeeName,
                            DATEPART(YEAR, t.entrydate) Year,
                            DATEPART(WEEK, t.entrydate) WeekNumber,
                            SUM(t.hours) TotalHours
                        FROM TimeEntries t
                        INNER JOIN Employees e ON e.id = t.employeeid
                        WHERE (@fromDate IS NULL OR t.entrydate >= @fromDate)
                          AND (@toDate IS NULL OR t.entrydate <= @toDate)
                        GROUP BY 
                            e.id,
                            e.fullname,
                            DATEPART(YEAR, t.entrydate),
                            DATEPART(WEEK, t.entrydate)
                    )

                    SELECT * FROM
                    (
                        SELECT *,
                               ROW_NUMBER() OVER (ORDER BY EmployeeName) RowNum
                        FROM WeeklySummary
                    ) AS Result
                    WHERE RowNum BETWEEN @StartRow AND @EndRow";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@StartRow", startRow);
                cmd.Parameters.AddWithValue("@EndRow", endRow);
                cmd.Parameters.AddWithValue("@fromDate", (object)fromDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@toDate", (object)toDate ?? DBNull.Value);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new EmployeeWeeklySummary
                        {
                            EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                            EmployeeName = reader["EmployeeName"].ToString(),
                            Year = Convert.ToInt32(reader["Year"]),
                            WeekNumber = Convert.ToInt32(reader["WeekNumber"]),
                            TotalHours = Convert.ToDecimal(reader["TotalHours"])
                            
                        }) ;
                    }
                }
            }

            result.Items = list;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.FromDate = fromDate ?? DateTime.Today;
            result.ToDate = toDate ?? DateTime.Today;
            result.TotalRecords = GetTotalEmployeeWeeklyCount(fromDate, toDate);

            return result;
        }

        public PagedResult<ProjectWeeklySummary> GetProjectWeeklySummary(DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize)
        {
            var result = new PagedResult<ProjectWeeklySummary>();
            var list = new List<ProjectWeeklySummary>();
            int startRow = ((pageNumber - 1) * pageSize) + 1;
            int endRow = pageNumber * pageSize;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                string sql = @"
                    WITH WeeklySummary AS
                    (
                                    SELECT 
                            p.id ProjectId,
                            p.name ProjectName,
                            DATEPART(YEAR, t.entrydate) Year,
                            DATEPART(WEEK, t.entrydate) WeekNumber,
                            SUM(t.hours) TotalHours
                        FROM TimeEntries t
                        INNER JOIN Projects p ON p.id = t.projectid
                        WHERE (@fromDate IS NULL OR t.entrydate >= @fromDate)
                                      AND (@toDate IS NULL OR t.entrydate <= @toDate)
                        GROUP BY 
                            p.id,
                            p.name,
                            DATEPART(YEAR, t.entrydate),
                            DATEPART(WEEK, t.entrydate)
                    )

                    SELECT * FROM
                    (
                        SELECT *,
                               ROW_NUMBER() OVER (ORDER BY ProjectName) RowNum
                        FROM WeeklySummary
                    ) AS Result
                    WHERE RowNum BETWEEN @StartRow AND @EndRow";

                
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@StartRow", startRow);
                cmd.Parameters.AddWithValue("@EndRow", endRow);
                cmd.Parameters.AddWithValue("@fromDate", (object)fromDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@toDate", (object)toDate ?? DBNull.Value);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ProjectWeeklySummary
                        {
                            ProjectId = Convert.ToInt32(reader["ProjectId"]),
                            ProjectName = reader["ProjectName"].ToString(),
                            Year = Convert.ToInt32(reader["Year"]),
                            WeekNumber = Convert.ToInt32(reader["WeekNumber"]),
                            TotalHours = Convert.ToDecimal(reader["TotalHours"])
                        });
                    }
                }
            }

            result.Items = list;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.FromDate = fromDate ?? DateTime.Today;
            result.ToDate = toDate ?? DateTime.Today;
            result.TotalRecords = GetTotalProjectWeeklyCount(fromDate, toDate);

            return result;
        }

        private int GetTotalEmployeeWeeklyCount(DateTime? fromDate, DateTime? toDate)
        {
            string sql = @"SELECT COUNT(*)
                            FROM
                            (
                                SELECT 
                                    e.id,
                                    DATEPART(YEAR, t.entrydate) entryYear,
                                    DATEPART(WEEK, t.entrydate) entryWeek
                                FROM TimeEntries t
                                INNER JOIN Employees e ON e.id = t.employeeid
                                WHERE (@fromDate IS NULL OR t.entrydate >= @fromDate)
                                AND (@toDate IS NULL OR t.entrydate <= @toDate)
                                GROUP BY 
                                    e.id,
                                    DATEPART(YEAR, t.entrydate),
                                    DATEPART(WEEK, t.entrydate)
                            ) T";

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@fromDate", (object)fromDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@toDate", (object)toDate ?? DBNull.Value);

                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int GetTotalProjectWeeklyCount(DateTime? fromDate, DateTime? toDate)
        {
            string sql = @"SELECT COUNT(*)
                            FROM
                            (
                                SELECT 
                                    p.id,
                                    DATEPART(YEAR, t.entrydate) entryYear,
                                    DATEPART(WEEK, t.entrydate) entryWeek
                                FROM TimeEntries t
                                INNER JOIN Projects p ON p.id = t.projectid
                                WHERE (@fromDate IS NULL OR t.entrydate >= @fromDate)
                                AND (@toDate IS NULL OR t.entrydate <= @toDate)
                                GROUP BY 
                                    p.id,
                                    DATEPART(YEAR, t.entrydate),
                                    DATEPART(WEEK, t.entrydate)
                            ) T";

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@fromDate", (object)fromDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@toDate", (object)toDate ?? DBNull.Value);

                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}