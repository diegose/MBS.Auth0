using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Routes;

namespace MBS.Auth0 
{
    [OrchardFeature("MBS.Auth0")]
    public class Routes : IRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes() {
            yield return new RouteDescriptor {
                Priority = 5,
                Route = new Route("Auth0",
                    new RouteValueDictionary {{"area", "MBS.Auth0"}, {"controller", "Login"}, {"action", "Callback"}},
                    new RouteValueDictionary(),
                    new RouteValueDictionary {{"area", "MBS.Auth0"}},
                    new MvcRouteHandler())
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes()) routes.Add(routeDescriptor);
        }
    }
}