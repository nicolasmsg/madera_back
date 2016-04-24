using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Controllers
{
    /// <summary>
    /// Controller to manage <strong><see cref="ConceptionDevisWS.Models.User"/>s</strong> and their <strong>authentication</strong>.
    /// </summary>
    public class UserController : ApiController
    {
        /// <summary>
        /// Create a Login resource.
        /// </summary>
        /// <param name="user">the user to log in</param>
        /// <returns>the request's HttpStatusCode</returns>
        [Route("api/login")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> Login(User user)
        {
            object result = await UserService.Login(user, ActionContext.Request.RequestUri.Scheme + "://" + ActionContext.Request.RequestUri.Host);
            return Ok<object>(result);
        }

        /// <summary>
        /// Retrieve a <see cref="ConceptionDevisWS.Models.User"/>.
        /// </summary>
        /// <param name="id">the user's identity</param>
        /// <returns>the given user</returns>
        /// <exception cref="HttpResponseException">In case something went wront (for example the requested user doesn't exist).</exception>
        [Authorize]
        [Route("api/users/{id}")]
        public async Task<User> GetUser(int id)
        {
            return await UserService.GetUser(id);
        }

        /// <summary>
        /// Retrieve all existing <see cref="ConceptionDevisWS.Models.User"/>s.
        /// </summary>
        /// <returns>a list of users</returns>
        [Authorize]
        [Route("api/users")]
        [AcceptVerbs("GET")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await UserService.GetAllUsers();
        }

        /// <summary>
        /// Update completely an <see cref="ConceptionDevisWS.Models.User"/>.
        /// </summary>
        /// <param name="id">the user's identity</param>
        /// <param name="user">the updated user</param>
        /// <returns>the updated user</returns>
        /// <exception cref="HttpResponseException">In case something went wront (for example the requested user doesn't exist).</exception>
        [Authorize]
        [Route("api/users/{id}")]
        [AcceptVerbs("PUT")]
        public async Task<User> PutUser(int id, User user)
        {
            return await UserService.UpdateUser(id, user);
        }

        /// <summary>
        /// Create an <see cref="ConceptionDevisWS.Models.User"/>.
        /// </summary>
        /// <param name="user">the user to store</param>
        /// <returns>the created user</returns>
        [Route("api/registration")]
        [AcceptVerbs("POST")]
        public async Task<User> Register(User user)
        {
            return await UserService.Register(user);
        }

        /// <summary>
        /// Remove an <see cref="ConceptionDevisWS.Models.User"/>.
        /// </summary>
        /// <param name="id">the user's identity</param>
        /// <returns>the request's HttpStatusCode</returns>
        /// <exception cref="HttpResponseException">In case something went wront (for example the requested user doesn't exist).</exception>
        [Authorize]
        [Route("api/users/{id}")]
        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            await UserService.RemoveUser(id);
            return Ok();
        }

        /// <summary>
        /// Logs a <see cref="ConceptionDevisWS.Models.User"/> out.
        /// </summary>
        /// <remarks>
        /// This is not fully \htmlonly <accronym title="REpresentational State Transfer">REST</accronym>\endhtmlonly compliant, but it's usual.
        /// </remarks>
        /// <returns>the request's HttpStatusCode</returns>
        [Authorize]
        [Route("api/logout"), AcceptVerbs("GET")]
        public async Task<IHttpActionResult> Logout()
        {
            
            await UserService.Logout(RequestContext.Principal);
            return Ok();
        }
    }
}