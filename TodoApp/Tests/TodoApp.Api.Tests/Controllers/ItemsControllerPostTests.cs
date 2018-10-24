using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Api.Tests.Extensions;
using TodoApp.Api.Tests.TestData;
using TodoApp.Api.Tests.Wrappers;
using TodoApp.Contracts.Models;
using TodoApp.TestContracts.Extensions;
using TodoApp.TestContracts.Factories;

namespace TodoApp.Api.Tests.Controllers
{
    [TestFixture]
    public class ItemsControllerPostTests : ItemsControllerTestsBase
    {
        [SetUp]
        public void SetUp()
            => Init();

        [Test]
        public async Task PostItemAsync_ValidItem_ReturnsCreatedItem()
        {
            var creationTime = new DateTime(2018, 9, 27);
            var expectedItem = ItemFactory.CreateItem("5694207A-697C-449B-BA49-05AC6683C5E5", "Write dummier controller", creationTime, creationTime);
            var newItem = ItemFactory.CreateItem(Guid.Empty, expectedItem.Text);
            var headerLocation = new Uri("http://localhost/");

            ItemCreatingService.CreateAsync(newItem).Returns(expectedItem);
            UrlService.GetItemUrl(expectedItem.Id).Returns(headerLocation);

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PostItemAsync(newItem));
            message.TryGetContentValue(out Item actualItem);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(message.Headers.Location, Is.EqualTo(headerLocation));
                Assert.That(actualItem, Is.EqualTo(expectedItem).UsingItemComparer());
            });
        }

        [Test]
        public async Task PostItemAsync_NullItem_ReturnsBadRequest()
        {
            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PostItemAsync(null));

            AssertExtended.IsBadResponseMessage(message, string.Empty);
        }

        [Test, TestCaseSource(typeof(ItemsControllerTestData), nameof(ItemsControllerTestData.IncorrectTextTestCases))]
        public async Task PostItemAsync_ItemWithIncorrectText_ReturnsBadRequest(string text)
        {
            var newItem = ItemFactory.CreateItem(Guid.Empty, text);

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PostItemAsync(newItem));

            AssertExtended.IsBadResponseMessage(message, nameof(Item.Text));
        }

        [Test]
        public async Task PostItemAsync_DefaultItem_ReturnsBadRequest()
        {
            var newItem = new Item();

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PostItemAsync(newItem));

            AssertExtended.IsBadResponseMessage(message, nameof(Item.Text));
        }

        [Test]
        public async Task PostItemAsync_ItemWithSetId_ReturnsBadRequest()
        {
            var newItem = ItemFactory.CreateItem("5694207A-697C-449B-BA49-05AC6683C5E5", "Write dummier controller");

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PostItemAsync(newItem));

            AssertExtended.IsBadResponseMessage(message, nameof(Item.Id));
        }

        [Test]
        public async Task PostItemAsync_ItemWithSetCreationTime_ReturnsBadRequest()
        {
            var newItem = ItemFactory.CreateItem(Guid.Empty, "Write dummier controller", new DateTime(2018, 10, 23));

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PostItemAsync(newItem));

            AssertExtended.IsBadResponseMessage(message, nameof(Item.CreationTime));
        }

        [Test]
        public async Task PostItemAsync_ItemWithSetLastUpdateTime_ReturnsBadRequest()
        {
            var newItem = ItemFactory.CreateItem(Guid.Empty, "Write dummier controller", DateTime.MinValue, new DateTime(2018, 10, 23));

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PostItemAsync(newItem));

            AssertExtended.IsBadResponseMessage(message, nameof(Item.LastUpdateTime));
        }

        [Test]
        public async Task PostItemAsync_BadItemModelState_ReturnsAllErrors()
        {
            var newItem = ItemFactory.CreateItem("5694207A-697C-449B-BA49-05AC6683C5E5", string.Empty, new DateTime(2018, 10, 23), new DateTime(2018, 10, 23));
            
            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PostItemAsync(newItem));

            AssertExtended.IsBadResponseMessage(message,
                nameof(Item.Id),
                nameof(Item.Text),
                nameof(Item.CreationTime),
                nameof(Item.LastUpdateTime)
            );
        }
    }
}
