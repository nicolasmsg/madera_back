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
            Composant sectionDroite = new Composant { Id = 1, Nom = "Section droite", Longueur = 4.0, AngleStr = "Droit", Referrence = "COMP-1" };
            Composant sectionFinale = new Composant { Id = 2, Nom = "Section finale", Longueur = 4.0, AngleStr = "Droit", Referrence = "COMP-2" };
            Composant sectionPaloise = new Composant { Id = 3, Nom = "Section paloise", Longueur = 4.0, AngleStr = "Droit", Referrence = "COMP-3"  };
            Module murN1 = new Module { Id = 1, Reference = "MOD-1", Nom = "Mur Nord 1", Composants = new List<Composant> { sectionDroite, sectionFinale } };
            Module murS1 = new Module { Id = 2, Reference = "MOD-2", Nom = "Mur Sud 1", Composants = new List<Composant> { sectionFinale, sectionPaloise } };

            context.Modules.AddRange(new List<Module> { murN1, murS1 });
        }
    }
}
