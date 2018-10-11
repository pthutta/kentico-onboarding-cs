using System;
using System.Threading.Tasks;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;

namespace TodoApp.Services.Items
{
    internal class ItemObtainingService : IItemObtainingService
    {
        private readonly IItemRepository _repository;

        private Item _cachedItem;

        public ItemObtainingService(IItemRepository repository)
            => _repository = repository;

        public async Task<Item> GetByIdAsync(Guid id)
        {
            if (_cachedItem?.Id != id)
            {
                _cachedItem = await _repository.GetByIdAsync(id);
            }

            return _cachedItem;
        }

        public async Task<bool> Exists(Guid id)
        {
            return await GetByIdAsync(id) != null;
        }
    }
}
