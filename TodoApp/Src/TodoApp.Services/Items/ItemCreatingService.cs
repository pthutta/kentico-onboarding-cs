using System.Threading.Tasks;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;

namespace TodoApp.Services.Items
{
    internal class ItemCreatingService : IItemCreatingService
    {
        private readonly IItemRepository _repository;
        private readonly IDateTimeService _dateTimeService;
        private readonly IGuidService _guidService;

        public ItemCreatingService(IItemRepository repository, IDateTimeService dateTimeService, IGuidService guidService)
        {
            _repository = repository;
            _dateTimeService = dateTimeService;
            _guidService = guidService;
        }

        public async Task<Item> CreateAsync(Item item)
        {
            item.Id = _guidService.GenerateGuid;
            item.CreationTime = _dateTimeService.CurrentDateTime;
            item.LastUpdateTime = item.CreationTime;

            return await _repository.CreateAsync(item);
        }
    }
}
