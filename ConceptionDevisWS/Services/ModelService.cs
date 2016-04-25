using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConceptionDevisWS.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Net;
using ConceptionDevisWS.Services.Utils;

namespace ConceptionDevisWS.Services
{
    /// <summary>
    /// A Service to manage a <see cref="ConceptionDevisWS.Models.Model"/>'s business.
    /// </summary>
    public class ModelService
    {
        /// <summary>
        /// Retrieve all existing <see cref="ConceptionDevisWS.Models.Model"/>s.
        /// </summary>
        /// <returns>a list of models</returns>
        public async static Task<List<Model>> GetAllModels()
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Models.ToListAsync();
            }
        }

        /// <summary>
        /// Retrieve the given <see cref="ConceptionDevisWS.Models.Model"/>.
        /// </summary>
        /// <param name="id">the model's identity</param>
        /// <returns>the model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested model does not exists).</exception>
        public async static Task<Model> GetModel(int id)
        {
            if (id == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Model model = await ctx.Models.FirstOrDefaultAsync(m => m.Id == id);
                if (model == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return model;
            }
        }

        /// <summary>
        /// Remove the given <see cref="ConceptionDevisWS.Models.Model"/> from storage.
        /// </summary>
        /// <param name="id">the model's identity</param>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested model does not exists).</exception>
        public async static Task RemoveModel(int id)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Model seekedModel = await GetModel(id);
                ctx.Entry(seekedModel).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update completely the given <see cref="ConceptionDevisWS.Models.Model"/>.
        /// </summary>
        /// <param name="id">the model's identity</param>
        /// <param name="newModel">the updated model to store</param>
        /// <returns>the updated model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested model does not exists).</exception>
        public async static Task<Model> UpdateModel(int id, Model newModel)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Model seekedModel = await GetModel(id);
                ctx.Entry(seekedModel).State = EntityState.Modified;
                await ServiceHelper<Model>.SetSingleNavigationProperty<Range>(newModel, seekedModel, ctx, m => m.Range, _getCtxRanges, _setRange);
                ctx.Entry(seekedModel).State = EntityState.Modified;

                seekedModel.UpdateNonComposedPropertiesFrom(newModel);
                await ctx.SaveChangesAsync();
                return seekedModel;

            }
        }

        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Model"/>.
        /// </summary>
        /// <param name="newModel">the model to store</param>
        /// <returns>the stored model</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (when the given model is null).</exception>
        public async static Task<Model> CreateNew(Model newModel)
        {
            if (newModel == null || newModel.Name == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                await ServiceHelper<Model>.LoadSingleNavigationProperty<Range>(newModel, ctx, m => m.Range, _getCtxRanges, _setRange);
                ctx.Models.Add(newModel);
                await ctx.SaveChangesAsync();
                return newModel;
            }
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