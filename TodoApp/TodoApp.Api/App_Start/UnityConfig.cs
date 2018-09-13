using System.Net.Http;
using System.Web;
using System.Web.Http;
using TodoApp.Api.Extensions;
using TodoApp.Api.Resolvers;
using TodoApp.Api.Services;
using TodoApp.Contracts.Services;
using TodoApp.Database.Configs;
using Unity;
using Unity.Injection;

namespace TodoApp.Api
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer()
                .RegisterDatabase<DatabaseConfig>()
                .RegisterType<HttpRequestMessage>(new InjectionFactory(_ => HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage))
                .RegisterType<IUrlService, UrlService>();
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}