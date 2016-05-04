using ConceptionDevisWS.Models;
using ConceptionDevisWS.Models.Converters;
using ConceptionDevisWS.Services.Utils;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
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
        /// <param name="lang">the culture to get the models into (fr-FR or en-US)</param>
        /// <returns>a list of models</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the given range does not exists).</exception>
        public async static Task<List<Model>> GetRangeModels(int rangeId, string lang)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Range seekedRange = await _searchRange(rangeId, lang);
                return seekedRange.Models;

            }
        }

        /// <summary>
        /// Retrieve the given <see cref="ConceptionDevisWS.Models.Range"/>'s <see cref="ConceptionDevisWS.Models.Model"/>.
        /// </summary>
        /// <param name="rangeId">the range's identity</param>
        /// <param name="id">the model's identity</param>
        /// <param name="lang">the culture to get the model into (fr-FR or en-US)</param>
        /// <returns>the model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range or model does not exists).</exception>
        public async static Task<Model> GetRangeModel(int rangeId, int id, string lang)
        {
            return await _searchRangeModel(rangeId, id, lang);
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
                Model seekedModel = await _searchRangeModel(rangeId, id, "fr-FR");
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
        /// <param name="lang">the culture to update this range into (fr-FR or en-US)</param>
        /// <returns>the updated model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range or model does not exists).</exception>
        public async static Task<Model> UpdateModel(int rangeId, int id, Model newModel, string lang)
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

                Model seekedModel = await _searchRangeModel(rangeId, id, lang);
                ctx.Entry(seekedModel).State = EntityState.Modified;
                await ServiceHelper<Model>.SetSingleNavigationProperty<Range>(newModel, seekedModel, ctx, m => m.Range, _getCtxRanges, _setRange);

                seekedModel.UpdateNonComposedPropertiesFrom(newModel);

                bool updateSuccess = true;
                do
                {
                    try
                    {
                        await ctx.SaveChangesAsync();
                        updateSuccess = true;
                    } catch(DbUpdateConcurrencyException dbuce)
                    {
                        DbEntityEntry entry = dbuce.Entries.Single();
                        entry.OriginalValues.SetValues(await entry.GetDatabaseValuesAsync());
                    }
                } while (!updateSuccess);
                return seekedModel;

            }
        }

        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Model"/> for an existing <see cref="ConceptionDevisWS.Models.Range"/>.
        /// </summary>
        /// <param name="rangeId">the range's identity</param>
        /// <param name="newModel">the model to store</param>
        /// <param name="lang">the culture to create the model into (fr-FR or en-US)</param>
        /// <returns>the stored model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (when the given range does not exists or the model is null).</exception>
        public async static Task<Model> CreateNew(int rangeId, Model newModel, string lang)
        {
            if (newModel == null || newModel.Name == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            Range seekedRange = await _searchRange(rangeId, lang);
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
            Range range = await _searchRange(rangeId, "fr-FR");
            return (model.FrameQuality & range.FrameQualities) == model.FrameQuality
                && (model.ExtFinishing & range.ExtFinishings) == model.ExtFinishing;
        }

        private async static Task<Range> _searchRange(int rangeId, string lang)
        {
            return await RangeService.GetRange(rangeId, lang);
        }

        private async static Task<Model> _searchRangeModel(int rangeId, int id, string lang)
        {
            if (id == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            CulturalEnumStringConverter.Culture = new CultureInfo(lang);
            Range seekedRange = await _searchRange(rangeId, lang);
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