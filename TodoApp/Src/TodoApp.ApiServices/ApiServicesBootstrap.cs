using System.Net.Http;
using System.Web;
using TodoApp.ApiServices.Services;
using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Services;

namespace TodoApp.ApiServices
{
    public class ApiServicesBootstrap : IBootstrap
    {
        public IDependencyContainer RegisterTypes(IDependencyContainer container) 
            => container
                .RegisterType(() => HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage)
                .RegisterType<IConnectionService, ConnectionService>()
                .RegisterType<IUrlService, UrlService>();
    }
}