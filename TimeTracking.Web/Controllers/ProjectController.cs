using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeTracking.Core.Interfaces;

namespace TimeTracking.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectQueryService _service;

        public ProjectController(IProjectQueryService service)
        {
            _service = service;
        }

        public ActionResult Index(int page = 1)
        {
            int pageSize = 10;

            var result = _service.GetProjects(page, pageSize);

            return View(result);
        }
    }
}