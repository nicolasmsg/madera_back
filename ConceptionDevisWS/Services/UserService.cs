using ConceptionDevisWS.Models;
using ConceptionDevisWS.Models.Auth;
using ConceptionDevisWS.Models.Converters;
using ConceptionDevisWS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Services
{
    /// <summary>
    /// 
    /// </summary>
    public static class UserService
    {
        
        /// <summary>
        /// Retrieve a <see cref="ConceptionDevisWS.Models.User"/>.
        /// </summary>
        /// <param name="id">the user's identity</param>
        /// <param name="lang">the culture to get the user into (fr-FR or en-US)</param>
        /// <returns>the given user</returns>
        /// <exception cref="HttpResponseException">In case something went wront (for example the requested user doesn't exist).</exception>
        public async static Task<User> GetUser(int id, string lang)

        {
            if(id == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            CulturalEnumStringConverter.Culture = new CultureInfo(lang);
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

        /// <summary>
        /// Retrieve all existing <see cref="ConceptionDevisWS.Models.User"/>s.
        /// </summary>
        /// <param name="lang">the culture to get users into (fr-FR or en-US)</param>
        /// <returns>a list of users</returns>
        public async static Task<IEnumerable<User>> GetAllUsers(string lang)
        {
            CulturalEnumStringConverter.Culture = new CultureInfo(lang);
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Users.Include(u => u.Clients).ToListAsync<User>();
            }
        }

        /// <summary>
        /// Create an <see cref="ConceptionDevisWS.Models.User"/>.
        /// </summary>
        /// <param name="user">the user to store</param>
        /// <returns>the created user</returns>
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

        /// <summary>
        /// Create a Login resource.
        /// </summary>
        /// <param name="user">the user to log in</param>
        /// <param name="baseAddress">the host to log into</param>
        /// <param name="lang">the culture to retrieve login info into (fr-FR or en-US)</param>
        /// <returns>a login object with credential headers and a User with its Clients</returns>
        public async static Task<object> Login(User user, string baseAddress, string lang)
        {
            if(user == null || user.Password == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            CulturalEnumStringConverter.Culture = new CultureInfo(lang);

            string hashedPassword = HashManager.GetHash(user.Password);
            user.Password = hashedPassword;
            User seekedUser = await _findUser(user.Login);

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
                user = seekedUser,
                ranges = await RangeService.GetAllRanges(lang)
            };
        }

        /// <summary>
        /// Indicate wether the user is logged out or not
        /// </summary>
        /// <param name="userPrincipal">the user's security identity</param>
        /// <returns>true if the user is logger out, false otherwise</returns>
        public async static Task<bool> HasLoggedOut(IPrincipal userPrincipal)
        {
            return (await _findToken(userPrincipal.Identity.Name)) != null;
        }

        public async static Task<User> GetUser(IPrincipal userPrincipal)
        {
            return await _findUser(userPrincipal.Identity.Name);
        }

        /// <summary>
        /// Logs a <see cref="ConceptionDevisWS.Models.User"/> out.
        /// </summary>
        /// <param name="principal">the user's security identity</param>
        /// <remarks>
        /// This is not fully \htmlonly <accronym title="REpresentational State Transfer">REST</accronym>\endhtmlonly compliant, but it's usual.
        /// </remarks>
        public async static Task Logout(IPrincipal principal)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                RevokedToken revokedToken = new RevokedToken { Name = principal.Identity.Name };
                ctx.RevokedTokens.Add(revokedToken);
                await ctx.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update completely an <see cref="ConceptionDevisWS.Models.User"/>.
        /// </summary>
        /// <param name="id">the user's identity</param>
        /// <param name="newUser">the updated user</param>
        /// <param name="lang">the culture to update this user into (fr-FR or en-US)</param>
        /// <returns>the updated user</returns>
        /// <exception cref="HttpResponseException">In case something went wront (for example the requested user doesn't exist).</exception>
        public async static Task<User> UpdateUser(int id, User newUser, string lang)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                User seekedUser = await GetUser(id, lang);
                ctx.Entry(seekedUser).State = EntityState.Modified;
                ctx.Entry(seekedUser).Collection(u => u.Clients).EntityEntry.State = EntityState.Modified;

                await ServiceHelper<User>.UpdateNavigationProperty<Client>(newUser, seekedUser, ctx, _getClients, _getCtxClients);
                seekedUser.UpdateNonComposedPropertiesFrom(newUser);
                bool updateSuccess = false;
                do
                {
                    try
                    {
                        await ctx.SaveChangesAsync();
                        updateSuccess = true;
                    }
                    catch (DbUpdateConcurrencyException dbuce)
                    {
                        DbEntityEntry entry = dbuce.Entries.Single();
                        entry.OriginalValues.SetValues(await entry.GetDatabaseValuesAsync());
                    }
                } while (!updateSuccess);
                return seekedUser;
            }
        }

        /// <summary>
        /// Remove an <see cref="ConceptionDevisWS.Models.User"/>.
        /// </summary>
        /// <param name="id">the user's identity</param>
        /// <exception cref="HttpResponseException">In case something went wront (for example the requested user doesn't exist).</exception>
        public async static Task RemoveUser(int id)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                User seekedUser = await GetUser(id, "fr-FR");
                ctx.Entry(seekedUser).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
        }

        private async static Task<User> _findUser(string login)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Users.Include(u => u.Clients.Select(c => c.Projects) )
                    .FirstOrDefaultAsync(u => u.Login.Equals(login, StringComparison.InvariantCultureIgnoreCase));
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