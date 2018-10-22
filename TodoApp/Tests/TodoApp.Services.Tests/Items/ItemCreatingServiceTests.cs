using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Api.Tests.Extensions;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;
using TodoApp.Contracts.Wrappers;
using TodoApp.Services.Items;

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
            _dateTimeWrapper.CurrentDateTime().Returns(new DateTime(2018, 9, 27));

            _guidWrapper = Substitute.For<IGuidWrapper>();
            _guidWrapper.GenerateGuid().Returns(Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312"));

            _itemRepository = Substitute.For<IItemRepository>();

            _itemCreatingService = new ItemCreatingService(_itemRepository, _dateTimeWrapper, _guidWrapper);
        }

        [Test]
        public async Task CreateItemAsync_ReturnsCorrectCreatedItem()
        {
            var expectedItem = new Item
            {
                Id = _guidWrapper.GenerateGuid(),
                Text = "This is a text.",
                CreationTime = _dateTimeWrapper.CurrentDateTime(),
                LastUpdateTime = _dateTimeWrapper.CurrentDateTime()
            };
            var item = new Item
            {
                Id = expectedItem.Id,
                Text = "This is a text.",
                CreationTime = _dateTimeWrapper.CurrentDateTime(),
                LastUpdateTime = _dateTimeWrapper.CurrentDateTime()
            };
            _itemRepository.CreateAsync(item).Returns(expectedItem);

            var createdItem = await _itemCreatingService.CreateAsync(item);

            Assert.That(createdItem, Is.EqualTo(expectedItem).UsingItemComparer());
        }
    }
}