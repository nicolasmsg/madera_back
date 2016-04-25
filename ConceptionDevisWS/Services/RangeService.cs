using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConceptionDevisWS.Models;
using System.Data.Entity;
using System.Web.Http;
using System.Net;
using ConceptionDevisWS.Services.Utils;

namespace ConceptionDevisWS.Services
{
    /// <summary>
    /// A Service to manage a <see cref="ConceptionDevisWS.Models.Range"/>'s business.
    /// </summary>
    public class RangeService
    {
        /// <summary>
        /// Retrieve all existing <see cref="ConceptionDevisWS.Models.Range"/>s with their <see cref="ConceptionDevisWS.Models.Model"/>s.
        /// </summary>
        /// <returns>a list of ranges</returns>
        public async static Task<IEnumerable<Range>> GetAllRanges()
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Ranges.Include( r => r.Models ).ToListAsync<Range>();
            }
        }

        /// <summary>
        /// Retrieve the given <see cref="ConceptionDevisWS.Models.Range"/>.
        /// </summary>
        /// <param name="id">the range's identity</param>
        /// <returns>the range</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range does not exists).</exception>
        public async static Task<Range> GetRange(int id)
        {
            if (id == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Range range = await ctx.Ranges.Include(r => r.Models).FirstOrDefaultAsync(r => r.Id == id);
                if (range == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return range;
            }
        }

        /// <summary>
        /// Remove the given <see cref="ConceptionDevisWS.Models.Range"/> from storage.
        /// </summary>
        /// <param name="id">the range's identity</param>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range does not exists).</exception>
        public async static Task RemoveRange(int id)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Range seekedRange = await GetRange(id);
                ctx.Entry(seekedRange).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Range"/>.
        /// </summary>
        /// <param name="newRange">the range to store</param>
        /// <returns>the stored range</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (when the given range is null).</exception>
        public async static Task<Range> CreateNew(Range newRange)
        {
            if (newRange == null || newRange.Name == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                await ServiceHelper<Range>.InitNavigationProperty<Model>(newRange, ctx, _getModels, _getCtxModels);
                ctx.Ranges.Add(newRange);
                await ctx.SaveChangesAsync();
                return newRange;
            }
        }

        /// <summary>
        /// Update completely the given <see cref="ConceptionDevisWS.Models.Range"/>.
        /// </summary>
        /// <param name="id">the range's identity</param>
        /// <param name="newRange">the updated range to store</param>
        /// <returns>the updated range</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested range does not exists).</exception>
        public async static Task<Range> UpdateRange(int id, Range newRange)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Range seekedRange = await GetRange(id);
                ctx.Entry(seekedRange).State = EntityState.Modified;
                ctx.Entry(seekedRange).Collection(r => r.Models).EntityEntry.State = EntityState.Modified;

                await ServiceHelper<Range>.UpdateNavigationProperty<Model>(newRange, seekedRange, ctx, _getModels, _getCtxModels);
                seekedRange.UpdateNonComposedPropertiesFrom(newRange);
                await ctx.SaveChangesAsync();
                return seekedRange;

            }
        }


        private static List<Model> _getModels(Range r)
        {
            return r.Models;
        }

        private static DbSet<Model> _getCtxModels(DbContext context)
        {
            return ((ModelsDBContext)context).Models;
        }
    }
}