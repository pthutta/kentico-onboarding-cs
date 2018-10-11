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
            item.Id = _guidWrapper.GenerateGuid;
            item.CreationTime = _dateTimeWrapper.CurrentDateTime;
            item.LastUpdateTime = item.CreationTime;

            return await _repository.CreateAsync(item);
        }
    }
}
