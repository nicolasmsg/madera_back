using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Controllers
{
    public class ProjectController : ApiController
    {
        [Authorize]
        [Route("api/clients/{clientId}/projects")]
        public async Task<IEnumerable<Project>> GetClientProjects(int clientId)
        {
            return await ProjectService.GetClientProjects(clientId);
        }

        [Authorize]
        [Route("api/clients/{clientId}/projects/{id}")]
        [AcceptVerbs("GET")]
        public async Task<Project> GeClientProject(int clientId, int id)
        {
            return await ProjectService.GetClientProject(clientId, id);
        }

        [Authorize]
        [Route("api/clients/{clientId}/projects/{id}")]
        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> DeleteProject(int clientId, int id)
        {
            await ProjectService.RemoveProject(clientId, id);
            return Ok();
        }

        [Authorize]
        [Route("api/clients/{clientId}/projects/{id}")]
        [AcceptVerbs("PATCH")]
        public async Task<Project> PatchProject(int clientId, int id, Project newProject)
        {
            return await ProjectService.UpdateProject(clientId, id, newProject);
        }

        [Authorize]
        [Route("api/clients/{clientId}/projects")]
        [AcceptVerbs("POST")]
        public async Task<Project> PostProject(int clientId, Project newProject)
        {
            return await ProjectService.CreateNew(clientId, newProject);
        }
    }
}
