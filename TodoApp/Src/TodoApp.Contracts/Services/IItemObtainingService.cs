using System;
using System.Threading.Tasks;
using TodoApp.Contracts.Models;

namespace TodoApp.Contracts.Services
{
    public interface IItemObtainingService
    {
        Item GetById(Guid id);

        Task<bool> ExistsAsync(Guid id);
    }
}
