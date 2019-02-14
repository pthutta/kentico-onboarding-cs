using System.Net.Http;
using System.Web.Http;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Api.Controllers;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Routes;
using TodoApp.Contracts.Services;

namespace TodoApp.Api.Tests.Controllers
{
    public class ItemsControllerTestsBase
    {
        protected ItemsController ItemsController;
        protected IItemObtainingService ItemObtainingService;
        protected IItemCreatingService ItemCreatingService;
        protected IItemUpdatingService ItemUpdatingService;
        protected IItemRepository ItemRepository;
        protected IUrlService UrlService;

        [SetUp]
        protected void SetUpDependencies()
        {
            ItemCreatingService = Substitute.For<IItemCreatingService>();
            ItemUpdatingService = Substitute.For<IItemUpdatingService>();
            UrlService = Substitute.For<IUrlService>();
            ItemObtainingService = Substitute.For<IItemObtainingService>();
            ItemRepository = Substitute.For<IItemRepository>();

            ItemsController = new ItemsController(ItemObtainingService, ItemCreatingService, ItemUpdatingService, ItemRepository, UrlService)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }
    }
}