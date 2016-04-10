using ConceptionDevisWS.Services;
using ConceptionDevisWS.Services.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ConceptionDevisWS.MessageHandlers
{
    public class JwtAuthMessageHandler : DelegatingHandler
    {
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

                request.GetRequestContext().Principal = identifiedUser;
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