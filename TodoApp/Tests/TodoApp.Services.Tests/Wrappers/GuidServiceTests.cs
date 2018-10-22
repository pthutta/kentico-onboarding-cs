using NUnit.Framework;
using TodoApp.Contracts.Wrappers;
using TodoApp.Services.Wrappers;

namespace TodoApp.Services.Tests.Wrappers
{
    [TestFixture]
    public class GuidServiceTests
    {
        private IGuidWrapper _guidWrapper;

        [SetUp]
        public void SetUp()
        {
            _guidWrapper = new GuidWrapper();
        }

        [Test]
        public void GenerateGuid_ReturnsDifferentIds()
        {
            var id1 = _guidWrapper.GenerateGuid();
            var id2 = _guidWrapper.GenerateGuid();
            var id3 = _guidWrapper.GenerateGuid();

            Assert.Multiple(() =>
            {
                Assert.That(id1, Is.Not.EqualTo(id2));
                Assert.That(id1, Is.Not.EqualTo(id3));
                Assert.That(id2, Is.Not.EqualTo(id3));
            });
        }
    }
}
