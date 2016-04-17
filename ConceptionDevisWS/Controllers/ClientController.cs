using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Controllers
{
    public class ClientController : ApiController
    {
        [Authorize]
        [Route("api/clients")]
        public async Task<IEnumerable<Client>> GetAllClients()
        {
            return await ClientService.GetAllClients();
        }

        [Authorize]
        [Route("api/clients/{id}")]
        public async Task<Client> GetClient(int id)
        {
            return await ClientService.GetClient(id);
        }

        [Authorize]
        [Route("api/clients/{id}")]
        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> DeleteClient(int id)
        {
            await ClientService.RemoveClient(id);
            return Ok();
        }

        [Authorize]
        [Route("api/clients/{id}")]
        [AcceptVerbs("PUT")]
        public async Task<Client> PutClient(int id, Client newClient)
        {
            return await ClientService.UpdateClient(id, newClient);
        }

        [Authorize]
        [Route("api/clients")]
        [AcceptVerbs("POST")]
        public async Task<Client> PostClient(Client newClient)
        {
            return await ClientService.CreateNew(newClient);
        }
    }
}