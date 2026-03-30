//using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc5;
using TimeTracking.Core.Interfaces;
using TimeTracking.Service;
using TimeTracking.Data.Repository;
using Unity;

namespace TimeTracking.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Repository
            container.RegisterType<IEmployeeRepository, EmployeeRepository>();
            container.RegisterType<IProjectQueryRepository, ProjectQueryRepository>();
            container.RegisterType<ITimeEntryQueryRepository, TimeEntryQueryRepository>();

            // Services
            container.RegisterType<IEmployeeQueryService, EmployeeQueryService>();
            container.RegisterType<IProjectQueryService, ProjectQueryService>();
            container.RegisterType<ITimeEntryQueryService, TimeEntryQueryService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}