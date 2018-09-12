using System.Web.Http;
using System.Web.Http.Routing;
using TodoApp.Api.Resolvers;
using TodoApp.Contracts.Repositories;
using TodoApp.DAL.Repositories;
using Unity;
using Unity.Lifetime;

namespace TodoApp.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var constraintResolver = new DefaultInlineConstraintResolver
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof( ApiVersionRouteConstraint )
                }
            };

            // Web API routes
            config.MapHttpAttributeRoutes(constraintResolver);
            config.AddApiVersioning();
            var container = new UnityContainer();
            container.RegisterType<IItemRepository, ItemRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}
