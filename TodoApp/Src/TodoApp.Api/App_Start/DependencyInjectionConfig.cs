using System.Web.Http;
using TodoApp.Dependency;

namespace TodoApp.Api
{
    public static class DependencyConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.DependencyResolver = UnityConfig.GetDependencyResolver();
        }
    }
}