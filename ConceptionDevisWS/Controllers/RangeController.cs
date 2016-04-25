

using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Controllers
{
    /// <summary>
    /// Controller to manage <see cref="ConceptionDevisWS.Models.Range"/>s.
    /// </summary>
    public class RangeController : ApiController
    {
        /// <summary>
        /// Retrieve all existing <see cref="ConceptionDevisWS.Models.Range"/>s with their <see cref="ConceptionDevisWS.Models.Model"/>s.
        /// </summary>
        /// <returns>a list of ranges</returns>
        [Authorize]
        [Route("api/ranges")]
        public async Task<IEnumerable<Range>> GetAllRanges()
        {
            return await RangeService.GetAllRanges();
        }

        /// <summary>
        /// Retrieve the given <see cref="ConceptionDevisWS.Models.Range"/>.
        /// </summary>
        /// <param name="id">the range's identity</param>
        /// <returns>the range</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range does not exists).</exception>
        [Authorize]
        [Route("api/ranges/{id}")]
        public async Task<Range> GetRange(int id)
        {
            return await RangeService.GetRange(id);
        }

        /// <summary>
        /// Remove the given <see cref="ConceptionDevisWS.Models.Range"/> from storage.
        /// </summary>
        /// <param name="id">the range's identity</param>
        /// <returns>The request's HttpStatusCode</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range does not exists).</exception>
        [Authorize]
        [Route("api/ranges/{id}")]
        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> DeleteRange(int id)
        {
            await RangeService.RemoveRange(id);
            return Ok();
        }

        /// <summary>
        /// Update completely the given <see cref="ConceptionDevisWS.Models.Range"/>.
        /// </summary>
        /// <param name="id">the range's identity</param>
        /// <param name="newRange">the updated range to store</param>
        /// <returns>the updated range</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range does not exists).</exception>
        [Authorize]
        [Route("api/ranges/{id}")]
        [AcceptVerbs("PUT")]
        public async Task<Range> PutRange(int id, Range newRange)
        {
            return await RangeService.UpdateRange(id, newRange);
        }

        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Range"/>.
        /// </summary>
        /// <param name="newRange">the range to store</param>
        /// <returns>the stored range</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (when the given range is null).</exception>
        [Authorize]
        [Route("api/ranges")]
        [AcceptVerbs("POST")]
        public async Task<Range> PostRange(Range newRange)
        {
            return await RangeService.CreateNew(newRange);
        }
    }
}
