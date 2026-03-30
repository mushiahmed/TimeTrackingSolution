using System.Web.Http;
using Unity;
using Unity.WebApi;
using TimeTracking.Core.Interfaces;
using TimeTracking.Service;
using TimeTracking.Data.Repository;

namespace TimeTracking.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<ITimeEntryCommandService, TimeEntryCommandService>();
            container.RegisterType<ITimeEntryCommandRepository, TimeEntryCommandRepository>();

            GlobalConfiguration.Configuration.DependencyResolver =
                new UnityDependencyResolver(container);
        }
    }
}