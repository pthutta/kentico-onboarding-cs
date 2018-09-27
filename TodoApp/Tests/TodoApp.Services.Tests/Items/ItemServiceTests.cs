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
    public class ItemServiceTests
    {
        private IDateTimeService _dateTimeService;
        private IGuidService _guidService;
        private IItemRepository _itemRepository;
        private IItemService _itemService;

        [SetUp]
        public void SetUp()
        {
            _dateTimeService = Substitute.For<IDateTimeService>();
            _dateTimeService.CurrentDateTime.Returns(new DateTime(2018, 9, 27));

            _guidService = Substitute.For<IGuidService>();
            _guidService.GenerateGuid.Returns(Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312"));

            _itemRepository = Substitute.For<IItemRepository>();

            _itemService = new ItemService(_itemRepository, _dateTimeService, _guidService);
        }

        [Test]
        public async Task GetItemByIdAsync_ReturnsItemWithId()
        {
            var item = new Item
            {
                Id = _guidService.GenerateGuid,
                Text = "This is a text.",
                CreationTime = _dateTimeService.CurrentDateTime,
                LastUpdateTime = _dateTimeService.CurrentDateTime
            };
            _itemRepository.GetByIdAsync(item.Id).Returns(item);

            var foundItem = await _itemService.GetItemByIdAsync(item.Id);

            Assert.That(foundItem, Is.EqualTo(item).UsingItemComparer());
        }

        [Test]
        public async Task CreateItemAsync_ReturnsCorrectCreatedItem()
        {
            var expectedItem = new Item
            {
                Id = _guidService.GenerateGuid,
                Text = "This is a text.",
                CreationTime = _dateTimeService.CurrentDateTime,
                LastUpdateTime = _dateTimeService.CurrentDateTime
            };
            var item = new Item
            {
                Id = expectedItem.Id,
                Text = "This is a text.",
                CreationTime = _dateTimeService.CurrentDateTime,
                LastUpdateTime = _dateTimeService.CurrentDateTime
            };
            _itemRepository.CreateAsync(item).Returns(expectedItem);

            var createdItem = await _itemService.CreateItemAsync(item);

            Assert.That(createdItem, Is.EqualTo(expectedItem).UsingItemComparer());
        }

        [Test]
        public async Task UpdateItemAsync_ReturnsTrue()
        {
            var updatedItem = new Item
            {
                Id = _guidService.GenerateGuid,
                Text = "This is a text.",
                CreationTime = _dateTimeService.CurrentDateTime,
                LastUpdateTime = _dateTimeService.CurrentDateTime
            };
            var currentTime = new DateTime(2018, 10, 1);

            _dateTimeService.CurrentDateTime.Returns(currentTime);
            _itemRepository.GetByIdAsync(updatedItem.Id).Returns(updatedItem);

            var updated = await _itemService.UpdateItemAsync(updatedItem);

            Assert.Multiple(() =>
            {
                Assert.That(updated, Is.True);
                Assert.That(updatedItem.LastUpdateTime, Is.EqualTo(currentTime));
                _itemRepository.Received().UpdateAsync(updatedItem);
            });
        }

        [Test]
        public async Task UpdateItemAsync_ReturnsFalse()
        {
            var updatedItem = new Item
            {
                Id = _guidService.GenerateGuid,
                Text = "This is a text.",
                CreationTime = _dateTimeService.CurrentDateTime,
                LastUpdateTime = _dateTimeService.CurrentDateTime
            };

            _itemRepository.GetByIdAsync(updatedItem.Id).Returns((Item) null);

            var updated = await _itemService.UpdateItemAsync(updatedItem);

            Assert.That(updated, Is.False);
        }
    }
}
