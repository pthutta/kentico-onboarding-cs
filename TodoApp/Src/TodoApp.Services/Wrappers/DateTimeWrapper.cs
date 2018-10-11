using System;
using TodoApp.Contracts.Wrappers;

namespace TodoApp.Services.Wrappers
{
    internal class DateTimeWrapper : IDateTimeWrapper
    {
        public DateTime CurrentDateTime => DateTime.Now;
    }
}
