using System;
using System.Configuration;
using TodoApp.Contracts.Repositories;

namespace TodoApp.ApiServices.Wrappers
{
    public class ConnectionStringWrapper : IConnectionStringWrapper
    {
        private readonly Lazy<string> _defaultConnectionString = new Lazy<string>(() => GetConnectionString("DefaultConnection"));

        public string DefaultConnectionString => _defaultConnectionString.Value;

        private static string GetConnectionString(string name)
            => ConfigurationManager.ConnectionStrings[name].ConnectionString;
    }
}
