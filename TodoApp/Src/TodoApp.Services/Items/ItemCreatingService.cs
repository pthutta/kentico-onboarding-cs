using System.Threading.Tasks;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;
using TodoApp.Contracts.Wrappers;

namespace TodoApp.Services.Items
{
    internal class ItemCreatingService : IItemCreatingService
    {
        private readonly IItemRepository _repository;
        private readonly IDateTimeWrapper _dateTimeWrapper;
        private readonly IGuidWrapper _guidWrapper;

        public ItemCreatingService(IItemRepository repository, IDateTimeWrapper dateTimeWrapper, IGuidWrapper guidWrapper)
        {
            _repository = repository;
            _dateTimeWrapper = dateTimeWrapper;
            _guidWrapper = guidWrapper;
        }

        public async Task<Item> CreateAsync(Item item)
        {
            var currentTime = _dateTimeWrapper.CurrentDateTime();
            var id = _guidWrapper.GenerateGuid();
            var newItem = new Item
            {
                Id = id,
                Text = item.Text,
                CreationTime = currentTime,
                LastUpdateTime = currentTime
            };

            return await _repository.CreateAsync(newItem);
        }
    }
}
