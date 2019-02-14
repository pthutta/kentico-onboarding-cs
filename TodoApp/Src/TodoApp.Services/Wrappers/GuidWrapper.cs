using System;
using TodoApp.Contracts.Wrappers;

namespace TodoApp.Services.Wrappers
{
    internal class GuidWrapper : IGuidWrapper
    {
        public Guid GenerateGuid() => Guid.NewGuid();
    }
}
