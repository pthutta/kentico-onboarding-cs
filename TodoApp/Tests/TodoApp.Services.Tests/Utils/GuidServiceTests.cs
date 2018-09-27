using NUnit.Framework;
using TodoApp.Contracts.Services;
using TodoApp.Services.Utils;

namespace TodoApp.Services.Tests.Utils
{
    [TestFixture]
    public class GuidServiceTests
    {
        private IGuidService _guidService;

        [SetUp]
        public void SetUp()
        {
            _guidService = new GuidService();
        }

        [Test]
        public void GenerateGuid_ReturnsDifferentIds()
        {
            var id1 = _guidService.GenerateGuid;
            var id2 = _guidService.GenerateGuid;
            var id3 = _guidService.GenerateGuid;

            Assert.Multiple(() =>
            {
                Assert.That(id1, Is.Not.EqualTo(id2));
                Assert.That(id1, Is.Not.EqualTo(id3));
                Assert.That(id2, Is.Not.EqualTo(id3));
            });
        }
    }
}
