using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;

namespace TodoApp.Database.Repositories
{
    internal class ItemRepository : IItemRepository
    {
        private static readonly Item[] Items =
        {
            new Item
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Text = "Learn react"
            },
            new Item
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Text = "Learn redux"
            },
            new Item
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Text = "Write Web API"
            },
            new Item
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                Text = "Write dummier controller"
            }
        };

        public async Task<IEnumerable<Item>> GetAllAsync()
            => await Task.FromResult(Items);

        public async Task<Item> GetByIdAsync(Guid id)
            => await Task.FromResult(Items[0]);

        public async Task<Item> CreateAsync(Item item)
            => await Task.FromResult(Items[1]);

        public async Task<Item> DeleteAsync(Guid id)
            => await Task.FromResult(Items[2]);

        public async Task UpdateAsync(Item item)
        {
            await Task.CompletedTask;
        }
    }
}
