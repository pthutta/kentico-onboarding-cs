using System;
using TodoApp.Contracts.Services;

namespace TodoApp.Services.Utils
{
    public class GuidService : IGuidService
    {
        public Guid GenerateGuid => Guid.NewGuid();
    }
}
