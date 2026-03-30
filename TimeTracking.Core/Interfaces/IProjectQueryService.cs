using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeTracking.Core.Models;

namespace TimeTracking.Core.Interfaces
{
    public interface IProjectQueryService
    {

        IEnumerable<Project> GetProjects();
        PagedResult<Project> GetProjects(int pageNumber, int pageSize);
    }
}