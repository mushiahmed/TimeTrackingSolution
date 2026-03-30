using TimeTracking.Core.Models;

namespace TimeTracking.Core.Interfaces
{
    public interface ITimeEntryCommandService
    {
        void Create(TimeEntry entry);

        void Update(TimeEntry entry);

        void Delete(int id);
    }
}
