using System.Threading;
using NUnit.Framework;
using TodoApp.Contracts.Wrappers;
using TodoApp.Services.Wrappers;

namespace TodoApp.Services.Tests.Wrappers
{
    [TestFixture]
    public class DateTimeServiceTests
    {
        private IDateTimeWrapper _dateTimeWrapper;

        [SetUp]
        public void SetUp()
        {
            _dateTimeWrapper = new DateTimeWrapper();
        }

        [Test]
        public void CurrentDateTime_ReturnsNotTooDistantTimes()
        {
            const int milliseconds = 1000;
            var time1 = _dateTimeWrapper.CurrentDateTime();
            Thread.Sleep(milliseconds);
            var time2 = _dateTimeWrapper.CurrentDateTime();

            var difference = time1.Subtract(time2);

            Assert.That(difference.Milliseconds, Is.EqualTo(milliseconds).Within(milliseconds * 2));
        }
    }
}
