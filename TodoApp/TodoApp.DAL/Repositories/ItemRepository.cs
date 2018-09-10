using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.DAL.Repositories
{
    public class ItemRepository : IItemRepository
    {
        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Item> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
