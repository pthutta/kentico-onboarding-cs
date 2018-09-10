using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.DAL.Repositories
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllAsync();

        Task<Item> GetByIdAsync(string id);

        Task CreateAsync(Item item);

        Task DeleteAsync(Item item);

        Task UpdateAsync(Item item);
    }
}
