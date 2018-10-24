using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;
using TodoApp.Services.Items;
using TodoApp.TestContracts.Extensions;
using TodoApp.TestContracts.Factories;

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
        public async Task GetById_ExistingItem_ReturnsItemWithIdAndCachesResult()
        {
            var id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312");
            var item = ItemFactory.CreateItem(id, "This is a text.");
            _itemRepository.GetByIdAsync(id).Returns(item);

            await _itemObtainingService.ExistsAsync(id);
            var firstItem = _itemObtainingService.GetById(id);
            var secondItem = _itemObtainingService.GetById(id);
            
            Assert.Multiple(() =>
            {
                Assert.That(firstItem, Is.EqualTo(item).UsingItemComparer());
                Assert.That(secondItem, Is.EqualTo(firstItem));
                _itemRepository.Received(1).GetByIdAsync(id);
            });
        }

        [Test]
        public void GetById_NotCallingExistsAsync_ThrowsInvalidOperationException()
        {
            var id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312");

            Assert.Throws<InvalidOperationException>(() =>
                _itemObtainingService.GetById(id)
            );
        }

        [Test]
        public async Task ExistsAsync_ExistingItem_ReturnsTrue()
        {
            var id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312");
            var item = ItemFactory.CreateItem(id, "This is a text.");
            _itemRepository.GetByIdAsync(id).Returns(item);

            var existsFirst = await _itemObtainingService.ExistsAsync(id);
            var existsSecond = await _itemObtainingService.ExistsAsync(id);

            Assert.Multiple(() =>
            {
                Assert.That(existsFirst, Is.True);
                Assert.That(existsSecond, Is.True);
                _itemRepository.Received(1).GetByIdAsync(id);
            });
        }

        [Test]
        public async Task ExistsAsync_NonexistentItem_ReturnsFalse()
        {
            var id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312");
            _itemRepository.GetByIdAsync(id).Returns((Item) null);

            var existsFirst = await _itemObtainingService.ExistsAsync(id);
            var existsSecond = await _itemObtainingService.ExistsAsync(id);

            Assert.Multiple(() =>
            {
                Assert.That(existsFirst, Is.False);
                Assert.That(existsSecond, Is.False);
                _itemRepository.Received(1).GetByIdAsync(id);
            });
        }
    }
}
