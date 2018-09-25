using System;

namespace TodoApp.Contracts.Services
{
    public interface IUrlService
    {
        Uri GetItemUrl(Guid id);
    }
}