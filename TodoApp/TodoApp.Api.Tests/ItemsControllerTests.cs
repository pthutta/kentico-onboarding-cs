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

            Assert.AreEqual(expected, response);
        }

        [Test]
        public async Task GetItemByIdAsync_ReturnsOkWithRequiredItem()
        {
            const string id = "1";
            var expected = ItemsController.Items.FirstOrDefault(it => it.Id == id);

            var message = await ExecuteAsyncAction(async () => await _itemsController.GetItemByIdAsync(id));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, message.StatusCode);
                Assert.IsNotNull(message.Content);
                Assert.IsTrue(message.TryGetContentValue<Item>(out var item));
                Assert.AreEqual(expected, item);
            });
        }

        [Test]
        public async Task PutItemAsync_ReturnsNoContent()
        {
            const string id = "1";

            var changedItem = new Item
            {
                Id = id,
                Text = "Write tests"
            };

            var message = await ExecuteAsyncAction(async () => await _itemsController.PutItemAsync(id, changedItem));
            var item = ItemsController.Items.FirstOrDefault(it => it.Id == id);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.NoContent, message.StatusCode);
                Assert.AreEqual(changedItem, item);
            });
        }

        [Test]
        public async Task DeleteItemAsync_ReturnsOkWithDeletedItem()
        {
            const string id = "3";
            var expected = ItemsController.Items.FirstOrDefault(it => it.Id == id);

            var message = await ExecuteAsyncAction(async () => await _itemsController.DeleteItemAsync(id));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, message.StatusCode);
                Assert.IsNotNull(message.Content);
                Assert.IsTrue(message.TryGetContentValue<Item>(out var item));
                Assert.AreEqual(expected, item);
                Assert.That(ItemsController.Items, Has.No.Member(item));
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

            var newItem = new Item
            {
                Id = "4",
                Text = "Write tests"
            };

            var message = await ExecuteAsyncAction(async () => await _itemsController.PostItemAsync(newItem));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.Created, message.StatusCode);
                Assert.AreEqual("http://localhost/api/test/4", message.Headers.Location.AbsoluteUri);
            });
        }

        private static async Task<HttpResponseMessage> ExecuteAsyncAction(Func<Task<IHttpActionResult>> httpAction)
        {
            IHttpActionResult response = await httpAction();
            return await response.ExecuteAsync(CancellationToken.None);
        }
    }
}
