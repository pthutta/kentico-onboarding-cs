using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Api.Tests.Extensions;
using TodoApp.Api.Tests.Wrappers;
using TodoApp.Contracts.Models;
using TodoApp.TestContracts.Factories;

namespace TodoApp.Api.Tests.Controllers
{
    [TestFixture]
    public class ItemsControllerDeleteTests : ItemsControllerTestsBase
    {
        [SetUp]
        public void SetUp()
            => Init();

        [Test]
        public async Task DeleteItemAsync_ExistingItem_ReturnsOkWithDeletedItem()
        {
            var expectedDeletedItem = ItemFactory.CreateItem("EA531B21-FC10-4BEB-B6BE-A9635E610213", "Write Web API");
            var id = expectedDeletedItem.Id;
            ItemObtainingService.ExistsAsync(id).Returns(true);
            ItemRepository.DeleteAsync(id).Returns(expectedDeletedItem);

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.DeleteItemAsync(id));
            message.TryGetContentValue(out Item actualDeletedItem);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(actualDeletedItem, Is.EqualTo(expectedDeletedItem));
            });
        }

        [Test]
        public async Task DeleteItemAsync_NonexistentItem_ReturnsNotFound()
        {
            var id = Guid.Parse("EA531B21-FC10-4BEB-B6BE-A9635E610213");
            ItemObtainingService.ExistsAsync(id).Returns(false);

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.DeleteItemAsync(id));

            Assert.Multiple(() =>
            {
                ItemRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>());
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public async Task DeleteItemAsync_EmptyGuid_ReturnsBadRequest()
        {
            var id = Guid.Empty;

            var message = await ItemsController.ExecuteAsyncAction(controller => controller.DeleteItemAsync(id));

            Assert.Multiple(() =>
            {
                ItemObtainingService.DidNotReceive().ExistsAsync(Arg.Any<Guid>());
                ItemRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>());
                AssertExtended.IsBadResponseMessage(message, string.Empty);
            });
        }
    }
}
