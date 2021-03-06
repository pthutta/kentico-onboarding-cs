﻿using System;
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
        private Guid _cachedId;

        public ItemObtainingService(IItemRepository repository)
            => _repository = repository;

        public Item GetById(Guid id)
        {
            if (_cachedItem == null)
            {
                throw new InvalidOperationException("Please, call ExistsAsync(Guid id) method before GetById(Guid id).");
            }

            return _cachedItem;
        }

        public async Task<bool> ExistsAsync(Guid id)
            => await GetItemAsync(id) != null;

        private async Task<Item> GetItemAsync(Guid id)
        {
            if (_cachedItem?.Id != id && _cachedId != id)
            {
                _cachedId = id;
                _cachedItem = await _repository.GetByIdAsync(id);
            }

            return _cachedItem;
        }
    }
}
