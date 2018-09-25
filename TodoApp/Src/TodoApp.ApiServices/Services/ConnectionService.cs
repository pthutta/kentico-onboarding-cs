using System.Configuration;
using TodoApp.Contracts.Services;

namespace TodoApp.ApiServices.Services
{
    public class ConnectionService : IConnectionService
    {
        public string DefaultConnectionString =>
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}
