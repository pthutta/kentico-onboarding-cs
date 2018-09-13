using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Contracts.Models;

namespace TodoApp.Contracts.Repositories
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllAsync();

        Task<Item> GetByIdAsync(Guid id);

        Task<Item> CreateAsync(Item item);

        Task<Item> DeleteAsync(Guid id);

        Task UpdateAsync(Item item);
    }
}
