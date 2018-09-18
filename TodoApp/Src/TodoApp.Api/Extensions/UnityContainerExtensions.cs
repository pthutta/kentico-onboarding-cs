using TodoApp.Contracts.Bootstraps;
using Unity;

namespace TodoApp.Api.Extensions
{
    public static class UnityContainerExtensions
    {
        public static UnityContainer Register<TBootstrap>(this UnityContainer container) 
            where TBootstrap : IBootstrap, new()
        {
            new TBootstrap().Register(container);
            return container;
        }
    }
}