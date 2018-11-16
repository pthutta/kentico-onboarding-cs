using System;

namespace TodoApp.Contracts.Routes
{
    public interface IUrlService
    {
        Uri GetItemUrl(Guid id);
    }
}