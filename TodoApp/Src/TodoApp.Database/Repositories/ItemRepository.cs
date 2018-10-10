using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;

namespace TodoApp.Database.Repositories
{
    internal class ItemRepository : IItemRepository
    {
        private const string ItemsCollection = "items";

        private readonly IMongoCollection<Item> _items;

        public ItemRepository(IConnectionService connectionService)
        {
            var mongoUrl = new MongoUrl(connectionService.DefaultConnectionString);
            var client = new MongoClient(mongoUrl);
            var database = client.GetDatabase(mongoUrl.DatabaseName);

            _items = database.GetCollection<Item>(ItemsCollection);
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
            => await _items.Find(FilterDefinition<Item>.Empty).ToListAsync();

        public async Task<Item> GetByIdAsync(Guid id)
            => await _items.Find(item => item.Id == id).FirstAsync();

        public async Task<Item> CreateAsync(Item item)
        {
            await _items.InsertOneAsync(item);
            return item;
        }

        public async Task<Item> DeleteAsync(Guid id)
            => await _items.FindOneAndDeleteAsync(item => item.Id == id);

        public async Task UpdateAsync(Item item)
            => await _items.FindOneAndReplaceAsync(dbItem => dbItem.Id == item.Id, item);
    }
}
