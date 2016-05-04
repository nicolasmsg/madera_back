using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Controllers
{
    /// <summary>
    /// Controller to manage <see cref="ConceptionDevisWS.Models.Project"/>s.
    /// </summary>
    /// <remarks>
    /// A <see cref="Project"/> is only accessible through its <see cref="Client"/>. It doesn't exist without a Client.
    /// </remarks>
    public class ProjectController : ApiController
    {
        /// <summary>
        /// Retrieve a <see cref="ConceptionDevisWS.Models.Client"/>'s <see cref="ConceptionDevisWS.Models.Project"/>s.
        /// </summary>
        /// <param name="clientId">the client's identity</param>
        /// <param name="lang">the culture to get projects into (fr-FR or en-US)</param>
        /// <returns>a list of projects</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example when the given client doesn't exist).</exception>
        [Authorize]
        [Route("api/clients/{clientId}/projects")]
        public async Task<IEnumerable<Project>> GetClientProjects(int clientId, [FromUri] string lang="fr-FR")
        {
            return await ProjectService.GetClientProjects(clientId, lang);
        }

        /// <summary>
        /// Retrieve a <see cref="ConceptionDevisWS.Models.Client"/>'s <see cref="ConceptionDevisWS.Models.Project"/>.
        /// </summary>
        /// <param name="clientId">the client's identity</param>
        /// <param name="id">the project's identity</param>
        /// <param name="lang">the culture to get the project into (fr-FR or en-US)</param>
        /// <returns>a project</returns>
        /// <exception cref="HttpResponseException">In case either the client or project doesn't exist.</exception>
        [Authorize]
        [Route("api/clients/{clientId}/projects/{id}")]
        [AcceptVerbs("GET")]
        public async Task<Project> GeClientProject(int clientId, int id, [FromUri] string lang="fr-FR")
        {
            return await ProjectService.GetClientProject(clientId, id, lang);
        }

        /// <summary>
        /// Remove the given <see cref="ConceptionDevisWS.Models.Project"/> from storage.
        /// </summary>
        /// <param name="clientId">the client's identity</param>
        /// <param name="id">the project's identity</param>
        /// <returns>the request's HttpStatusCode</returns>
        /// <exception cref="HttpResponseException">In case either the client or project doesn't exist.</exception>
        [Authorize]
        [Route("api/clients/{clientId}/projects/{id}")]
        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> DeleteProject(int clientId, int id)
        {
            await ProjectService.RemoveProject(clientId, id);
            return Ok();
        }

        /// <summary>
        /// Update completely a given <see cref="ConceptionDevisWS.Models.Project"/>.
        /// </summary>
        /// <param name="clientId">the client's identity</param>
        /// <param name="id">the project's identity</param>
        /// <param name="newProject">the updated project </param>
        /// <param name="lang">the culture to update the project into (fr-FR or en-US)</param>
        /// <returns>the updated project</returns>
        /// <exception cref="HttpResponseException">In case either the client or project doesn't exist.</exception>
        [Authorize]
        [Route("api/clients/{clientId}/projects/{id}")]
        [AcceptVerbs("PUT")]
        public async Task<Project> PutProject(int clientId, int id, Project newProject, [FromUri]string lang = "fr-FR")
        {
            return await ProjectService.UpdateProject(clientId, id, newProject, lang);
        }

        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Project"/> for an existing <see cref="ConceptionDevisWS.Models.Client"/>.
        /// </summary>
        /// <param name="clientId">the client's identity</param>
        /// <param name="newProject">the project to store</param>
        /// <param name="lang">the culture to create the project into (fr-FR or en-US)</param>
        /// <returns>the created project</returns>
        [Authorize]
        [Route("api/clients/{clientId}/projects")]
        [AcceptVerbs("POST")]
        public async Task<Project> PostProject(int clientId, Project newProject, [FromUri]string lang="fr-FR")
        {
            return await ProjectService.CreateNew(clientId, newProject, lang);
        }
    }
}
