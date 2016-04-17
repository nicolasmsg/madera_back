using ConceptionDevisWS.MessageHandlers;
using System.Web.Http;

namespace ConceptionDevisWS
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsMessageHandler());
            GlobalConfiguration.Configuration.MessageHandlers.Add(new JwtAuthMessageHandler());
            
        }
    }
}
