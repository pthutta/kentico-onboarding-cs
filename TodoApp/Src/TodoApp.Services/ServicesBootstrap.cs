using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Enums;
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
                .RegisterType<IGuidWrapper, GuidWrapper>(LifetimeManagerType.SingletonPerApplication)
                .RegisterType<IDateTimeWrapper, DateTimeWrapper>(LifetimeManagerType.SingletonPerApplication)
                .RegisterType<IItemObtainingService, ItemObtainingService>()
                .RegisterType<IItemCreatingService, ItemCreatingService>()
                .RegisterType<IItemUpdatingService, ItemUpdatingService>();
    }
}
