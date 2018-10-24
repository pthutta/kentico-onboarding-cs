using System;
using System.Threading;
using NUnit.Framework;
using TodoApp.Contracts.Wrappers;
using TodoApp.Services.Wrappers;

namespace TodoApp.Services.Tests.Wrappers
{
    [TestFixture]
    public class DateTimeWrapperTests
    {
        private IDateTimeWrapper _dateTimeWrapper;

        [SetUp]
        public void SetUp()
        {
            _dateTimeWrapper = new DateTimeWrapper();
        }

        [Test]
        public void CurrentDateTime_DoesNotReturnDefaultValue()
        {
            var time = _dateTimeWrapper.CurrentDateTime();

            Assert.That(time, Is.Not.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void CurrentDateTime_ReturnsNotTooDistantConsecutiveTimes()
        {
            const int milliseconds = 100;

            var time1 = _dateTimeWrapper.CurrentDateTime();
            Thread.Sleep(milliseconds);
            var time2 = _dateTimeWrapper.CurrentDateTime();
            var difference = time2 - time1;

            Assert.Multiple(() =>
            {
                Assert.That(difference.Milliseconds, Is.GreaterThan(0));
                Assert.That(difference.Milliseconds, Is.EqualTo(milliseconds).Within(milliseconds * 0.5));
            });
        }
    }
}
