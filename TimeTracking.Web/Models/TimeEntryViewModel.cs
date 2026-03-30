
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TimeTracking.Web.Models
{
    public class TimeEntryViewModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }

        public DateTime EntryDate { get; set; }

        public decimal Hours { get; set; }

        public string Notes { get; set; }
        public string Source { get; set; }

        public IEnumerable<SelectListItem> Employees { get; set; }

        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}