using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Core.Interfaces;
using TimeTracking.Core.Models;
using System.Configuration;

namespace TimeTracking.Service
{
    public class TimeEntryQueryService : ITimeEntryQueryService
    {
        private readonly ITimeEntryQueryRepository _repo;
        private readonly int _overtimeThreshold;
        public TimeEntryQueryService(ITimeEntryQueryRepository repo)
        {
            _repo = repo;

            _overtimeThreshold = int.Parse(ConfigurationManager.AppSettings["WeeklyOvertimeThreshold"]);
        }

        public PagedResult<TimeEntry> GetTimeEntries(int? employeeId,int? projectId,DateTime? startDate,DateTime? endDate,int page,int pageSize)
        {
            return _repo.GetTimeEntries(employeeId,projectId,startDate,endDate,page,pageSize);
        }
        public TimeEntry GetTimeEntryById(int id)
        {
            return _repo.GetTimeEntryById(id);
        }

        public PagedResult<EmployeeWeeklySummary> GetEmployeeWeeklySummary(DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize)
        {
            if (fromDate.HasValue)
            {
                fromDate = GetWeekStart(fromDate.Value);
            }
            else
            {
                fromDate = GetWeekStart(DateTime.Now);
            }
            if (toDate.HasValue)
            {
                toDate = GetWeekEnd(toDate.Value);
            }
            else
            {
                toDate = GetWeekEnd(DateTime.Now);
            }

            var data = _repo.GetEmployeeWeeklySummary(fromDate, toDate, pageNumber, pageSize);

            foreach (var item in data.Items)
            {
                
                item.IsOvertime = item.TotalHours > _overtimeThreshold;
            }

            return data;
        }

        public PagedResult<ProjectWeeklySummary> GetProjectWeeklySummary(DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize)
        {
            if (fromDate.HasValue)
            {
                fromDate = GetWeekStart(fromDate.Value);
            }
            else
            {
                fromDate = GetWeekStart(DateTime.Now);
            }
            if (toDate.HasValue)
            {
                toDate = GetWeekEnd(toDate.Value);
            }
            else
            {
                toDate = GetWeekEnd(DateTime.Now);
            }
            return _repo.GetProjectWeeklySummary(fromDate, toDate, pageNumber, pageSize);
        }

        private DateTime GetWeekStart(DateTime date)
        {
            int diff = date.DayOfWeek - DayOfWeek.Monday;

            if (diff < 0)
                diff += 7;

            return date.AddDays(-diff).Date;
        }

        private DateTime GetWeekEnd(DateTime date)
        {
            int diff = DayOfWeek.Sunday - date.DayOfWeek;

            if (diff < 0)
                diff += 7;

            return date.AddDays(diff).Date;
        }
    }
}
