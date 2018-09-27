using System;
using TodoApp.Contracts.Services;

namespace TodoApp.Services.Utils
{
    internal class DateTimeService : IDateTimeService
    {
        public DateTime CurrentDateTime => DateTime.Now;
    }
}
