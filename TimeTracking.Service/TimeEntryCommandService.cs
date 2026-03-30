using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Core.Interfaces;
using TimeTracking.Core.Models;

namespace TimeTracking.Service
{
    public class TimeEntryCommandService : ITimeEntryCommandService
    {
        private readonly ITimeEntryCommandRepository _repository;

        public TimeEntryCommandService(ITimeEntryCommandRepository repository)
        {
            _repository = repository;
        }

        public void Create(TimeEntry entry)
        {
            _repository.Insert(entry);
        }

        public void Update(TimeEntry entry)
        {
            _repository.Update(entry);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
