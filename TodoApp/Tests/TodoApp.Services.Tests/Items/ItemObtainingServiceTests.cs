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
    public class ItemObtainingServiceTests
    {
        private IItemRepository _itemRepository;
        private IItemObtainingService _itemObtainingService;

        [SetUp]
        public void SetUp()
        {
            _itemRepository = Substitute.For<IItemRepository>();
            _itemObtainingService = new ItemObtainingService(_itemRepository);
        }

        [Test]
        public async Task GetItemByIdAsync_ReturnsItemWithId()
        {
            var item = new Item
            {
                Id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312"),
                Text = "This is a text.",
            };
            _itemRepository.GetByIdAsync(item.Id).Returns(item);

            var foundItem = await _itemObtainingService.GetByIdAsync(item.Id);
            await _itemObtainingService.GetByIdAsync(item.Id);
            
            Assert.Multiple(() =>
            {
                Assert.That(foundItem, Is.EqualTo(item).UsingItemComparer());
                _itemRepository.Received(1).GetByIdAsync(item.Id);
            });
        }

        [Test]
        public async Task GetItemByIdAsync_WhenNotFound_ReturnsNull()
        {
            var id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312");
            _itemRepository.GetByIdAsync(id).Returns((Item) null);

            var foundItem = await _itemObtainingService.GetByIdAsync(id);

            Assert.That(foundItem, Is.Null);
        }

        [Test]
        public async Task Exists_WhenFound_ReturnsTrue()
        {
            var item = new Item
            {
                Id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312"),
                Text = "This is a text.",
            };
            _itemRepository.GetByIdAsync(item.Id).Returns(item);

            var exists = await _itemObtainingService.Exists(item.Id);

            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task Exists_WhenNotFound_ReturnsFalse()
        {
            var id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312");
            _itemRepository.GetByIdAsync(id).Returns((Item) null);

            var exists = await _itemObtainingService.Exists(id);

            Assert.That(exists, Is.False);
        }
    }
}
