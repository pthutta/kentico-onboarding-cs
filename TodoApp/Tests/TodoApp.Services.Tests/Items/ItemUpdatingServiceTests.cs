using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Contracts.Exceptions;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;
using TodoApp.Contracts.Wrappers;
using TodoApp.Services.Items;
using TodoApp.TestContracts.Extensions;
using TodoApp.TestContracts.Factories;
using TodoApp.TestContracts.Wrappers;

namespace TodoApp.Services.Tests.Items
{
    [TestFixture]
    public class ItemUpdatingServiceTests
    {
        private IDateTimeWrapper _dateTimeWrapper;
        private IItemRepository _itemRepository;
        private IItemObtainingService _itemObtainingService;
        private IItemUpdatingService _itemUpdatingService;

        [SetUp]
        public void SetUp()
        {
            _dateTimeWrapper = Substitute.For<IDateTimeWrapper>();
            _itemObtainingService = Substitute.For<IItemObtainingService>();
            _itemRepository = Substitute.For<IItemRepository>();

            _itemUpdatingService = new ItemUpdatingService(_itemObtainingService, _itemRepository, _dateTimeWrapper);
        }

        [Test]
        public async Task UpdateAsync_ExistingItem_UpdatesItem()
        {
            var currentTime = new DateTime(2018, 10, 1, 8, 42, 5);
            var id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312");
            var (originalItem, receivedItem, changedItem) = CreateItems(id, currentTime);

            _itemObtainingService.ExistsAsync(id).Returns(true);
            _itemObtainingService.GetById(id).Returns(originalItem);
            _dateTimeWrapper.CurrentDateTime().Returns(currentTime, DateTime.MinValue);

            await _itemUpdatingService.UpdateAsync(receivedItem);

            await _itemRepository.Received(1).UpdateAsync(ArgExtended.IsItem(changedItem));
        }

        [Test]
        public void UpdateAsync_NonexistentItem_ThrowsItemNotFoundException()
        {
            var creationTime = new DateTime(2018, 9, 27, 12, 33, 41);
            var id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312");
            var updatedItem = ItemFactory.CreateItem(id, "This is a text.", creationTime, creationTime);
            _itemObtainingService.ExistsAsync(id).Returns(false);

            Assert.Multiple(() =>
            {
                Assert.ThrowsAsync<ItemNotFoundException>(async () =>
                    await _itemUpdatingService.UpdateAsync(updatedItem)
                );
                _itemRepository.DidNotReceive().UpdateAsync(Arg.Any<Item>());
            });
        }

        private (Item, Item, Item) CreateItems(Guid id, DateTime currentTime)
        {
            var creationTime = new DateTime(2018, 9, 27, 12, 33, 41);
            var newText = "This is a text.";

            var originalItem = ItemFactory.CreateItem(id, "Some old text.", creationTime, creationTime);
            var receivedItem = ItemFactory.CreateItem(id, newText);
            var changedItem = ItemFactory.CreateItem(id, newText, creationTime, currentTime);

            return (originalItem, receivedItem, changedItem);
        }
    }
}