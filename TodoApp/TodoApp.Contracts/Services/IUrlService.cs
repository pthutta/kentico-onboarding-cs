using System;

namespace TodoApp.Contracts.Services
{
    public interface IUrlService
    {
        string GetItemUrl(Guid id);
    }
}