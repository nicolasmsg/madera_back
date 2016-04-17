using ConceptionDevisWS.ActionFilters;
using ConceptionDevisWS.MessageHandlers;
using System.Web.Http;

namespace ConceptionDevisWS
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new JwtAuthMessageHandler());
            GlobalConfiguration.Configuration.Filters.Add(new CorsActionFilter());
        }
    }
}
