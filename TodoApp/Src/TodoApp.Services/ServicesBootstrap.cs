using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Services;
using TodoApp.Contracts.Wrappers;
using TodoApp.Services.Items;
using TodoApp.Services.Wrappers;

namespace TodoApp.Services
{
    public class ServicesBootstrap : IBootstrap
    {
        public IDependencyContainer RegisterTypes(IDependencyContainer container)
            => container
                .RegisterType<IGuidWrapper, GuidWrapper>(Lifecycle.SingletonPerApplication)
                .RegisterType<IDateTimeWrapper, DateTimeWrapper>(Lifecycle.SingletonPerApplication)
                .RegisterType<IItemObtainingService, ItemObtainingService>(Lifecycle.SingletonPerRequest)
                .RegisterType<IItemCreatingService, ItemCreatingService>(Lifecycle.SingletonPerRequest)
                .RegisterType<IItemUpdatingService, ItemUpdatingService>(Lifecycle.SingletonPerRequest);
    }
}
