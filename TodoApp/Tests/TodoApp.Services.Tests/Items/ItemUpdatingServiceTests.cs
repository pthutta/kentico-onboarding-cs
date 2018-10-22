using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;
using TodoApp.Contracts.Wrappers;
using TodoApp.Services.Items;

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
            _dateTimeWrapper.CurrentDateTime().Returns(new DateTime(2018, 9, 27));

            _itemObtainingService = Substitute.For<IItemObtainingService>();
            _itemRepository = Substitute.For<IItemRepository>();

            _itemUpdatingService = new ItemUpdatingService(_itemObtainingService, _itemRepository, _dateTimeWrapper);
        }

        [Test]
        public async Task UpdateItemAsync_UpdatesItem()
        {
            var updatedItem = new Item
            {
                Id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312"),
                Text = "This is a text.",
                CreationTime = _dateTimeWrapper.CurrentDateTime(),
                LastUpdateTime = _dateTimeWrapper.CurrentDateTime()
            };
            var currentTime = new DateTime(2018, 10, 1);

            _itemObtainingService.ExistsAsync(updatedItem.Id).Returns(true);
            _itemObtainingService.GetById(updatedItem.Id).Returns(updatedItem);
            _dateTimeWrapper.CurrentDateTime().Returns(currentTime);

            await _itemUpdatingService.UpdateAsync(updatedItem);

            Assert.Multiple(() =>
            {
                Assert.That(updatedItem.LastUpdateTime, Is.EqualTo(currentTime));
                _itemRepository.Received().UpdateAsync(updatedItem);
            });
        }

        [Test]
        public async Task UpdateItemAsync_WhenNotFound_Returns()
        {
            var updatedItem = new Item
            {
                Id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312"),
                Text = "This is a text.",
                CreationTime = _dateTimeWrapper.CurrentDateTime(),
                LastUpdateTime = _dateTimeWrapper.CurrentDateTime()
            };

            _itemObtainingService.ExistsAsync(updatedItem.Id).Returns(false);

            await _itemUpdatingService.UpdateAsync(updatedItem);

            await _itemRepository.Received(0).UpdateAsync(updatedItem);
        }
    }
}