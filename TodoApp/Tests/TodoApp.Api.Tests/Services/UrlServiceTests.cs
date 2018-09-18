using System;
using System.Web.Http.Routing;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Api.Services;
using TodoApp.Contracts.Services;

namespace TodoApp.Api.Tests.Services
{
    [TestFixture]
    public class UrlServiceTests
    {
        private IUrlService _urlService;
        private UrlHelper _urlHelper;

        [SetUp]
        public void SetUp()
        {
            _urlHelper = Substitute.For<UrlHelper>();
            _urlService = new UrlService(_urlHelper);
        }

        [Test]
        public void GetItemUrl_ReturnsCreatedUrl()
        {
            const string routeName = UrlService.NewItemRouteName;
            var id = Guid.Parse("EAB88043-345B-44F9-9839-296579D8AC62");
            var expectedUrl = $"http://localhost/{id}/tests";

            _urlHelper
                .Link(routeName, Arg.Is<object>(value => CompareIds(value, id)))
                .Returns(expectedUrl);

            var resultUrl = _urlService.GetItemUrl(id);

            Assert.That(resultUrl, Is.EqualTo(expectedUrl));
        }

        private static bool CompareIds(object value, Guid id)
            => (Guid) new HttpRouteValueDictionary(value)["id"] == id;
    }
}
