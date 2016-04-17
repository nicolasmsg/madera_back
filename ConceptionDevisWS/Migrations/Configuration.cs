namespace ConceptionDevisWS.Migrations
{
    using Models;
    using Services.Utils;
    using System;
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


            context.Users.AddOrUpdate(new User[] {
                new User { Id=1, Login="tutu", Password=HashManager.GetHash("ah$34!"), Rights=ERights.ConceptionDevis }
            });

            Client client = new Client {
                Id = 1,
                FirstName = "Test",
                LastName = "Tartampion",
                Address = "10 rue Lagrange",
                City = "Pau",
                ZipCode = 64000,
                Email = "test.tartampion@laposte.net",
                Birdthdate = new DateTime(1984, 11, 16, 0, 0, 0, DateTimeKind.Utc)
            };

            Project firstProj = new Project
            {
                Id = 1,
                Name = "SuperProjet1",
                CreationDate = DateTime.UtcNow,
                State = EProjectState.Signed,
                Client = client,
                TechnicalSheetPath = @"/techSheets/techSheet_1.pdf"
            };

            context.Clients.AddOrUpdate(new Client[] { client });
            context.Projects.AddOrUpdate(new Project[] {
                firstProj
            });

        }

    }
}
