using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;

namespace TodoApp.Database.Repositories
{
    internal class ItemRepository : IItemRepository
    {
        private const string ItemsCollectionName = "items";

        private readonly IMongoCollection<Item> _items;

        public ItemRepository(IConnectionStringWrapper connectionStringWrapper)
        {
            var mongoUrl = new MongoUrl(connectionStringWrapper.DefaultConnectionString);
            var client = new MongoClient(mongoUrl);
            var database = client.GetDatabase(mongoUrl.DatabaseName);

            _items = database.GetCollection<Item>(ItemsCollectionName);
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
            => await _items.Find(FilterDefinition<Item>.Empty).ToListAsync();

        public async Task<Item> GetByIdAsync(Guid id)
            => await _items.Find(item => item.Id == id).FirstOrDefaultAsync();

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
