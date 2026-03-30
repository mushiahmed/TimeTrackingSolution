using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TimeTracking.Core.Interfaces;
using TimeTracking.Core.Models;
using TimeTracking.Data.Repository;
using TimeTracking.Service;


namespace TimeTracking.API.Controllers
{
    [RoutePrefix("api/timeentries")]
    public class TimeEntriesController : ApiController
    {
        private readonly ITimeEntryCommandService _service;

        public TimeEntriesController(ITimeEntryCommandService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
           
            return Ok("API is running!");
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(TimeEntry entry)
        {
            _service.Create(entry);
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, TimeEntry entry)
        {
            entry.Id = id;
            _service.Update(entry);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }

    }
}