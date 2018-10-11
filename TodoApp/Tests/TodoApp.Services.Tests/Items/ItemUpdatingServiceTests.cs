using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Api.Tests.Extensions;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;
using TodoApp.Services.Items;

namespace TodoApp.Services.Tests.Items
{
    [TestFixture]
    public class ItemUpdatingServiceTests
    {
        private IDateTimeService _dateTimeService;
        private IItemRepository _itemRepository;
        private IItemObtainingService _itemObtainingService;
        private IItemUpdatingService _itemUpdatingService;

        [SetUp]
        public void SetUp()
        {
            _dateTimeService = Substitute.For<IDateTimeService>();
            _dateTimeService.CurrentDateTime.Returns(new DateTime(2018, 9, 27));

            _itemObtainingService = Substitute.For<IItemObtainingService>();
            _itemRepository = Substitute.For<IItemRepository>();

            _itemUpdatingService = new ItemUpdatingService(_itemObtainingService, _itemRepository, _dateTimeService);
        }

        [Test]
        public async Task UpdateItemAsync_UpdatesItem()
        {
            var updatedItem = new Item
            {
                Id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312"),
                Text = "This is a text.",
                CreationTime = _dateTimeService.CurrentDateTime,
                LastUpdateTime = _dateTimeService.CurrentDateTime
            };
            var currentTime = new DateTime(2018, 10, 1);

            _itemObtainingService.Exists(updatedItem.Id).Returns(true);
            _itemObtainingService.GetByIdAsync(updatedItem.Id).Returns(updatedItem);
            _dateTimeService.CurrentDateTime.Returns(currentTime);

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
                CreationTime = _dateTimeService.CurrentDateTime,
                LastUpdateTime = _dateTimeService.CurrentDateTime
            };

            _itemObtainingService.Exists(updatedItem.Id).Returns(false);

            await _itemUpdatingService.UpdateAsync(updatedItem);

            await _itemRepository.Received(0).UpdateAsync(updatedItem);
        }
    }
}