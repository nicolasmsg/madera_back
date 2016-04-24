﻿using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services.Utils;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Services
{
    /// <summary>
    /// A Service to manager <see cref="ConceptionDevisWS.Models.Module"/>'s business.
    /// </summary>
    public static class ModuleService
    {
        /// <summary>
        /// Retrieve all existing <see cref="ConceptionDevisWS.Models.Module"/>s with their <see cref="ConceptionDevisWS.Models.Component"/>s. 
        /// </summary>
        /// <returns>a list of modules</returns>
        public async static Task<IEnumerable<Module>> GetAllModules()
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Modules.Include( m => m.Components ).ToListAsync();
            }
        }

        /// <summary>
        /// Retrieve the given <see cref="ConceptionDevisWS.Models.Module"/>.
        /// </summary>
        /// <param name="id">the module's identity</param>
        /// <returns>the given module</returns>
        /// <exception cref="HttpResponseException">In case something went wront (for example, the given module is not found).</exception>
        public async static Task<Module> GetModule(int id)
        {
            using(ModelsDBContext ctx = new ModelsDBContext())
            {
                Module module = await ctx.Modules.Include( m => m.Components).FirstOrDefaultAsync( m => m.Id == id);
                if (module == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return module;
            }
        }

        /// <summary>
        /// Remove the given module from storage.
        /// </summary>
        /// <param name="id">the module's identity</param>
        /// <exception cref="HttpResponseException">In case something went wront (for example, the given module is not found).</exception>
        public async static Task RemoveModule(int id)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Module module = await GetModule(id);
                ctx.Entry(module).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Module"/>.
        /// </summary>
        /// <param name="newModule">the module to store</param>
        /// <returns>the created module</returns>
        /// <exception cref="HttpResponseException">In case something went wront (when the given module is null).</exception>
        public async static Task<Module> CreateNew(Module newModule)
        {
            if(newModule == null || newModule.Name == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                await ServiceHelper<Module>.InitNavigationProperty<Component>(newModule, ctx, _getComposants, _getContextComponents);
                ctx.Modules.Add(newModule);
                
                await ctx.SaveChangesAsync();
                return newModule;
            }
        }

        /// <summary>
        /// Update a given module
        /// 
        /// only components associations can be updated
        /// </summary>
        /// <param name="id">the module's identity</param>
        /// <param name="newModule">the updated module to store</param>
        /// <returns>the updated module</returns>
        public async static Task<Module> UpdateModule(int id, Module newModule)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Module seekedModule = await GetModule(id);
                ctx.Entry(seekedModule).State = EntityState.Modified;
                ctx.Entry(seekedModule).Collection(mod => mod.Components).EntityEntry.State = EntityState.Modified;

                await ServiceHelper<Module>.UpdateNavigationProperty<Component>(newModule, seekedModule, ctx, _getComposants, _getContextComponents);

                seekedModule.UpdateNonComposedPropertiesFrom(newModule);

                int affectedRows = await ctx.SaveChangesAsync();
                
                return seekedModule;
            }
        }

        private static List<Component> _getComposants(Module module)
        {
            return module.Components;
        }

        private static DbSet<Component> _getContextComponents(DbContext context)
        {
            return ((ModelsDBContext)context).Components;
        }
    }
}