using System;
using TodoApp.Contracts.Services;

namespace TodoApp.Services.Utils
{
    internal class GuidService : IGuidService
    {
        public Guid GenerateGuid => Guid.NewGuid();
    }
}
