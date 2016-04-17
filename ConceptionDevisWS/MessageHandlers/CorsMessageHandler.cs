using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace ConceptionDevisWS.MessageHandlers
{
    public class CorsMessageHandler : DelegatingHandler
    {
        private void AddCorsHeaders(HttpResponseMessage response, IEnumerable<string> requestHeaders)
        {
            response.Headers.Add("Access-Control-Allow-Origin", ConfigurationManager.AppSettings["AllowedDomains"].Split(';'));
            response.Headers.Add("Access-Control-Allow-Methods", new string[] { "GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS" });
            response.Headers.Add("Access-Control-Allow-Headers", requestHeaders);
        }

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