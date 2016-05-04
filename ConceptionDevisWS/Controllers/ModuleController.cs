using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Controllers
{
    /// <summary>
    /// Controller to manage <see cref="ConceptionDevisWS.Models.Module"/>s.
    /// </summary>
    public class ModuleController : ApiController
    {
        /// <summary>
        /// Retrieve all existing <see cref="ConceptionDevisWS.Models.Module"/>s with their <see cref="ConceptionDevisWS.Models.Component"/>s. 
        /// </summary>
        /// <param name="lang">the culture to get modules into (fr-FR or en-US)</param>
        /// <returns>a list of modules</returns>
        [Authorize]
        [Route("api/modules")]
        public async Task<IEnumerable<Module>> GetAllModules([FromUri] string lang="fr-FR")
        {
            return await ModuleService.GetAllModules(lang);
        }

        /// <summary>
        /// Retrieve the given <see cref="ConceptionDevisWS.Models.Module"/>.
        /// </summary>
        /// <param name="id">the module's identity</param>
        /// <param name="lang">the culture to get the module into (fr-FR or en-US)</param>
        /// <returns>the given module</returns>
        /// <exception cref="HttpResponseException">In case something went wront (for example, the given module is not found).</exception>
        [Authorize]
        [Route("api/modules/{id}")]
        public async Task<Module> GetModule(int id, [FromUri] string lang="fr-FR")
        {
            return await ModuleService.GetModule(id, lang);
        }

        /// <summary>
        /// Remove the given module from storage.
        /// </summary>
        /// <param name="id">the module's identity</param>
        /// <returns>the request's HttpStatusCode</returns>
        /// <exception cref="HttpResponseException">In case something went wront (for example, the given module is not found).</exception>
        [Authorize]
        [Route("api/modules/{id}")]
        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> DeleteModule(int id)
        {
            await ModuleService.RemoveModule(id);
            return Ok();
        }

        /// <summary>
        /// Update completely the given <see cref="ConceptionDevisWS.Models.Module"/>.
        /// </summary>
        /// <param name="id">the module's identity</param>
        /// <param name="newModule">the updated module to store</param>
        /// <param name="lang">the culture to update the model into (fr-FR or en-US)</param>
        /// <returns>the updated module</returns>
        /// <exception cref="HttpResponseException">In case something went wront (for example, the given module is not found).</exception>
        [Authorize]
        [Route("api/modules/{id}")]
        [AcceptVerbs("PUT")]
        public async Task<Module> PutModule(int id, Module newModule, [FromUri]string lang="fr-FR")
        {
            return await ModuleService.UpdateModule(id, newModule, lang);
        }

        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Module"/>.
        /// </summary>
        /// <param name="newModule">the module to store</param>
        /// <param name="lang">the culture to create the module into (fr-FR or en-US)</param>
        /// <returns>the created module</returns>
        /// <exception cref="HttpResponseException">In case something went wront (when the given module is null).</exception>
        [Authorize]
        [Route("api/modules")]
        [AcceptVerbs("POST")]
        public async Task<Module> PostModule(Module newModule, [FromUri] string lang="fr-FR")
        {
            return await ModuleService.CreateNew(newModule, lang);
        }
    }
}
