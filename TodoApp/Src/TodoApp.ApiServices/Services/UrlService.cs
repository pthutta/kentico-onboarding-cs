using System;
using System.Web.Http.Routing;
using TodoApp.Contracts.Routes;

namespace TodoApp.ApiServices.Services
{
    public class UrlService : IUrlService
    {
        private readonly UrlHelper _urlHelper;
        private readonly IRouteNames _routeNames;

        public UrlService(UrlHelper urlHelper, IRouteNames routeNames)
        {
            _urlHelper = urlHelper;
            _routeNames = routeNames;
        }

        public Uri GetItemUrl(Guid id)
            => new Uri(GetItemLink(id));

        private string GetItemLink(Guid id)
            => _urlHelper.Link(_routeNames.ItemRouteName, new { id });
    }
}