using System;
using TodoApp.Contracts.Models;

namespace TodoApp.TestContracts.Factories
{
    public static class ItemFactory
    {
        public static Item CreateItem(string id, string text, DateTime? creationTime = null, DateTime? lastUpdateTime = null)
            => CreateItem(Guid.Parse(id), text, creationTime, lastUpdateTime);

        public static Item CreateItem(Guid id, string text, DateTime? creationTime = null, DateTime? lastUpdateTime = null)
            => new Item
            {
                Id = id,
                Text = text,
                CreationTime = creationTime ?? DateTime.MinValue,
                LastUpdateTime = lastUpdateTime ?? DateTime.MinValue
            };
    }
}
