using System;
using System.Web.Http.Routing;
using NSubstitute;
using NUnit.Framework;
using TodoApp.ApiServices.Services;
using TodoApp.Contracts.Routes;

namespace TodoApp.ApiServices.Tests.Services
{
    [TestFixture]
    public class UrlServiceTests
    {
        private IUrlService _urlService;
        private IRouteNames _routeNames;
        private UrlHelper _urlHelper;

        [SetUp]
        public void SetUp()
        {
            _urlHelper = Substitute.For<UrlHelper>();
            _routeNames = Substitute.For<IRouteNames>();
            _routeNames.ItemRouteName.Returns("NewItemRoute");

            _urlService = new UrlService(_urlHelper, _routeNames);
        }

        [Test]
        public void GetItemUrl_ReturnsCreatedUrl()
        {
            var routeName = _routeNames.ItemRouteName;
            var id = Guid.Parse("EAB88043-345B-44F9-9839-296579D8AC62");
            var expectedUrl = new Uri($"http://localhost/{id}/tests");

            _urlHelper
                .Link(routeName, Arg.Is<object>(value => CompareIds(value, id)))
                .Returns(expectedUrl.ToString());

            var actualUrl = _urlService.GetItemUrl(id);

            Assert.That(actualUrl, Is.EqualTo(expectedUrl));
        }

        private static bool CompareIds(object value, Guid id)
            => (Guid) new HttpRouteValueDictionary(value)["id"] == id;
    }
}
