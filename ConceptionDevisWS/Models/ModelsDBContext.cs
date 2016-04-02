using System.Data.Entity;

namespace ConceptionDevisWS.Models
{
    public class ModelsDBContext : DbContext
    {
        public DbSet<Module> Modules { get; set; }

        public DbSet<Composant> Components { get; set; }

        public ModelsDBContext() 
            :base("db")
        { }
    }
}