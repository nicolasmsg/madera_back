using System.Web.Http;

namespace ConceptionDevisWS
{
    /// <summary>
    /// Configures the routes for this WebService
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register the routing configuration
        /// </summary>
        /// <param name="config">The routing configuration</param>
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
