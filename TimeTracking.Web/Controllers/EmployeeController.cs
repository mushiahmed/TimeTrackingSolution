using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeTracking.Core.Interfaces;
using TimeTracking.Service;
using TimeTracking.Web.Models;

namespace TimeTracking.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeQueryService _service;

        public EmployeeController(IEmployeeQueryService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var data = _service.GetEmployees();

            var model = data.Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                EmployeeCode = e.EmployeeCode,
                FullName = e.FullName,
                IsActive = e.IsActive
            }).ToList();

            return View(model);
        }
    }
}