using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Controllers
{
    public class ModuleController : ApiController
    {
        [Authorize]
        [Route("api/modules")]
        public async Task<IEnumerable<Module>> GetAllModules()
        {
            return await ModuleService.GetAllModules();
        }

        [Authorize]
        [Route("api/modules/{id}")]
        public async Task<Module> GetModule(int id)
        {
            return await ModuleService.GetModule(id);
        }

        [Authorize]
        [Route("api/modules/{id}")]
        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> DeleteModule(int id)
        {
            await ModuleService.RemoveModule(id);
            return Ok();
        }

        [Authorize]
        [Route("api/modules/{id}")]
        [AcceptVerbs("PUT")]
        public async Task<Module> PutModule(int id, Module newModule)
        {
            return await ModuleService.UpdateModule(id, newModule);
        }

        [Authorize]
        [Route("api/modules")]
        [AcceptVerbs("POST")]
        public async Task<Module> PostModule(Module newModule)
        {
            return await ModuleService.CreateNew(newModule);
        }
    }
}
