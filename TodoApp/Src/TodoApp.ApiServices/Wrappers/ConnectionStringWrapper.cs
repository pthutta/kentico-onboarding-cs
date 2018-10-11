using System.Configuration;
using TodoApp.Contracts.Wrappers;

namespace TodoApp.ApiServices.Wrappers
{
    public class ConnectionStringWrapper : IConnectionStringWrapper
    {
        public string DefaultConnectionString =>
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}
