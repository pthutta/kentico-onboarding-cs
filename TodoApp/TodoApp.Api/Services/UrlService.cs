using System;
using System.Web.Http.Routing;
using TodoApp.Contracts.Services;

namespace TodoApp.Api.Services
{
    public class UrlService : IUrlService
    {
        public const string NewItemRouteName = "NewItemRoute";

        private readonly UrlHelper _urlHelper;

        public UrlService(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public string GetItemUrl(Guid id)
            => _urlHelper.Route(NewItemRouteName, new { id });
    }
}