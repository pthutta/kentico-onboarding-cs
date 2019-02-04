using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Api.Tests.Extensions;
using TodoApp.Api.Tests.Wrappers;
using TodoApp.Contracts.Models;
using TodoApp.TestContracts.Extensions;
using TodoApp.TestContracts.Factories;

namespace TodoApp.Api.Tests.Controllers
{
    [TestFixture]
    public class ItemsControllerGetTests : ItemsControllerTestsBase
    {
        [Test]
        public async Task GetAllItemsAsync_ReturnsAllItems()
        {
            var expectedItems = new []
            {
                ItemFactory.CreateItem("46A4D418-931F-45EC-8C2F-06236772B245", "Learn react"),
                ItemFactory.CreateItem("0EBF758B-FC9F-4A42-8A6E-3DD7209E0E46", "Learn redux"),
                ItemFactory.CreateItem("EA531B21-FC10-4BEB-B6BE-A9635E610213", "Write Web API"),
                ItemFactory.CreateItem("5694207A-697C-449B-BA49-05AC6683C5E5", "Write dummier controller")
            };
            ItemRepository.GetAllAsync().Returns(expectedItems);

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.GetAllItemsAsync());
            message.TryGetContentValue(out IEnumerable<Item> actualItems);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(actualItems, Is.EquivalentTo(expectedItems));
            });
        }

        [Test]
        public async Task GetItemByIdAsync_ExistingItem_ReturnsOkWithRequiredItem()
        {
            var expectedItem = ItemFactory.CreateItem("46A4D418-931F-45EC-8C2F-06236772B245", "Learn react");
            var id = expectedItem.Id;
            ItemObtainingService.ExistsAsync(id).Returns(true);
            ItemObtainingService.GetById(id).Returns(expectedItem);

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.GetItemByIdAsync(id));
            message.TryGetContentValue(out Item actualItem);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(actualItem, Is.EqualTo(expectedItem));
            });
        }

        [Test]
        public async Task GetItemByIdAsync_NonexistentItem_ReturnsNotFound()
        {
            var id = Guid.Parse("46A4D418-931F-45EC-8C2F-06236772B245");
            ItemObtainingService.ExistsAsync(id).Returns(false);

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.GetItemByIdAsync(id));

            Assert.Multiple(() =>
            {
                ItemObtainingService.DidNotReceive().GetById(Arg.Any<Guid>());
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public async Task GetItemByIdAsync_EmptyGuid_ReturnsBadRequest()
        {
            var id = Guid.Empty;
            
            var message = await ItemsController.ExecuteAsyncAction(controller => controller.GetItemByIdAsync(id));

            AssertExtended.IsBadResponseMessage(message, string.Empty);
        }
    }
}
