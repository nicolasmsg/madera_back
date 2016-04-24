using ConceptionDevisWS.MessageHandlers;
using System.Web.Http;

namespace ConceptionDevisWS
{
    /// <summary>
    /// The application handling initial configuration.
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// The "OnStart" event handler register global MessageHandlers and WebApiConfig.
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsMessageHandler());
            GlobalConfiguration.Configuration.MessageHandlers.Add(new JwtAuthMessageHandler());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Da‌​teFormatString = "dd/MM/yyyy HH:mm:ss";
        }
    }
}
