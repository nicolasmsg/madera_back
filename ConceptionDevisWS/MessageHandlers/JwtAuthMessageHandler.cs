using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using ConceptionDevisWS.Services.Utils;
using System;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ConceptionDevisWS.MessageHandlers
{
    /// <summary>
    /// Class to handle token based authentication (to be used over crypted protocol such as https)
    /// </summary>
    public class JwtAuthMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// Check the request's token, and if it's a valid jwt token, send the request through next handler down to the controller.
        /// </summary>
        /// <param name="request">the http request</param>
        /// <param name="cancellationToken">a token to cancel the request from another thread</param>
        /// <returns></returns>
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
            string requestRouteTemplate = request.RequestUri.PathAndQuery.Substring(1);
            string key = "az4s";
            string key2 = "a2un";
            string key3 = "e7gu";
            string unauthorizedMessage = "Unauthorized request";
            string [] freeRoutes = { "api/login", "api/registration" };

            if(Array.Find<string>(freeRoutes, s => s == requestRouteTemplate) != null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            if (!request.Headers.Contains(key) || !request.Headers.Contains(key2) || !request.Headers.Contains(key3))
            {
                return request.CreateErrorResponse(HttpStatusCode.Unauthorized, unauthorizedMessage);
            }

            try
            {
                ClaimsPrincipal identifiedUser = JsonWebTokenManager.ValidateToken(request.Headers.GetValues(key).ElementAt(0), request.RequestUri.Scheme + "://" + request.RequestUri.Host);


                if (identifiedUser == null)
                {
                    return request.CreateErrorResponse(HttpStatusCode.Unauthorized, unauthorizedMessage);
                }

                if(await UserService.HasLoggedOut(identifiedUser))
                {
                    // we do not throw a SecurityTokenExpiredException as Exception catching is time consumming
                    return request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Timeout");
                }
                User currentUser = await UserService.GetUser(identifiedUser);
                request.GetRequestContext().Principal = identifiedUser;
                request.Properties.Add("User", currentUser);
                return await base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenExpiredException)
            {
                return request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Timeout");
            }
            catch (SecurityTokenValidationException)
            {
                return request.CreateErrorResponse(HttpStatusCode.Unauthorized, unauthorizedMessage);
            }

        }
    }
}