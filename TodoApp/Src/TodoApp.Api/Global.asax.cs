using System.Web.Http;
using TodoApp.Dependency;

namespace TodoApp.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(RouteConfig.Register);
            GlobalConfiguration.Configure(JsonFormatterConfig.Register);
            GlobalConfiguration.Configure(DependencyConfig.Register);
        }
    }
}
