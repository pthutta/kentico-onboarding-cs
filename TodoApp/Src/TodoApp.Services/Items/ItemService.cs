using System;
using System.Threading.Tasks;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;

namespace TodoApp.Services.Items
{
    internal class ItemService : IItemService
    {
        private readonly IItemRepository _repository;
        private Item _cachedItem;

        public ItemService(IItemRepository repository)
            => _repository = repository;

        public async Task<Item> GetItemByIdAsync(Guid id)
        {
            if (_cachedItem?.Id != id)
            {
                _cachedItem = await _repository.GetByIdAsync(id);
            }

            return _cachedItem;
        }

        public async Task<Item> CreateItemAsync(Item item)
        {
            item.CreationTime = DateTime.Now;
            item.LastUpdateTime = DateTime.Now;

            return await _repository.CreateAsync(item);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            if (_repository.GetByIdAsync(item.Id) == null)
            {
                return false;
            }

            item.LastUpdateTime = DateTime.Now;

            await _repository.UpdateAsync(item);
            return true;
        }
    }
}
