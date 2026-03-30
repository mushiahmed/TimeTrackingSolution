using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeTracking.Core.Models;

namespace TimeTracking.Web.Models
{
    public class TimeEntryIndexViewModel
    {
        public PagedResult<TimeEntry> PagedResult { get; set; }
        public int? EmployeeId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public IEnumerable<SelectListItem> Employees { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}