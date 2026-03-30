using System.Collections.Generic;
using TimeTracking.Core.Models;

namespace TimeTracking.Core.Interfaces
{
    public interface ITimeEntryCommandRepository
    {
        void Insert(TimeEntry entry);
        void Update(TimeEntry entry);
        void Delete(int id);
    }
}
