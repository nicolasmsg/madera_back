using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Controllers
{
    public class ModuleController : ApiController
    {
        [Route("api/modules")]
        public async Task<IEnumerable<Module>> GetAllModules()
        {
            return await ModuleService.GetAllModules();
        }

        [Route("api/modules/{id}")]
        public async Task<Module> GetModule(int id)
        {
            return await ModuleService.GetModule(id);
        }

        [Route("api/modules/{id}")]
        [AcceptVerbs("DELETE")]
        public async Task DeleteModule(int id)
        {
            await ModuleService.RemoveModule(id);
        }

        [Route("api/modules/{id}")]
        [AcceptVerbs("PATCH")]
        public async Task<Module> PatchModule(int id, Module newModule)
        {
            return await ModuleService.UpdateModule(id, newModule);
        }

        [Route("api/modules")]
        [AcceptVerbs("POST")]
        public async Task<Module> PostModule(Module newModule)
        {
            return await ModuleService.CreateNew(newModule);
        }
    }
}
