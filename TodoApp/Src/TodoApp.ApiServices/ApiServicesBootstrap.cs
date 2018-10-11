using System.Net.Http;
using System.Web;
using TodoApp.ApiServices.Services;
using TodoApp.ApiServices.Wrappers;
using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Enums;
using TodoApp.Contracts.Services;
using TodoApp.Contracts.Wrappers;

namespace TodoApp.ApiServices
{
    public class ApiServicesBootstrap : IBootstrap
    {
        public IDependencyContainer RegisterTypes(IDependencyContainer container) 
            => container
                .RegisterType(() => HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage)
                .RegisterType<IConnectionStringWrapper, ConnectionStringWrapper>(LifetimeManagerType.SingletonPerApplication)
                .RegisterType<IUrlService, UrlService>();
    }
}