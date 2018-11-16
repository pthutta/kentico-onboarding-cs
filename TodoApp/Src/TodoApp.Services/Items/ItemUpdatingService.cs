using System.Threading.Tasks;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;
using TodoApp.Contracts.Wrappers;

namespace TodoApp.Services.Items
{
    internal class ItemUpdatingService : IItemUpdatingService
    {
        private readonly IItemObtainingService _itemObtainingService;
        private readonly IItemRepository _repository;
        private readonly IDateTimeWrapper _dateTimeWrapper;

        public ItemUpdatingService(IItemObtainingService itemObtainingService, IItemRepository repository, IDateTimeWrapper dateTimeWrapper)
        {
            _itemObtainingService = itemObtainingService;
            _repository = repository;
            _dateTimeWrapper = dateTimeWrapper;
        }

        public async Task UpdateAsync(Item item)
        {
            if (!await _itemObtainingService.ExistsAsync(item.Id))
            {
                throw new ItemNotFoundException("Item with id=" + item.Id + " was not found.");
            }

            var originalItem = _itemObtainingService.GetById(item.Id);
            UpdateExistingItem(originalItem, item);

            await _repository.UpdateAsync(originalItem);
        }

        private void UpdateExistingItem(Item existingItem, Item updatedItem)
        {
            existingItem.LastUpdateTime = _dateTimeWrapper.CurrentDateTime();
            existingItem.Text = updatedItem.Text;
        }
    }
}
