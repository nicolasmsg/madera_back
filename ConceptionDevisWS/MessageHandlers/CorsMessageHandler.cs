using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
//! Classes responsible for handling HTTP messages : that's to say generate an HttpResponse to fullfill an HttpRequest.
namespace ConceptionDevisWS.MessageHandlers
{
    /// <summary>
    /// MessageHandler to handle \htmlonly <acronym title="Cross Origin Resource Sharing">CORS</acronym>\endhtmlonly related http headers.
    /// </summary>
    public class CorsMessageHandler : DelegatingHandler
    {
        private void AddCorsHeaders(HttpResponseMessage response, IEnumerable<string> requestHeaders)
        {
            if(ConfigurationManager.AppSettings["AllowedDomains"] == null)
            {
                throw new ConfigurationErrorsException("La clé AllowedDomains doit être renseignée avec une liste de valeurs séparée par ; ou *");
            }
            response.Headers.Add("Access-Control-Allow-Origin", ConfigurationManager.AppSettings["AllowedDomains"].Split(';'));
            response.Headers.Add("Access-Control-Allow-Methods", new string[] { "GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS" });
            response.Headers.Add("Access-Control-Allow-Headers", requestHeaders);
        }

        /// <summary>
        /// Add CORS http headers.
        /// </summary>
        /// <remarks>
        /// Headers set here : (Access-Control-Allow-Origin,Access-Control-Allow-Methods,Access-Control-Allow-Headers)
        /// </remarks>
        /// <param name="request">the request</param>
        /// <param name="cancellationToken">a token to cancel the request from antoher thread</param>
        /// <returns>the http response</returns>
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains("Origin"))
            {
                HttpResponseMessage response = null;
                if (request.Method == HttpMethod.Options)
                {
                    response = request.CreateResponse(HttpStatusCode.OK);
                    AddCorsHeaders(response, request.Headers.GetValues("Access-Control-Request-Headers"));
                }
                else
                {
                    response = await base.SendAsync(request, cancellationToken);
                    response.Headers.Add("Access-Control-Allow-Origin",  request.Headers.GetValues("Origin").First());
                }
                
                return response;
            }
            return await base.SendAsync(request, cancellationToken);
            
        }
    }
}