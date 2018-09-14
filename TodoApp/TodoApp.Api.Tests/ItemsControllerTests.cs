using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Api.Controllers;
using TodoApp.Api.Tests.Extensions;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;

namespace TodoApp.Api.Tests
{
    [TestFixture]
    public class ItemsControllerTests
    {
        private ItemsController _itemsController;
        private IItemRepository _itemRepository;
        private IUrlService _urlService;

        private static readonly Item[] Items =
        {
            new Item
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Text = "Learn react"
            },
            new Item
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Text = "Learn redux"
            },
            new Item
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Text = "Write Web API"
            },
            new Item
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                Text = "Write dummier controller"
            }
        };

        [SetUp]
        public void SetUp()
        {
            _itemRepository = Substitute.For<IItemRepository>();
            _urlService = Substitute.For<IUrlService>();
            _itemsController = new ItemsController(_itemRepository, _urlService)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [Test]
        public async Task GetAllItemsAsync_ReturnsAllItems()
        {
            _itemRepository.GetAllAsync().Returns(Items);

            var message = await ExecuteAsyncAction(() => _itemsController.GetAllItemsAsync());
            message.TryGetContentValue<Item[]>(out var items);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(items, Is.EquivalentTo(Items).UsingItemComparer());
            });
        }

        [Test]
        public async Task GetItemByIdAsync_ReturnsOkWithRequiredItem()
        {
            var expected = Items[0];
            var id = expected.Id;
            _itemRepository.GetByIdAsync(id).Returns(expected);

            var message = await ExecuteAsyncAction(() => _itemsController.GetItemByIdAsync(id));
            message.TryGetContentValue<Item>(out var item);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(item, Is.EqualTo(expected).UsingItemComparer());
            });
        }

        [Test]
        public async Task PutItemAsync_ReturnsOkWithUpdatedItem()
        {
            var changedItem = Items[3];
            // _itemRepository.UpdateAsync(changedItem).Returns(Task.CompletedTask);

            var message = await ExecuteAsyncAction(() => _itemsController.PutItemAsync(changedItem.Id, changedItem));

            Assert.AreEqual(HttpStatusCode.NoContent, message.StatusCode);
        }

        [Test]
        public async Task DeleteItemAsync_ReturnsOkWithDeletedItem()
        {
            var deleted = Items[2];
            var id = deleted.Id;
            _itemRepository.DeleteAsync(id).Returns(deleted);

            var message = await ExecuteAsyncAction(() => _itemsController.DeleteItemAsync(id));
            message.TryGetContentValue<Item>(out var item);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(item, Is.EqualTo(deleted).UsingItemComparer());
            });
        }

        [Test]
        public async Task PostItemAsync_ReturnsCreatedItem()
        {
            var expectedItem = Items[1];
            var newItem = new Item { Text = expectedItem.Text };
            const string headerLocation = "http://localhost/";

            _itemRepository.CreateAsync(newItem).Returns(expectedItem);
            _urlService.GetItemUrl(expectedItem.Id).Returns(headerLocation);

            var message = await ExecuteAsyncAction(() => _itemsController.PostItemAsync(newItem));
            message.TryGetContentValue<Item>(out var item);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(message.Headers.Location.ToString(), Is.EqualTo(headerLocation));
                Assert.That(item, Is.EqualTo(expectedItem).UsingItemComparer());
            });
        }

        private static async Task<HttpResponseMessage> ExecuteAsyncAction(Func<Task<IHttpActionResult>> httpAction)
        {
            IHttpActionResult response = await httpAction();
            return await response.ExecuteAsync(CancellationToken.None);
        }
    }
}
