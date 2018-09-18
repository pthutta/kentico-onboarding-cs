using System.Net.Http;
using System.Web;
using TodoApp.Api.Services;
using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Services;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace TodoApp.Api
{
    public class ApiBootstrap : IBootstrap
    {
        public IUnityContainer RegisterTypes(IUnityContainer container) 
            => container
                .RegisterType<HttpRequestMessage>(GetHttpRequestMessageInjectionFactory())
                .RegisterType<IUrlService, UrlService>(new HierarchicalLifetimeManager());

        private static InjectionFactory GetHttpRequestMessageInjectionFactory()
            => new InjectionFactory(_ => HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage);
    }
}