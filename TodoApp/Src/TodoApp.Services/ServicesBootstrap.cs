using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Services;
using TodoApp.Services.Items;
using TodoApp.Services.Utils;

namespace TodoApp.Services
{
    public class ServicesBootstrap : IBootstrap
    {
        public IDependencyContainer RegisterTypes(IDependencyContainer container)
            => container
                .RegisterType<IGuidService, GuidService>()
                .RegisterType<IDateTimeService, DateTimeService>()
                .RegisterType<IItemService, ItemService>();
    }
}
