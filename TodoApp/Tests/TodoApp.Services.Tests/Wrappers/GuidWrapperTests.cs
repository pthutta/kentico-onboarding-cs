using System;
using System.Linq;
using NUnit.Framework;
using TodoApp.Contracts.Wrappers;
using TodoApp.Services.Wrappers;

namespace TodoApp.Services.Tests.Wrappers
{
    [TestFixture]
    public class GuidWrapperTests
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
            const int numberOfIds = 10;

            var multipleIds = Enumerable
                .Repeat<Func<Guid>>(_guidWrapper.GenerateGuid, numberOfIds)
                .Select(generateGuid => generateGuid())
                .ToArray();

            Assert.Multiple(() =>
            {
                Assert.That(multipleIds, Is.Unique);
                Assert.That(multipleIds, Does.Not.Contain(Guid.Empty));
            });
        }
    }
}
