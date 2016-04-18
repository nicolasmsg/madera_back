using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using System.Collections.Generic;
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
        
        [Authorize]
        [Route("api/users/{id}")]
        public async Task<User> GetUser(int id)
        {
            return await UserService.GetUser(id);
        }

        [Authorize]
        [Route("api/users")]
        [AcceptVerbs("GET")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await UserService.GetAllUsers();
        }

        [Authorize]
        [Route("api/users/{id}")]
        [AcceptVerbs("PUT")]
        public async Task<User> PutUser(int id, User user)
        {
            return await UserService.UpdateUser(id, user);
        }

        [Route("api/registration")]
        [AcceptVerbs("POST")]
        public async Task<User> Register(User user)
        {
            return await UserService.Register(user);
        }

        [Authorize]
        [Route("api/users/{id}")]
        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            await UserService.RemoveUser(id);
            return Ok();
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