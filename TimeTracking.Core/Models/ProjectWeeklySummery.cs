using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Core.Models
{
    public class ProjectWeeklySummary
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int WeekNumber { get; set; }
        public int Year { get; set; }
        public decimal TotalHours { get; set; }
    }
}
