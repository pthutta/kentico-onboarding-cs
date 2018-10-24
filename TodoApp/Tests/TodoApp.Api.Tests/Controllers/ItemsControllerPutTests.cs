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
    public class ItemsControllerPutTests : ItemsControllerTestsBase
    {
        [SetUp]
        public void SetUp()
            => Init();

        [Test]
        public async Task PutItemAsync_ExistingItem_ReturnsOkWithUpdatedItem()
        {
            var changedItem = ItemFactory.CreateItem("0EBF758B-FC9F-4A42-8A6E-3DD7209E0E46", "Learn redux");
            ItemObtainingService.ExistsAsync(changedItem.Id).Returns(true);

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PutItemAsync(changedItem.Id, changedItem));

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
                ItemUpdatingService.Received(1).UpdateAsync(changedItem);
            });
        }

        [Test]
        public async Task PutItemAsync_NullItem_ReturnsBadRequest()
        {
            var id = Guid.Parse("0EBF758B-FC9F-4A42-8A6E-3DD7209E0E46");

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PutItemAsync(id, null));

            AssertExtended.IsBadResponseMessage(message, string.Empty);
        }

        [Test]
        public async Task PutItemAsync_EmptyGuid_ReturnsBadRequest()
        {
            var updatedItem = ItemFactory.CreateItem("0EBF758B-FC9F-4A42-8A6E-3DD7209E0E46", "Learn redux");

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PutItemAsync(Guid.Empty, updatedItem));

            AssertExtended.IsBadResponseMessage(message, string.Empty);
        }

        [Test, TestCaseSource(typeof(ItemsControllerTestData), nameof(ItemsControllerTestData.IncorrectTextTestCases))]
        public async Task PutItemAsync_ItemWithIncorrectText_ReturnsBadRequest(string text)
        {
            var changedItem = ItemFactory.CreateItem("0EBF758B-FC9F-4A42-8A6E-3DD7209E0E46", text);

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PutItemAsync(changedItem.Id, changedItem));

            AssertExtended.IsBadResponseMessage(message, nameof(Item.Text));
        }

        [Test]
        public async Task PutItemAsync_NotMatchingIds_ReturnsBadRequest()
        {
            var id = Guid.Parse("EA531B21-FC10-4BEB-B6BE-A9635E610213");
            var changedItem = ItemFactory.CreateItem("0EBF758B-FC9F-4A42-8A6E-3DD7209E0E46", "Learn redux");

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PutItemAsync(id, changedItem));

            AssertExtended.IsBadResponseMessage(message, nameof(Item.Id));
        }

        [Test]
        public async Task PutItemAsync_ItemWithSetCreationTime_ReturnsBadRequest()
        {
            var changedItem = ItemFactory.CreateItem("0EBF758B-FC9F-4A42-8A6E-3DD7209E0E46", "Learn redux", new DateTime(2018, 10, 23));

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PutItemAsync(changedItem.Id, changedItem));

            AssertExtended.IsBadResponseMessage(message, nameof(Item.CreationTime));
        }

        [Test]
        public async Task PutItemAsync_ItemWithSetLastUpdateTime_ReturnsBadRequest()
        {
            var changedItem = ItemFactory.CreateItem("0EBF758B-FC9F-4A42-8A6E-3DD7209E0E46", "Learn redux", null, new DateTime(2018, 10, 23));

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PutItemAsync(changedItem.Id, changedItem));

            AssertExtended.IsBadResponseMessage(message, nameof(Item.LastUpdateTime));
        }

        [Test]
        public async Task PutItemAsync_BadItemModelState_ReturnsAllErrors()
        {
            var id = Guid.Parse("0EBF758B-FC9F-4A42-8A6E-3DD7209E0E46");
            var changedItem = ItemFactory.CreateItem(Guid.Empty, null, new DateTime(2018, 10, 23), new DateTime(2018, 10, 23));

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PutItemAsync(id, changedItem));

            AssertExtended.IsBadResponseMessage(message,
                nameof(changedItem.Id),
                nameof(changedItem.Text),
                nameof(changedItem.CreationTime),
                nameof(changedItem.LastUpdateTime)
            );
        }

        [Test]
        public async Task PutItemAsync_NonexistentItem_ReturnsCreated()
        {
            var creationTime = new DateTime(2018, 9, 27);
            var changedItem = ItemFactory.CreateItem("0EBF758B-FC9F-4A42-8A6E-3DD7209E0E46", "Learn redux");
            var expectedItem = ItemFactory.CreateItem(changedItem.Id, changedItem.Text, creationTime, creationTime);
            var headerLocation = new Uri("http://localhost/");

            ItemObtainingService.ExistsAsync(changedItem.Id).Returns(false);
            ItemCreatingService.CreateAsync(Arg.Is<Item>(newItem => newItem.Text == changedItem.Text)).Returns(expectedItem);
            UrlService.GetItemUrl(expectedItem.Id).Returns(headerLocation);

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PutItemAsync(changedItem.Id, changedItem));
            message.TryGetContentValue(out Item actualItem);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(message.Headers.Location, Is.EqualTo(headerLocation));
                Assert.That(actualItem, Is.EqualTo(expectedItem).UsingItemComparer());
            });
        }

        [Test]
        public async Task PutItemAsync_DefaultItem_ReturnsBadRequest()
        {
            var id = Guid.Parse("0EBF758B-FC9F-4A42-8A6E-3DD7209E0E46");
            var changedItem = new Item();
            
            var message = await ItemsController.ExecuteAsyncAction(controller => controller.PutItemAsync(id, changedItem));

            AssertExtended.IsBadResponseMessage(message, nameof(Item.Id), nameof(Item.Text));
        }
    }
}
