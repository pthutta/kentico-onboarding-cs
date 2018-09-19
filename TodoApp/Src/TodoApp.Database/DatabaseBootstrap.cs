using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Repositories;
using TodoApp.Database.Repositories;

namespace TodoApp.Database
{
    public class DatabaseBootstrap : IBootstrap
    {
        public IDependencyContainer RegisterTypes(IDependencyContainer container)
            => container.RegisterType<IItemRepository, ItemRepository>();
    }
}
