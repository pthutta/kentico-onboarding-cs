using TodoApp.Contracts.Routes;

namespace TodoApp.Api.Routes
{
    internal class RouteNames : IRouteNames
    {
        internal const string NewItemRouteName = "NewItemRoute";

        public string ItemRouteName => NewItemRouteName;
    }
}
