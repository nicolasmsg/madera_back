namespace ConceptionDevisWS.Migrations
{
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ConceptionDevisWS.Models.ModelsDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        

        protected override void Seed(ConceptionDevisWS.Models.ModelsDBContext context)
        {
            Component sectionDroite = new Component { Id = 1, Name = "Section droite", Length = 4.0, AngleStr = "Droit" };
            Component sectionFinale = new Component { Id = 2, Name = "Section finale", Length = 4.0, AngleStr = "Droit" };
            Component sectionPaloise = new Component { Id = 3, Name = "Section paloise", Length = 4.0, AngleStr = "Droit"  };
            Module murN1 = new Module { Id = 1, Name = "Mur Nord 1", Components = new List<Component> { sectionDroite, sectionFinale } };
            Module murS1 = new Module { Id = 2, Name = "Mur Sud 1", Components = new List<Component> { sectionFinale, sectionPaloise } };

            context.Modules.AddRange(new List<Module> { murN1, murS1 });
        }

    }
}
