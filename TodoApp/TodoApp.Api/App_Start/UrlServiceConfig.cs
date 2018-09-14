using System.Net.Http;
using System.Web;
using TodoApp.Api.Services;
using TodoApp.Contracts.Configs;
using TodoApp.Contracts.Services;
using Unity;
using Unity.Injection;

namespace TodoApp.Api
{
    public class UrlServiceConfig : IConfig
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<HttpRequestMessage>(new InjectionFactory(_ => HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage))
                .RegisterType<IUrlService, UrlService>();
        }
    }
}