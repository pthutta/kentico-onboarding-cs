using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.Web.Http.Routing;
using NUnit.Framework;
using TodoApp.Api.Controllers;
using TodoApp.Api.Models;
using TodoApp.Api.Tests.Utils;

namespace TodoApp.Api.Tests
{
    [TestFixture]
    public class ItemsControllerTests
    {
        private ItemsController _itemsController;

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
            var expected = ItemsController.Items;

            var response = await _itemsController.GetAllItemsAsync();

            Assert.That(response, Is.EquivalentTo(expected));
        }

        [Test]
        public async Task GetItemByIdAsync_ReturnsOkWithRequiredItem()
        {
            const string id = "1";
            var expected = ItemsController.Items[0];

            var message = await ExecuteAsyncAction(async () => await _itemsController.GetItemByIdAsync(id));

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(message.Content, Is.Not.Null);
                Assert.That(message.TryGetContentValue<Item>(out var item), Is.Not.Null);
                Assert.That(item.IsEqual(expected), Is.True);
            });
        }

        [Test]
        public async Task PutItemAsync_ReturnsNoContent()
        {
            const string id = "1";
            var expected = ItemsController.Items[2];

            var message = await ExecuteAsyncAction(async () => await _itemsController.PutItemAsync(id, expected));

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(message.Content, Is.Not.Null);
                Assert.That(message.TryGetContentValue<Item>(out var item), Is.Not.Null);
                Assert.That(item.IsEqual(expected), Is.True);
            });
        }

        [Test]
        public async Task DeleteItemAsync_ReturnsOkWithDeletedItem()
        {
            const string id = "3";
            var expected = ItemsController.Items[3];

            var message = await ExecuteAsyncAction(async () => await _itemsController.DeleteItemAsync(id));

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(message.Content, Is.Not.Null);
                Assert.That(message.TryGetContentValue<Item>(out var item), Is.Not.Null);
                Assert.That(item.IsEqual(expected), Is.True);
            });
        }

        [Test]
        public async Task PostItemAsync_ReturnsCreatedWithLinkToItem()
        {
            _itemsController.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/v1/items")
            };
            _itemsController.Configuration.Routes.MapHttpRoute(
                name: "PostNewItem",
                routeTemplate: "api/test/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var newItem = ItemsController.Items[1];

            var message = await ExecuteAsyncAction(async () => await _itemsController.PostItemAsync(newItem));

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(message.Headers.Location.AbsoluteUri, Is.EqualTo("http://localhost/api/test/2"));
                Assert.That(message.Content, Is.Not.Null);
                Assert.That(message.TryGetContentValue<Item>(out var item), Is.Not.Null);
                Assert.That(item.IsEqual(newItem), Is.True);
            });
        }

        private static async Task<HttpResponseMessage> ExecuteAsyncAction(Func<Task<IHttpActionResult>> httpAction)
        {
            IHttpActionResult response = await httpAction();
            return await response.ExecuteAsync(CancellationToken.None);
        }
    }
}
