using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeTracking.Core.Interfaces;
using TimeTracking.Web.Models;
using TimeTracking.Web.Helpers;
using TimeTracking.Core.Models;
using System.Web;

namespace TimeTracking.Web.Controllers
{
    public class TimeEntryController : Controller
    {
        private readonly IEmployeeQueryService _employeeService;
        private readonly IProjectQueryService _projectService;
        private readonly ITimeEntryQueryService _timeentryService;

        public TimeEntryController(IEmployeeQueryService employeeService, IProjectQueryService projectService, ITimeEntryQueryService timeentryQueryService)
        {
            _employeeService = employeeService;
            _projectService = projectService;
            _timeentryService = timeentryQueryService;
        }

        public ActionResult Index(int? employeeId, int? projectId, DateTime? startDate, DateTime? endDate, int page = 1)
        {
            try
            {
                startDate = startDate.HasValue ? startDate : DateTime.Today;
                endDate = endDate.HasValue ? endDate : DateTime.Today;
                var result = _timeentryService.GetTimeEntries(employeeId, projectId, startDate, endDate, page, 10);

                ViewBag.Employees = _employeeService.GetEmployees();
                ViewBag.Projects = _projectService.GetProjects();
                result.EmplyeeId = employeeId;
                result.ProjectId = projectId;
                result.FromDate = startDate;
                result.ToDate = endDate;
                return View(result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, "TimeEntryController", "Index", new { employeeId, projectId, startDate, endDate, page });

                ViewBag.Error = "Unable to load time entries.";
                ViewBag.Employees = _employeeService.GetEmployees();
                ViewBag.Projects = _projectService.GetProjects();

                var empty = new PagedResult<TimeEntry>
                {
                    Items = new List<TimeEntry>(),
                    PageNumber = page,
                    PageSize = 10,
                    TotalRecords = 0,
                    EmplyeeId = employeeId,
                    ProjectId = projectId,
                    FromDate = startDate,
                    ToDate = endDate
                };

                return View(empty);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                var model = new TimeEntryViewModel
                {
                    EntryDate = DateTime.Today,
                    Employees = _employeeService.GetEmployees()
                        .Select(e => new SelectListItem
                        {
                            Value = e.Id.ToString(),
                            Text = e.FullName
                        }),

                    Projects = _projectService.GetProjects()
                        .Select(p => new SelectListItem
                        {
                            Value = p.Id.ToString(),
                            Text = p.Name
                        })
                };
                return View(model);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, "TimeEntryController", "Create > Get");

                var model = new TimeEntryViewModel
                {
                    EntryDate = DateTime.Today,
                    Employees = Enumerable.Empty<SelectListItem>(),
                    Projects = Enumerable.Empty<SelectListItem>()
                };

                ViewBag.Error = "Unable to open create page";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TimeEntryViewModel model)
        {
            if (model == null)
                return Json(new { success = false, error = "Invalid request." });

            if (!ModelState.IsValid)
            {
                LogHelper.LogMessage("Model state is not valid.", "TimeEntryController", "Create > Post");
                return Json(new { success = false, error = "An error occurred while saving the time entry." });
            }

            try
            {
                var rowData = new
                {
                    EmployeeId = model.EmployeeId,
                    ProjectId = model.ProjectId,
                    EntryDate = model.EntryDate,
                    Hours = model.Hours,
                    Notes = model.Notes,
                    Source = model.Source
                };
                var apiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiBaseUrl"] + "timeentries";

                using (var http = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(rowData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var resp = await http.PostAsync(apiUrl, content).ConfigureAwait(false);
                    var respBody = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (resp.IsSuccessStatusCode)
                    {
                        return Json(new { success = true });
                    }

                    string apiError = respBody;
                    try
                    {
                        var errObj = JsonConvert.DeserializeObject<dynamic>(respBody);
                        if (errObj != null && errObj.error != null)
                            apiError = (string)errObj.error;
                    }
                    catch { }

                    LogHelper.LogMessage($"An Error returned from Server: {apiError}", "TimeEntryController", "Create > Post", new { apiUrl, payload = rowData, status = resp.StatusCode });

                    return Json(new { success = false, error = apiError ?? "API returned an error." });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, "TimeEntryController", "Create > Post", model);
                return Json(new { success = false, error = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            try
            {
                var entry = _timeentryService.GetTimeEntryById(id);

                if (entry == null)
                    return HttpNotFound();

                var model = new TimeEntryViewModel
                {
                    Id = entry.Id,
                    EmployeeId = entry.EmployeeId,
                    ProjectId = entry.ProjectId,
                    EntryDate = entry.EntryDate,
                    Hours = entry.Hours,
                    Notes = entry.Notes,
                    Source = entry.Source,
                    Employees = _employeeService.GetEmployees()
                        .Select(e => new SelectListItem
                        {
                            Value = e.Id.ToString(),
                            Text = e.FullName
                        }),
                    Projects = _projectService.GetProjects()
                        .Select(p => new SelectListItem
                        {
                            Value = p.Id.ToString(),
                            Text = p.Name
                        })
                };

                return View(model);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, "TimeEntryController", "Update > Get");

                var model = new TimeEntryViewModel
                {
                    EntryDate = DateTime.Today,
                    Employees = Enumerable.Empty<SelectListItem>(),
                    Projects = Enumerable.Empty<SelectListItem>()
                };

                ViewBag.Error = "An error occurred while retrieving time entry ";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id, TimeEntryViewModel model)
        {
            if (model == null)
                return Json(new { success = false, error = "Invalid request." });

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, error = "An error occurred while updating the time entry." });
                }

            try
            {
                var rowData = new
                {
                    EmployeeId = model.EmployeeId,
                    ProjectId = model.ProjectId,
                    EntryDate = model.EntryDate,
                    Hours = model.Hours,
                    Notes = model.Notes,
                    Source = model.Source
                };
                var apiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiBaseUrl"] + $"timeentries/{id}";

                using (var http = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(rowData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var resp = await http.PutAsync(apiUrl, content).ConfigureAwait(false);
                    var respBody = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (resp.IsSuccessStatusCode)
                    {
                        return Json(new { success = true });
                    }

                    string apiError = respBody;
                    try
                    {
                        var errObj = JsonConvert.DeserializeObject<dynamic>(respBody);
                        if (errObj != null && errObj.error != null)
                            apiError = (string)errObj.error;
                    }
                    catch { }

                    LogHelper.LogMessage($"An Error occurred while updating time entry: {apiError}", "TimeEntryController", "Update > Post", new { apiUrl, payload = rowData, status = resp.StatusCode });

                    return Json(new { success = false, error = apiError ?? "An Error occurred while updating time entry." });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, "TimeEntryController", "Update > Post", model);
                return Json(new { success = false, error = "An error occurred while updating the time entry." });
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var apiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiBaseUrl"] + $"timeentries/{id}";

                using (var http = new HttpClient())
                {
                    var resp = await http.DeleteAsync(apiUrl).ConfigureAwait(false);
                    var respBody = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (resp.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }

                    string apiError = respBody;
                    try
                    {
                        var errObj = JsonConvert.DeserializeObject<dynamic>(respBody);
                        if (errObj != null && errObj.error != null)
                            apiError = (string)errObj.error;
                    }
                    catch { }

                    LogHelper.LogMessage($"An error occurred while deleting time entry: {apiError}", "TimeEntryController", "Delete", new { apiUrl, status = resp.StatusCode });

                    return Json(new { success = false, error = apiError ?? "An error occurred while deleting the time entry." });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, "TimeEntryController", "Delete", new { id });
                return Json(new { success = false, error = "An error occurred while deleting the time entry." });
            }
        }

        [HttpGet]
        public ActionResult EmployeeWeeklyReport(DateTime? fromDate, DateTime? toDate, int page = 1)
        {
            int pageSize = 10;
            var data = _timeentryService.GetEmployeeWeeklySummary(fromDate, toDate, page, pageSize);
            
            return View(data);
        }

        [HttpGet]
        public ActionResult ProjectWeeklyReport(DateTime? fromDate, DateTime? toDate, int page = 1)
        {
            int pageSize = 10;
            var data = _timeentryService.GetProjectWeeklySummary(fromDate, toDate, page, pageSize);
            return View(data);
        }
    }
}