using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ConceptionDevisWS.Controllers
{
    public class UserController : ApiController
    {

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        
        [Route("api/login")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> Login(User user)
        {
            object result = await UserService.Login(user, ActionContext.Request.RequestUri.Scheme + "://" + ActionContext.Request.RequestUri.Host);
            return Ok<object>(result);
        }
        
        [Route("api/registration")]
        [AcceptVerbs("POST")]
        public async Task<User> Register(User user)
        {
            return await UserService.Register(user);
        }

        [Authorize]
        [Route("api/logout"), AcceptVerbs("GET")]
        public async Task<IHttpActionResult> Logout()
        {
            
            await UserService.Logout(RequestContext.Principal);
            return Ok();
        }
    }
}