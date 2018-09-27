using System.Threading;
using NUnit.Framework;
using TodoApp.Contracts.Services;
using TodoApp.Services.Utils;

namespace TodoApp.Services.Tests.Utils
{
    [TestFixture]
    public class DateTimeServiceTests
    {
        private IDateTimeService _dateTimeService;

        [SetUp]
        public void SetUp()
        {
            _dateTimeService = new DateTimeService();
        }

        [Test]
        public void CurrentDateTime_ReturnsNotTooDistantTimes()
        {
            const int milliseconds = 1000;
            var time1 = _dateTimeService.CurrentDateTime;
            Thread.Sleep(milliseconds);
            var time2 = _dateTimeService.CurrentDateTime;

            var difference = time1.Subtract(time2);

            Assert.That(difference.Milliseconds, Is.EqualTo(milliseconds).Within(milliseconds * 2));
        }
    }
}
