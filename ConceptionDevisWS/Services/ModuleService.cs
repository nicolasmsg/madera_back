using System.Collections.Generic;
using System.Threading.Tasks;
using ConceptionDevisWS.Models;
using System.Data.Entity;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using ConceptionDevisWS.Services.Utils;

namespace ConceptionDevisWS.Services
{
    public static class ModuleService
    {
        public async static Task<IEnumerable<Module>> GetAllModules()
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Modules.Include( m => m.Composants ).ToListAsync();
            }
        }

        public async static Task<Module> GetModule(int id)
        {
            using(ModelsDBContext ctx = new ModelsDBContext())
            {
                Module module = await ctx.Modules.Include( m => m.Composants).FirstOrDefaultAsync( m => m.Id == id);
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
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                await ServiceHelper<Module>.EnsuresNoNewElement<Composant>(newModule, getComposants, ctx, getContextComponents);
                ctx.Modules.Add(newModule);
                foreach(Composant comp in newModule.Composants)
                {
                    ctx.Entry(comp).State = EntityState.Unchanged;
                }
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
                Module module = await GetModule(id);
                ctx.Entry(module).State = EntityState.Modified;
                ctx.Entry(module).Collection(mod => mod.Composants).EntityEntry.State = EntityState.Modified;

                await ServiceHelper<Module>.UpdateNavigationProperty<Composant>(newModule, module, ctx, getComposants, getContextComponents);

                module.Nom = newModule.Nom;
                module.Reference = newModule.Reference;

                int affectedRows = await ctx.SaveChangesAsync();
                return module;
            }
        }

        private static List<Composant> getComposants(Module module)
        {
            return module.Composants;
        }

        private static DbSet<Composant> getContextComponents(DbContext context)
        {
            return ((ModelsDBContext)context).Components;
        }

        private static async Task EnsuresNoNewComponents(Module newModule, ModelsDBContext ctx)
        {
            List<Composant> newComps = new List<Composant>();

            foreach (Composant newComp in newModule.Composants)
            {
                if (await ctx.Components.FirstOrDefaultAsync(comp => comp.Id == newComp.Id) == null)
                {
                    newComps.Add(newComp);
                }
            }

            if (newComps.Count > 0)
            {
                string errorMsg = string.Format("Following entities have not been found: {0}", string.Concat(newComps.ConvertAll(c => c.Id)));
                throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound, Content = new StringContent(errorMsg) });
            }
        }

        private static async Task AddAllComponentsFromContext(Module srcModule, Module destModule, ModelsDBContext ctx)
        {
            foreach (Composant newComp in srcModule.Composants)
            {
                Composant trackedNewComp = await ctx.Components.FindAsync(newComp.Id);
                destModule.Composants.Add(trackedNewComp);
            }
        }
    }
}