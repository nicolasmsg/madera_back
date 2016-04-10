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
        [Route("api/projects")]
        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            return await ProjectService.GetAllProjects();
        }

        [Authorize]
        [Route("api/projects/{id}")]
        public async Task<Project> GetProject(int id)
        {
            return await ProjectService.GetProject(id);
        }

        [Authorize]
        [Route("api/projects/{id}")]
        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> DeleteProject(int id)
        {
            await ProjectService.RemoveProject(id);
            return Ok();
        }

        [Authorize]
        [Route("api/projects/{id}")]
        [AcceptVerbs("PATCH")]
        public async Task<Project> PatchProject(int id, Project newProject)
        {
            return await ProjectService.UpdateProject(id, newProject);
        }

        [Authorize]
        [Route("api/projects")]
        [AcceptVerbs("POST")]
        public async Task<Project> PostProject(Project newProject)
        {
            return await ProjectService.CreateNew(newProject);
        }
    }
}
