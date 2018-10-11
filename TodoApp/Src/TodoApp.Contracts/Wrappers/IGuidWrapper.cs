using System;

namespace TodoApp.Contracts.Wrappers
{
    public interface IGuidWrapper
    {
        Guid GenerateGuid { get; }
    }
}
