using System.Collections.Generic;
using System.Threading.Tasks;
using ConceptionDevisWS.Models;
using System.Data.Entity;
using System.Net;
using System.Web.Http;

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
                ctx.Modules.Add(newModule);
                await ctx.SaveChangesAsync();
                return newModule;
            }
        }

        public async static Task<Module> UpdateModule(int id, Module newModule)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Module module = await GetModule(id);
                newModule.Id = id;
                ctx.Entry(module).State = EntityState.Detached;
                ctx.Entry(newModule).State = EntityState.Modified;
                int affectedRows = await ctx.SaveChangesAsync();
                return newModule;
            }
        }
    }
}