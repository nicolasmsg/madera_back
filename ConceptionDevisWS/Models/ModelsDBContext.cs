using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ConceptionDevisWS.Models
{
    public class ModelsDBContext : DbContext
    {
        public DbSet<Module> Modules { get; set; }
        
        public DbSet<Component> Components { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<User> Users { get; set; }

        public ModelsDBContext() 
            :base("db")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Module>()
               .HasMany<Component>(m => m.Components)
               .WithMany(c => c.Modules)
               .Map(mBuilder => {
                   mBuilder.MapLeftKey("Module_Id");
                   mBuilder.MapRightKey("Composant_Id");
                   mBuilder.ToTable("ModuleComposants");
               });
        }
    }
}