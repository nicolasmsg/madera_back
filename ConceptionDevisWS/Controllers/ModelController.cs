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
        /// Retrieve a given <see cref="ConceptionDevisWS.Models.Range"/>'s <see cref="ConceptionDevisWS.Models.Model"/>s.
        /// </summary>
        /// <param name="rangeId">the range's identity</param>
        /// <param name="lang">the culture to get models into (fr-FR or en-US)</param>
        /// <returns>a list of models</returns>
        /// /// <exception cref="HttpResponseException">In case something went wrong (for example, when the given range does not exists).</exception>
        [Authorize]
        [Route("api/ranges/{rangeId}/models")]
        public async Task<List<Model>> GetRangeModels(int rangeId, [FromUri]string lang="fr-FR")
        {
            return await ModelService.GetRangeModels(rangeId, lang);
        }

        /// <summary>
        /// Retrieve the given <see cref="ConceptionDevisWS.Models.Range"/>'s <see cref="ConceptionDevisWS.Models.Model"/>.
        /// </summary>
        /// <param name="rangeId">the range's identity</param>
        /// <param name="id">the model's identity</param>
        /// <param name="lang">the culture to get the model into (fr-FR or en-US)</param>
        /// <returns>the model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range or model does not exists).</exception>
        [Authorize]
        [Route("api/ranges/{rangeId}/models/{id}")]
        public async Task<Model> GetRangeModel(int rangeId, int id, [FromUri] string lang="fr-FR")
        {
            return await ModelService.GetRangeModel(rangeId, id, lang);
        }

        /// <summary>
        /// Remove the given <see cref="ConceptionDevisWS.Models.Model"/> from storage.
        /// </summary>
        /// <param name="rangeId">the range'es identity</param>
        /// <param name="id">the model's identity</param>
        /// <returns>The request's HttpStatusCode</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range or model does not exists).</exception>
        [Authorize]
        [Route("api/ranges/{rangeId}/models/{id}")]
        public async Task<IHttpActionResult> DeleteModel(int rangeId, int id)
        {
            await ModelService.RemoveModel(rangeId, id);
            return Ok();
        }

        /// <summary>
        /// Update completely the given <see cref="ConceptionDevisWS.Models.Model"/>.
        /// </summary>
        /// <param name="rangeId">the range's identity</param>
        /// <param name="id">the model's identity</param>
        /// <param name="newModel">the updated model to store</param>
        /// <param name="lang">the culture to update the model into (fr-FR or en-US)</param>
        /// <returns>the updated model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range or model does not exists).</exception>
        [Authorize]
        [Route("api/ranges/{rangeId}/models/{id}")]
        [AcceptVerbs("PUT")]
        public async Task<Model> PutModel(int rangeId, int id, Model newModel, [FromUri] string lang="fr-FR")
        {
            return await ModelService.UpdateModel(rangeId, id, newModel, lang);
        }

        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Model"/> for an existing <see cref="ConceptionDevisWS.Models.Range"/>.
        /// </summary>
        /// <param name="rangeId">the range's identity</param>
        /// <param name="newModel">the model to store</param>
        /// <param name="lang">the culture to create the model into (fr-FR or en-US)</param>
        /// <returns>the stored model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (when the given range does not exists or the model is null).</exception>
        [Authorize]
        [Route("api/ranges/{rangeId}/models/")]
        [AcceptVerbs("POST")]
        public async Task<Model> PostModel(int rangeId, Model newModel, [FromUri] string lang="fr-FR")
        {
            return await ModelService.CreateNew(rangeId, newModel, lang);
        }
    }
}
