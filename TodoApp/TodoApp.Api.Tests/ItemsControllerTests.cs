using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Api.Controllers;
using TodoApp.Api.Tests.Utils;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;

namespace TodoApp.Api.Tests
{
    [TestFixture]
    public class ItemsControllerTests
    {
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

        [Test]
        public async Task GetAllItemsAsync_ReturnsAllItems()
        {
            var itemRepository = Substitute.For<IItemRepository>();
            itemRepository.GetAllAsync().Returns(Items);

            var itemsController = CreateController(itemRepository);

            var message = await ExecuteAsyncAction(() => itemsController.GetAllItemsAsync());
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
            var id = Items[0].Id.ToString();
            var expected = Items[0];
            var itemRepository = Substitute.For<IItemRepository>();
            itemRepository.GetByIdAsync(id).Returns(expected);

            var itemsController = CreateController(itemRepository);

            var message = await ExecuteAsyncAction(() => itemsController.GetItemByIdAsync(id));
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
            var changedItem = Items[2];

            var itemRepository = Substitute.For<IItemRepository>();
            itemRepository.UpdateAsync(changedItem).Returns(Task.CompletedTask);

            var itemsController = CreateController(itemRepository);

            var message = await ExecuteAsyncAction(() => itemsController.PutItemAsync(changedItem.Id.ToString(), changedItem));

            Assert.AreEqual(HttpStatusCode.NoContent, message.StatusCode);
        }

        [Test]
        public async Task DeleteItemAsync_ReturnsOkWithDeletedItem()
        {
            var id = Items[3].Id.ToString();
            var expected = Items[3];

            var itemRepository = Substitute.For<IItemRepository>();
            itemRepository.GetByIdAsync(id).Returns(expected);
            itemRepository.DeleteAsync(expected).Returns(Task.CompletedTask);

            var itemsController = CreateController(itemRepository);

            var message = await ExecuteAsyncAction(() => itemsController.DeleteItemAsync(id));
            message.TryGetContentValue<Item>(out var item);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(item, Is.EqualTo(Items[3]).UsingItemComparer());
            });
        }

        [Test]
        public async Task PostItemAsync_ReturnsCreatedWithLinkToItem()
        {
            var newItem = Items[1];

            var itemRepository = Substitute.For<IItemRepository>();
            itemRepository.CreateAsync(newItem).Returns(Task.CompletedTask);

            var itemsController = CreateController(itemRepository); 
            itemsController.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/test")
            };

            itemsController.Configuration.Routes.MapHttpRoute(
                name: "PostNewItem",
                routeTemplate: "api/{id}/test",
                defaults: new { id = RouteParameter.Optional }
            );

            var headerLocation = "http://localhost/api/" + newItem.Id + "/test";
            var message = await ExecuteAsyncAction(() => itemsController.PostItemAsync(newItem));
            message.TryGetContentValue<Item>(out var item);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(message.Headers.Location.AbsoluteUri, Is.EqualTo(headerLocation));
                Assert.That(item, Is.EqualTo(Items[1]).UsingItemComparer());
            });
        }

        private static async Task<HttpResponseMessage> ExecuteAsyncAction(Func<Task<IHttpActionResult>> httpAction)
        {
            IHttpActionResult response = await httpAction();
            return await response.ExecuteAsync(CancellationToken.None);
        }

        private static ItemsController CreateController(IItemRepository repository)
            => new ItemsController(repository)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
    }
}
