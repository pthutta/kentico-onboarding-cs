using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using NUnit.Framework;
using TodoApp.Api.Controllers;

namespace TodoApp.Api.Tests
{
    [TestFixture]
    public class ItemsControllerTests
    {
        private ItemsController _itemsController;

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
            _itemsController = new ItemsController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [Test]
        public async Task GetAllItemsAsync_ReturnsAllItems()
        {
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
            var id = Items[0].Id.ToString();

            var message = await ExecuteAsyncAction(() => _itemsController.GetItemByIdAsync(id));
            message.TryGetContentValue<Item>(out var item);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(item, Is.EqualTo(Items[0]).UsingItemComparer());
            });
        }

        [Test]
        public async Task PutItemAsync_ReturnsOkWithUpdatedItem()
        {
            var updatedItem = Items[2];

            var message = await ExecuteAsyncAction(() => _itemsController.PutItemAsync(updatedItem.Id.ToString(), updatedItem));
            message.TryGetContentValue<Item>(out var item);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(item, Is.EqualTo(Items[2]).UsingItemComparer());
            });
        }

        [Test]
        public async Task DeleteItemAsync_ReturnsOkWithDeletedItem()
        {
            var id = Items[3].Id.ToString();

            var message = await ExecuteAsyncAction(() => _itemsController.DeleteItemAsync(id));
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
            _itemsController.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/test")
            };
            _itemsController.Configuration.Routes.MapHttpRoute(
                name: "PostNewItem",
                routeTemplate: "api/{id}/test",
                defaults: new { id = RouteParameter.Optional }
            );

            var newItem = Items[1];
            var headerLocation = "http://localhost/api/" + newItem.Id + "/test";

            var message = await ExecuteAsyncAction(() => _itemsController.PostItemAsync(newItem));
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
    }
}
