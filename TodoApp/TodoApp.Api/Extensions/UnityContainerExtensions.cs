﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TodoApp.Contracts.Configs;
using Unity;

namespace TodoApp.Api.Extensions
{
    public static class UnityContainerExtensions
    {
        public static UnityContainer RegisterDatabase<TDatabase>(this UnityContainer container) 
            where TDatabase : IDatabaseConfig, new()
        {
            new TDatabase().Register(container);
            return container;
        }
    }
}