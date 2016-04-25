using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

//! <a href="http://www.oodesign.com/mediator-pattern.html">Mediators</a> between the <see cref="ConceptionDevisWS.WebApiConfig">routing</see> and <see cref="Services">services</see> layers.
namespace ConceptionDevisWS.Controllers
{
    /// <summary>
    /// Controller to manage <see cref="ConceptionDevisWS.Models.Client"/>s.
    /// </summary>
    public class ClientController : ApiController
    {
        /// <summary>
        /// Retrieve all existing <see cref="ConceptionDevisWS.Models.Client"/>s with their <see cref="ConceptionDevisWS.Models.Project"/>s.
        /// </summary>
        /// <returns>a list of clients</returns>
        [Authorize]
        [Route("api/clients")]
        public async Task<IEnumerable<Client>> GetAllClients()
        {
            return await ClientService.GetAllClients();
        }

        /// <summary>
        /// Retrieve the given <see cref="ConceptionDevisWS.Models.Client"/>.
        /// </summary>
        /// <param name="id">the client's identity</param>
        /// <returns>the client</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested client does not exists).</exception>
        [Authorize]
        [Route("api/clients/{id}")]
        public async Task<Client> GetClient(int id)
        {
            return await ClientService.GetClient(id);
        }

        /// <summary>
        /// Remove the given <see cref="ConceptionDevisWS.Models.Client"/> from storage.
        /// </summary>
        /// <param name="id">the client's identity</param>
        /// <returns>The request's HttpStatusCode</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested client does not exists).</exception>
        [Authorize]
        [Route("api/clients/{id}")]
        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> DeleteClient(int id)
        {
            await ClientService.RemoveClient(id);
            return Ok();
        }

        /// <summary>
        /// Update completely the given <see cref="ConceptionDevisWS.Models.Client"/>.
        /// </summary>
        /// <param name="id">the client's identity</param>
        /// <param name="newClient">the updated client to store</param>
        /// <returns>the updated client</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested client does not exists).</exception>
        [Authorize]
        [Route("api/clients/{id}")]
        [AcceptVerbs("PUT")]
        public async Task<Client> PutClient(int id, Client newClient)
        {
            return await ClientService.UpdateClient(id, newClient);
        }

        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Client"/>.
        /// </summary>
        /// <param name="newClient">the client to store</param>
        /// <returns>the stored client</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (when the given client is null).</exception>
        [Authorize]
        [Route("api/clients")]
        [AcceptVerbs("POST")]
        public async Task<Client> PostClient(Client newClient)
        {
            return await ClientService.CreateNew(newClient);
        }
    }
}