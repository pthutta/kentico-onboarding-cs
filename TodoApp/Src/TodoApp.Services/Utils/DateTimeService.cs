using System;
using TodoApp.Contracts.Services;

namespace TodoApp.Services.Utils
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime CurrentDateTime => DateTime.Now;
    }
}
