using ConceptionDevisWS.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using System;
using System.Web.Http;
using System.Net;
using ConceptionDevisWS.Services.Utils;
using System.Security.Principal;
using System.Security.Claims;
using System.Linq;
using ConceptionDevisWS.Models.Auth;
using System.Collections.Generic;

namespace ConceptionDevisWS.Services
{
    public static class UserService
    {
        public async static Task<User> FindUser(string login)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Users.Include(u => u.Clients)
                    .FirstOrDefaultAsync( u => u.Login.Equals(login, StringComparison.InvariantCultureIgnoreCase) );
            }
        }

        public async static Task<User> GetUser(int id)

        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                User seekedUser = await ctx.Users.FirstOrDefaultAsync(u => u.Id == id);
                if(seekedUser == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return seekedUser;
            }
        }

        public async static Task<IEnumerable<User>> GetAllUsers()
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Users.Include(u => u.Clients).ToListAsync<User>();
            }
        }

        public async static Task<User> Register(User user)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                if(user == null || user.Login == null || user.Password == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                ctx.Users.Add(user);
                user.Rights = ERights.ConceptionDevis;
                user.Password = HashManager.GetHash(user.Password);
                await ctx.SaveChangesAsync();
                user.Password = null;
                return user;
            }
        }

        public async static Task<object> Login(User user, string baseAddress)
        {
            if(user == null || user.Password == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            string hashedPassword = HashManager.GetHash(user.Password);
            user.Password = hashedPassword;
            User seekedUser = await FindUser(user.Login);

            if (seekedUser == null || hashedPassword != seekedUser.Password)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            await _removeLogout(seekedUser.Login);

            Random r1 = new Random(159736545);
            Random r2 = new Random(1892344171);
            seekedUser.Password = null;
            return new
            {
                a2un = string.Format("{0}.{1}.{2}", string.Format("{0:X12}", r1.Next(0x5F4A2C3)), string.Format("{0:X18}", r1.Next(0x5FDA6C1)), string.Format("{0:X22}", r1.Next(0x5F1C2C3))),
                az4s = JsonWebTokenManager.CreateToken(user.Login, "user", baseAddress),
                e7gu = string.Format("{0}.{1}.{2}", string.Format("{0:X12}", r2.Next(0x5F4A2C3)), string.Format("{0:X18}", r2.Next(0x5FDA6C1)), string.Format("{0:X22}", r2.Next(0x5F1C2C3))),
                user = seekedUser
            };
        }

        public async static Task<bool> HasLoggedOut(IPrincipal userPrincipal)
        {
            return (await _findToken(userPrincipal.Identity.Name)) != null;
        }

        public async static Task Logout(IPrincipal principal)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                RevokedToken revokedToken = new RevokedToken { Name = principal.Identity.Name };
                ctx.RevokedTokens.Add(revokedToken);
                await ctx.SaveChangesAsync();
            }
        }

        public async static Task<User> UpdateUser(int id, User newUser)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                User seekedUser = await GetUser(id);
                ctx.Entry(seekedUser).State = EntityState.Modified;
                ctx.Entry(seekedUser).Collection(u => u.Clients).EntityEntry.State = EntityState.Modified;

                await ServiceHelper<User>.UpdateNavigationProperty<Client>(newUser, seekedUser, ctx, _getClients, _getCtxClients);
                seekedUser.UpdateNonComposedPropertiesFrom(newUser);
                await ctx.SaveChangesAsync();
                return seekedUser;
            }
        }

        public async static Task RemoveUser(int id)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                User seekedUser = await GetUser(id);
                ctx.Entry(seekedUser).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
        }

        private async static Task<RevokedToken> _findToken(string login)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                RevokedToken revokedToken = await ctx.RevokedTokens.FirstOrDefaultAsync(t => t.Name == login);
                return revokedToken;
            }
        }

        private async static Task _removeLogout(string login)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                RevokedToken seekedToken = await _findToken(login);
                if (seekedToken != null)
                {
                    ctx.Entry(seekedToken).State = EntityState.Deleted;
                    await ctx.SaveChangesAsync();
                }
            }
        }

        private static List<Client> _getClients(User user)
        {
            return user.Clients;
        }

        private static DbSet<Client> _getCtxClients(DbContext context)
        {
            return ((ModelsDBContext)context).Clients;
        }
    }
}