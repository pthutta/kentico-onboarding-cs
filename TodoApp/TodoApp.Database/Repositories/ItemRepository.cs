using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Contracts;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;

namespace TodoApp.DAL.Repositories
{
    public class ItemRepository : IItemRepository
    {
        internal static readonly IList<Item> Items = new List<Item>
        {
            new Item
            {
                Id = "1",
                Text = "Learn react"
            },
            new Item
            {
                Id = "2",
                Text = "Learn redux"
            },
            new Item
            {
                Id = "3",
                Text = "Write Web API"
            }
        };

        public async Task<IEnumerable<Item>> GetAllAsync()
            => await Task.FromResult(Items);

        public async Task<Item> GetByIdAsync(string id)
            => await Task.FromResult(Items.FirstOrDefault(i => i.Id == id));

        public async Task CreateAsync(Item item)
        {
            Items.Add(item);
        }

        public async Task DeleteAsync(Item item)
        {
            Items.Remove(item);
        }

        public async Task UpdateAsync(Item item)
        {
            var updatedItem = Items.FirstOrDefault(i => i.Id == item.Id);
            if (updatedItem != null)
            {
                updatedItem.Text = item.Text;
            }
        }
    }
}
