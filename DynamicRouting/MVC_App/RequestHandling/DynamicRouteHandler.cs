using System.Web;
using System.Web.Routing;

namespace Custom.DynamicRouting.RequestHandling
{
    public class DynamicRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new DynamicHttpHandler(requestContext);
        }
    }
}