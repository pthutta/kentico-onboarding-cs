using Unity;

namespace TodoApp.Contracts.Bootstraps
{
    public interface IBootstrap
    {
        IUnityContainer RegisterTypes(IUnityContainer container);
    }
}
