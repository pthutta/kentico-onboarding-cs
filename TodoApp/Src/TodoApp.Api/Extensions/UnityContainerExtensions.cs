using TodoApp.Contracts.Configs;
using Unity;

namespace TodoApp.Api.Extensions
{
    public static class UnityContainerExtensions
    {
        public static UnityContainer Register<TConfig>(this UnityContainer container) 
            where TConfig : IConfig, new()
        {
            new TConfig().Register(container);
            return container;
        }
    }
}