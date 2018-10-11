using System.Threading.Tasks;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;

namespace TodoApp.Services.Items
{
    internal class ItemUpdatingService : IItemUpdatingService
    {
        private readonly IItemObtainingService _itemObtainingService;
        private readonly IItemRepository _repository;
        private readonly IDateTimeService _dateTimeService;

        public ItemUpdatingService(IItemObtainingService itemObtainingService, IItemRepository repository, IDateTimeService dateTimeService)
        {
            _itemObtainingService = itemObtainingService;
            _repository = repository;
            _dateTimeService = dateTimeService;
        }

        public async Task UpdateAsync(Item item)
        {
            if (!await _itemObtainingService.Exists(item.Id))
            {
                return;
            }
            var foundItem = await _itemObtainingService.GetByIdAsync(item.Id);

            foundItem.LastUpdateTime = _dateTimeService.CurrentDateTime;
            foundItem.Text = item.Text;

            await _repository.UpdateAsync(foundItem);
        }
    }
}
