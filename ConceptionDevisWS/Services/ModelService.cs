using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services.Utils;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Services
{
    /// <summary>
    /// A Service to manage a <see cref="ConceptionDevisWS.Models.Model"/>'s business.
    /// </summary>
    public class ModelService
    {
        /// <summary>
        /// Retrieve a given <see cref="ConceptionDevisWS.Models.Range"/>'s <see cref="ConceptionDevisWS.Models.Model"/>s.
        /// </summary>
        /// <param name="rangeId">the range's identity</param>
        /// <returns>a list of models</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the given range does not exists).</exception>
        public async static Task<List<Model>> GetRangeModels(int rangeId)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Range seekedRange = await _searchRange(rangeId);
                return seekedRange.Models;

            }
        }

        /// <summary>
        /// Retrieve the given <see cref="ConceptionDevisWS.Models.Range"/>'s <see cref="ConceptionDevisWS.Models.Model"/>.
        /// </summary>
        /// <param name="rangeId">the range's identity</param>
        /// <param name="id">the model's identity</param>
        /// <returns>the model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range or model does not exists).</exception>
        public async static Task<Model> GetRangeModel(int rangeId, int id)
        {
            return await _searchRangeModel(rangeId, id);
        }

        /// <summary>
        /// Remove the given <see cref="ConceptionDevisWS.Models.Model"/> from storage.
        /// </summary>
        /// <param name="rangeId">the range'es identity</param>
        /// <param name="id">the model's identity</param>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range or model does not exists).</exception>
        public async static Task RemoveModel(int rangeId, int id)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Model seekedModel = await _searchRangeModel(rangeId, id);
                ctx.Entry(seekedModel).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update completely the given <see cref="ConceptionDevisWS.Models.Model"/>.
        /// </summary>
        /// <param name="rangeId">the range's identity</param>
        /// <param name="id">the model's identity</param>
        /// <param name="newModel">the updated model to store</param>
        /// <returns>the updated model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range or model does not exists).</exception>
        public async static Task<Model> UpdateModel(int rangeId, int id, Model newModel)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                if (newModel.Range == null)
                {
                    newModel.Range = new Range();
                }
                newModel.Range.Id = rangeId;

                if(!await Validate(rangeId, newModel))
                {
                    string errMsg = "Invalid content : The model ExternalFinishing or FrameQuality does not match its Range.";
                    HttpResponseMessage responseMessage = new HttpResponseMessage { StatusCode=HttpStatusCode.BadRequest, Content = new StringContent(errMsg) };
                    throw new HttpResponseException(responseMessage);
                }

                Model seekedModel = await _searchRangeModel(rangeId, id);
                ctx.Entry(seekedModel).State = EntityState.Modified;
                await ServiceHelper<Model>.SetSingleNavigationProperty<Range>(newModel, seekedModel, ctx, m => m.Range, _getCtxRanges, _setRange);

                seekedModel.UpdateNonComposedPropertiesFrom(newModel);
                await ctx.SaveChangesAsync();
                return seekedModel;

            }
        }

        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Model"/> for an existing <see cref="ConceptionDevisWS.Models.Range"/>.
        /// </summary>
        /// <param name="rangeId">the range's identity</param>
        /// <param name="newModel">the model to store</param>
        /// <returns>the stored model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (when the given range does not exists or the model is null).</exception>
        public async static Task<Model> CreateNew(int rangeId, Model newModel)
        {
            if (newModel == null || newModel.Name == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            Range seekedRange = await _searchRange(rangeId);
            newModel.Range = seekedRange;

            if (!await Validate(rangeId, newModel))
            {
                string errMsg = "Invalid content : The model ExternalFinishing or FrameQuality does not match its Range.";
                HttpResponseMessage responseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(errMsg) };
                throw new HttpResponseException(responseMessage);
            }

            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                await ServiceHelper<Model>.LoadSingleNavigationProperty<Range>(newModel, ctx, m => m.Range, _getCtxRanges, _setRange);
                ctx.Models.Add(newModel);
                await ctx.SaveChangesAsync();
                return newModel;
            }
        }

        public async static Task<bool> Validate(int rangeId, Model model)
        {
            Range range = await _searchRange(rangeId);
            return (model.FrameQuality & range.FrameQualities) == model.FrameQuality
                && (model.ExtFinishing & range.ExtFinishings) == model.ExtFinishing;
        }

        private async static Task<Range> _searchRange(int rangeId)
        {
            return await RangeService.GetRange(rangeId);
        }

        private async static Task<Model> _searchRangeModel(int rangeId, int id)
        {
            if (id == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            Range seekedRange = await _searchRange(rangeId);
            Model seekedModel = seekedRange.Models.FirstOrDefault(m => m.Id == id);
            if (seekedRange == null || seekedModel == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return seekedModel;
        }

        private static DbSet<Range> _getCtxRanges(DbContext context)
        {
            return ((ModelsDBContext)context).Ranges;
        }

        private static void _setRange(Model m, Range r)
        {
            m.Range = r;
        }
    }
}