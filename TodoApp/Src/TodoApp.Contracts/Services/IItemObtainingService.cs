using System;
using System.Threading.Tasks;
using TodoApp.Contracts.Models;

namespace TodoApp.Contracts.Services
{
    public interface IItemObtainingService
    {
        Task<Item> GetByIdAsync(Guid id);

        Task<bool> Exists(Guid id);
    }
}
