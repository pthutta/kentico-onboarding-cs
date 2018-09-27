using System;

namespace TodoApp.Contracts.Services
{
    public interface IGuidService
    {
        Guid GenerateGuid { get; }
    }
}
