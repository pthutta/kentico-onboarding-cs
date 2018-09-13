using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TodoApp.Contracts.Configs;
using TodoApp.Contracts.Repositories;
using TodoApp.Database.Repositories;
using Unity;
using Unity.Lifetime;

namespace TodoApp.Database.Configs
{
    public class DatabaseConfig : IDatabaseConfig
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<IItemRepository, ItemRepository>(new HierarchicalLifetimeManager());
        }
    }
}
