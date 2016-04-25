using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Controllers
{
    /// <summary>
    /// Controller to manager <see cref="ConceptionDevisWS.Models.Model"/>.
    /// </summary>
    public class ModelController : ApiController
    {
        /// <summary>
        /// Retrieve all existing <see cref="ConceptionDevisWS.Models.Model"/>s.
        /// </summary>
        /// <returns>a list of models</returns>
        [Authorize]
        [Route("api/models")]
        public async Task<List<Model>> GetAllModels()
        {
            return await ModelService.GetAllModels();
        }

        /// <summary>
        /// Retrieve the given <see cref="ConceptionDevisWS.Models.Model"/>.
        /// </summary>
        /// <param name="id">the model's identity</param>
        /// <returns>the model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested model does not exists).</exception>
        [Authorize]
        [Route("api/models/{id}")]
        public async Task<Model> GetModel(int id)
        {
            return await ModelService.GetModel(id);
        }

        /// <summary>
        /// Remove the given <see cref="ConceptionDevisWS.Models.Model"/> from storage.
        /// </summary>
        /// <param name="id">the model's identity</param>
        /// <returns>The request's HttpStatusCode</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested model does not exists).</exception>
        [Authorize]
        [Route("api/models/{id}")]
        public async Task<IHttpActionResult> DeleteModel(int id)
        {
            await ModelService.RemoveModel(id);
            return Ok();
        }

        /// <summary>
        /// Update completely the given <see cref="ConceptionDevisWS.Models.Model"/>.
        /// </summary>
        /// <param name="id">the model's identity</param>
        /// <param name="newModel">the updated model to store</param>
        /// <returns>the updated model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested model does not exists).</exception>
        [Authorize]
        [Route("api/models/{id}")]
        [AcceptVerbs("PUT")]
        public async Task<Model> PutModel(int id, Model newModel)
        {
            return await ModelService.UpdateModel(id, newModel);
        }

        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Model"/>.
        /// </summary>
        /// <param name="newModel">the model to store</param>
        /// <returns>the stored model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (when the given model is null).</exception>
        [Authorize]
        [Route("api/models")]
        [AcceptVerbs("POST")]
        public async Task<Model> PostModel(Model newModel)
        {
            return await ModelService.CreateNew(newModel);
        }
    }
}
