﻿using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Services;
using TodoApp.Services.Items;

namespace TodoApp.Services
{
    public class ServicesBootstrap : IBootstrap
    {
        public IDependencyContainer RegisterTypes(IDependencyContainer container)
            => container.RegisterType<IItemService, ItemService>();
    }
}