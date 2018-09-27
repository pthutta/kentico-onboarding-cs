using System;

namespace TodoApp.Contracts.Services
{
    public interface IDateTimeService
    {
        DateTime CurrentDateTime { get; }
    }
}
