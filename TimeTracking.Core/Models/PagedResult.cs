using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Core.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }

        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }

        public int PageSize { get; set; }
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? EmplyeeId { get; set; }
        public int? ProjectId { get; set; }
    }
}
