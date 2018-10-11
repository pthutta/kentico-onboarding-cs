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
    public class ItemCreatingServiceTests
    {
        private IDateTimeService _dateTimeService;
        private IGuidService _guidService;
        private IItemRepository _itemRepository;
        private IItemCreatingService _itemCreatingService;

        [SetUp]
        public void SetUp()
        {
            _dateTimeService = Substitute.For<IDateTimeService>();
            _dateTimeService.CurrentDateTime.Returns(new DateTime(2018, 9, 27));

            _guidService = Substitute.For<IGuidService>();
            _guidService.GenerateGuid.Returns(Guid.Parse("F7148339-E162-4657-B886-C29BF6A2D312"));

            _itemRepository = Substitute.For<IItemRepository>();

            _itemCreatingService = new ItemCreatingService(_itemRepository, _dateTimeService, _guidService);
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

            var createdItem = await _itemCreatingService.CreateAsync(item);

            Assert.That(createdItem, Is.EqualTo(expectedItem).UsingItemComparer());
        }
    }
}