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
        private readonly IDateTimeService _dateTimeService;
        private readonly IGuidService _guidService;

        private Item _cachedItem;

        public ItemService(IItemRepository repository, IDateTimeService dateTimeService, IGuidService guidService)
        {
            _repository = repository;
            _dateTimeService = dateTimeService;
            _guidService = guidService;
        }

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
            item.Id = _guidService.GenerateGuid;
            item.CreationTime = _dateTimeService.CurrentDateTime;
            item.LastUpdateTime = item.CreationTime;

            return await _repository.CreateAsync(item);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var foundItem = await _repository.GetByIdAsync(item.Id);
            if (foundItem == null)
            {
                return false;
            }

            item.LastUpdateTime = _dateTimeService.CurrentDateTime;

            await _repository.UpdateAsync(item);
            return true;
        }
    }
}
