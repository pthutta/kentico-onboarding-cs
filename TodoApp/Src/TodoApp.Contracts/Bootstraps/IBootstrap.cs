using TodoApp.Contracts.Containers;

namespace TodoApp.Contracts.Bootstraps
{
    public interface IBootstrap
    {
        IDependencyContainer RegisterTypes(IDependencyContainer container);
    }
}
