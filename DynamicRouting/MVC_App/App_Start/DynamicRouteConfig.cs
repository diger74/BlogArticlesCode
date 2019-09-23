using System.Web.Mvc;
using System.Web.Routing;
using Custom.Domain;
using Custom.DynamicRouting.Helpers;
using Custom.DynamicRouting.RequestHandling;
using DancingGoat.Infrastructure;

namespace Custom.DynamicRouting
{
    public class DynamicRouteConfig
    {
        public static void RegisterDynamicRoutes(RouteCollection routes)
        {
            PageTypeRoutingHelper.Initialize();

            var route = routes.MapRoute(
                name: "CheckByUrl",
                url: $"{{culture}}/{{*{Constants.DynamicRouting.RoutingUrlParameter}}}",
                defaults: new {defaultcontroller = "HttpErrors", defaultaction = "Index"},
                constraints: new {culture = new SiteCultureConstraint(), PageFound = new PageFoundConstraint()}
            );
            route.RouteHandler = new DynamicRouteHandler();
        }
    }
}