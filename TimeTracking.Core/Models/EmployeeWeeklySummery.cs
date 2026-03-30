using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Core.Models
{
    public class EmployeeWeeklySummary
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int WeekNumber { get; set; }
        public int Year { get; set; }
        public decimal TotalHours { get; set; }
        public bool IsOvertime { get; set; }
        
    }
}
