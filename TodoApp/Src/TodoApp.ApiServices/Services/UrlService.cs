using System;
using System.Web.Http.Routing;
using TodoApp.Contracts.Services;

namespace TodoApp.ApiServices.Services
{
    public class UrlService : IUrlService
    {
        public const string NewItemRouteName = "NewItemRoute";

        private readonly UrlHelper _urlHelper;

        public UrlService(UrlHelper urlHelper)
            => _urlHelper = urlHelper;

        public Uri GetItemUrl(Guid id)
            => new Uri(GetItemLink(id));

        private string GetItemLink(Guid id)
            => _urlHelper.Link(NewItemRouteName, new { id });
    }
}