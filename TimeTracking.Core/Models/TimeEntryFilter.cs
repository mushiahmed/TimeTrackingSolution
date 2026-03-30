using System;

namespace TimeTracking.Core.Models
{
    public class TimeEntryFilter
    {
        public int? EmployeeId { get; set; }

        public int? ProjectId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
