using System.Net.Http;
using System.Web;
using TodoApp.ApiServices.Services;
using TodoApp.ApiServices.Wrappers;
using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Routes;

namespace TodoApp.ApiServices
{
    public class ApiServicesBootstrap : IBootstrap
    {
        public IDependencyContainer RegisterTypes(IDependencyContainer container) 
            => container
                .RegisterType(CurrentHttpRequestMessage, Lifecycle.SingletonPerRequest)
                .RegisterType<IConnectionStringWrapper, ConnectionStringWrapper>(Lifecycle.SingletonPerApplication)
                .RegisterType<IUrlService, UrlService>(Lifecycle.SingletonPerRequest);

        private static HttpRequestMessage CurrentHttpRequestMessage()
            => HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
    }
}