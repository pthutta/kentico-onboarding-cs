using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
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
    public class ItemCreatingServiceTests
    {
        private IDateTimeWrapper _dateTimeWrapper;
        private IGuidWrapper _guidWrapper;
        private IItemRepository _itemRepository;
        private IItemCreatingService _itemCreatingService;

        [SetUp]
        public void SetUp()
        {
            _dateTimeWrapper = Substitute.For<IDateTimeWrapper>();
            _guidWrapper = Substitute.For<IGuidWrapper>();
            _itemRepository = Substitute.For<IItemRepository>();

            _itemCreatingService = new ItemCreatingService(_itemRepository, _dateTimeWrapper, _guidWrapper);
        }

        [Test]
        public async Task CreateAsync_ReturnsCorrectCreatedItem()
        {
            var id = Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312");
            var currentTime = new DateTime(2018, 9, 27, 18, 32, 54);
            var (receivedItem, expectedItem) = MockRepository(id, currentTime);
            MockWrappers(id, currentTime);

            var actualItem = await _itemCreatingService.CreateAsync(receivedItem);

            Assert.That(actualItem, Is.EqualTo(expectedItem).UsingItemComparer());
        }

        private (Item, Item) MockRepository(Guid id, DateTime currentTime)
        {
            var receivedItem = ItemFactory.CreateItem(Guid.Empty, "This is a text.");
            var expectedItem = ItemFactory.CreateItem(id, receivedItem.Text, currentTime, currentTime);

            _itemRepository.CreateAsync(ArgExtended.IsItem(expectedItem)).Returns(expectedItem);

            return (receivedItem, expectedItem);
        }

        private void MockWrappers(Guid id, DateTime currentTime)
        {
            _dateTimeWrapper.CurrentDateTime().Returns(currentTime, DateTime.MinValue);
            _guidWrapper.GenerateGuid().Returns(id, Guid.Empty);
        }
    }
}