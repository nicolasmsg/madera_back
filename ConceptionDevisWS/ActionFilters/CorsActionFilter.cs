using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace ConceptionDevisWS.ActionFilters
{
    public class CorsActionFilter : ActionFilterAttribute
    {
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", ConfigurationManager.AppSettings["AllowedDomains"].Split(';'));
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Methods", new string[] { "GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS" });
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
    }
}