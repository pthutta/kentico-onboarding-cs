using Unity;

namespace TodoApp.Contracts.Bootstraps
{
    public interface IBootstrap
    {
        void Register(IUnityContainer container);
    }
}
