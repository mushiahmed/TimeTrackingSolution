using System;

namespace TimeTracking.Core.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }

        public DateTime EntryDate { get; set; }

        public decimal Hours { get; set; }

        public string Notes { get; set; }

        public string Source { get; set; }
        public string EmployeeName { get; set; }
        public string ProjectName { get; set; }

    }
}
