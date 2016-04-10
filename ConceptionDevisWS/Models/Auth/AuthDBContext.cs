using System.Data.Entity;

namespace ConceptionDevisWS.Models.Auth
{
    public class AuthDBContext : DbContext
    {
        public DbSet<RevokedToken> RevokedTokens { get; set; }

        public AuthDBContext()
            :base("db") { }
    }
}