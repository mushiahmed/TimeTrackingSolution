using System;
using System.Collections.Generic;
using TimeTracking.Core.Models;

namespace TimeTracking.Core.Interfaces
{
    public interface ITimeEntryQueryRepository
    {
        PagedResult<TimeEntry> GetTimeEntries(int? employeeId,int? projectId,DateTime? startDate,DateTime? endDate,int page,int pageSize);

        TimeEntry GetTimeEntryById(int id);
        PagedResult<EmployeeWeeklySummary> GetEmployeeWeeklySummary(DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize);
        PagedResult<ProjectWeeklySummary> GetProjectWeeklySummary(DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize);
    }
}
