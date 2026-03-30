using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Core.Interfaces;
using TimeTracking.Core.Models;

namespace TimeTracking.Service
{
    public class ProjectQueryService : IProjectQueryService
    {
        private readonly IProjectQueryRepository _repo;

        public ProjectQueryService(IProjectQueryRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Project> GetProjects()
        {
            return _repo.GetAllProjects();
        }
        public PagedResult<Project> GetProjects(int page, int pageSize)
        {
            return _repo.GetProjects(page, pageSize);
        }
    }
}
