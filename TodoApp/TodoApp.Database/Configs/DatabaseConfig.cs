﻿using TodoApp.Contracts.Configs;
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