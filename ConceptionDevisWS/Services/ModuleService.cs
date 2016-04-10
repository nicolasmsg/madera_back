using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services.Utils;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Services
{
    public static class ModuleService
    {
        public async static Task<IEnumerable<Module>> GetAllModules()
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Modules.Include( m => m.Components ).ToListAsync();
            }
        }

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

        public async static Task RemoveModule(int id)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Module module = await GetModule(id);
                ctx.Entry(module).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
        }

        public async static Task<Module> CreateNew(Module newModule)
        {
            if(newModule.Name == null)
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
        /// only components associations can be updated : if we try to update an associated component this change will get ignored as reflected by the response
        /// in pure REST we should only return api/components/{id} links with components ids
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newModule"></param>
        /// <returns></returns>
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