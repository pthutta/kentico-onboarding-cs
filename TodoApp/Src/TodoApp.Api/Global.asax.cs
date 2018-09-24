using System.Web.Http;
using TodoApp.Api.Routes;
using TodoApp.Dependency;

namespace TodoApp.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(RouteConfig.Register);
            GlobalConfiguration.Configure(JsonFormatterConfig.Register);

            RegisterDependencies();
        }

        private static void RegisterDependencies()
        {
            var dependencyConfig = CreateDependencyConfig();
            GlobalConfiguration.Configure(dependencyConfig.Register);
        }

        private static DependencyConfig CreateDependencyConfig()
        {
            var routeNames = new RouteNames();
            return new DependencyConfig(routeNames);
        }
    }
}
