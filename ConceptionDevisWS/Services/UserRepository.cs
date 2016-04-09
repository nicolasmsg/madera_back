using ConceptionDevisWS.Auth;
using ConceptionDevisWS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Services
{
    public class UserRepository : IDisposable
    {
        private static AuthContext _ctx;
        private static UserManager<IdentityUser> _userManager;

        public UserRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
        }

        public  async Task<IdentityResult> Register(User user)
        {
            IdentityUser preIdUser = new IdentityUser
            {
                UserName = user.Login
            };
            IdentityResult idResult = await _userManager.CreateAsync(preIdUser, user.Password);
            if(!idResult.Succeeded)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return idResult;
        }

        public async Task<IdentityUser> FindUser(User user)
        {
            IdentityUser idUser = await _userManager.FindAsync(user.Login, user.Password);

            if (idUser == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            return idUser;
        }

        public void Dispose()
        {

            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}