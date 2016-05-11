//! EntityFramework files to setup the database.
namespace ConceptionDevisWS.Migrations
{
    using Models;
    using Services.Utils;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Class used to initialize fixture (test) data.
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<ConceptionDevisWS.Models.ModelsDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        
        /// <summary>
        /// Fill the database with test data.
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(ConceptionDevisWS.Models.ModelsDBContext context)
        {
            Component sectionDroite = new Component { Id = 1, Name = "Section droite", Length = 4.0, Angle = EAngle.Straight };
            Component sectionFinale = new Component { Id = 2, Name = "Section finale", Length = 4.0, Angle = EAngle.Straight };
            Component sectionPaloise = new Component { Id = 3, Name = "Section paloise", Length = 4.0, Angle = EAngle.Straight  };
            Module murN1 = new Module { Id = 1, Name = "Mur Nord 1", Components = new List<Component> { sectionDroite, sectionFinale } };
            Module murS1 = new Module { Id = 2, Name = "Mur Sud 1", Components = new List<Component> { sectionFinale, sectionPaloise } };

            context.Modules.AddRange(new List<Module> { murN1, murS1 });

            User tutuUser = new User
            {
                Id = 1,
                Login = "tutu",
                Password = HashManager.GetHash("ah$34!"),
                Rights = ERights.ConceptionDevis
            };



            context.Users.AddOrUpdate(new User[] { tutuUser });

            

            Client client = new Client {
                Id = 2,
                FirstName = "Test",
                LastName = "Tartampion",
                Address = "10 rue Lagrange",
                City = "Pau",
                ZipCode = 64000,
                Email = "test.tartampion@laposte.net",
                Birdthdate = new DateTime(1984, 11, 16, 0, 0, 0, DateTimeKind.Utc),
                Phone = "0200094524",
                User = tutuUser
            };

            Client client2 = new Client
            {
                Id = 3,
                FirstName = "Test2",
                LastName = "LaTruffe",
                Address = "12 rue Lagrange",
                City = "Pau",
                ZipCode = 64000,
                Email = "test2.latruffe@gmail.com",
                Birdthdate = new DateTime(1984, 11, 14, 0, 0, 0, DateTimeKind.Utc),
                Phone = "0100094524",
                User = tutuUser
            };

            Client maderaClient = new Client
            {
                Id = 1,
                FirstName = "Madera",
                LastName = "",
                Birdthdate = DateTime.ParseExact("16/02/1988", "dd/MM/yyyy", null),
                ZipCode = -1
            };

            context.Clients.AddOrUpdate(new Client[] { maderaClient, client, client2 });

            
            

            Project firstProj = new Project
            {
                Id = 1,
                Name = "SuperProjet1",
                CreationDate = DateTime.UtcNow,
                State = EProjectState.Signed,
                Client = client,
                TechnicalSheetPath = @"/techSheets/techSheet_1.pdf"
            };

            
            context.Projects.AddOrUpdate(new Project[] {
                firstProj
            });

            Range nature = new Range
            {
                Id = 1,
                Name = "Nature",
                ExtFinishings = EExtFinishing.Wood,
                FrameQualities = EFrameQuality.Wood,
                Insulators = EInsulatorKind.RockWool,
                FrameStructure = EFrameStructure.Angleless
            };

            Range bois = new Range
            {
                Id = 2,
                Name = "Bois",
                ExtFinishings = EExtFinishing.Wood | EExtFinishing.Roughcast,
                FrameQualities = EFrameQuality.Wood | EFrameQuality.PVC,
                Insulators = EInsulatorKind.Styrofoam,
                FrameStructure = EFrameStructure.OpenAngle
            };

            Range couleur = new Range
            {
                Id = 3,
                Name = "Couleurs",
                ExtFinishings = EExtFinishing.Roughcast | EExtFinishing.Paint,
                FrameQualities = EFrameQuality.Wood | EFrameQuality.PVC,
                Insulators = EInsulatorKind.GlassWool,
                FrameStructure = EFrameStructure.ClosedAngle
            };

            context.Ranges.AddOrUpdate(nature, bois, couleur);

            Model maison3ChSdb = new Model
            {
                Name = "Maison 3 Chambres",
                BasePricePercentage = 15.00,
                Filling = EFillingKind.NaturalWool,
                IntFinishing = EIntFinishing.Plasterboard,
                ExtFinishing = EExtFinishing.Roughcast,
                FrameQuality = EFrameQuality.Wood,
                Range = bois
            };

            Model villaAvecTerrasse = new Model
            {
                Name = "Maison a étage",
                BasePricePercentage = 25.00,
                Filling = EFillingKind.NaturalWool,
                IntFinishing = EIntFinishing.Plasterboard,
                ExtFinishing = EExtFinishing.Roughcast,
                FrameQuality = EFrameQuality.PVC,
                Range = bois
            };

            Model maison2ChJardin = new Model
            {
                Name = "Maison 2 Chambres avec jardin",
                BasePricePercentage = 18.00,
                Filling = EFillingKind.NaturalWool,
                IntFinishing = EIntFinishing.Styrofoam,
                ExtFinishing = EExtFinishing.Wood,
                FrameQuality = EFrameQuality.Wood,
                Range = bois
            };

            Model chalet2Ch = new Model
            {
                Name = "Chalet 2 Chambres",
                BasePricePercentage = 35.00,
                Filling = EFillingKind.NaturalWool,
                IntFinishing = EIntFinishing.Plasterboard,
                ExtFinishing = EExtFinishing.Wood,
                FrameQuality = EFrameQuality.Wood,
                Range = nature
            };

            Model abrisMontagnard = new Model
            {
                Name = "Abris Montagnard",
                BasePricePercentage = 28.00,
                Filling = EFillingKind.Hemp,
                IntFinishing = EIntFinishing.Styrofoam,
                ExtFinishing = EExtFinishing.Wood,
                FrameQuality = EFrameQuality.Wood,
                Range = nature
            };

            Model villaAvecPiscine = new Model
            {
                Name = "Villa avec piscine",
                BasePricePercentage = 40.00,
                Filling = EFillingKind.NaturalWool,
                IntFinishing = EIntFinishing.Wood,
                ExtFinishing = EExtFinishing.Wood,
                FrameQuality = EFrameQuality.Wood,
                Range = nature
            };

            Model creche = new Model
            {
                Name = "Creche",
                BasePricePercentage = 8.00,
                Filling = EFillingKind.WoodenWool,
                IntFinishing = EIntFinishing.Plasterboard,
                ExtFinishing = EExtFinishing.Roughcast,
                FrameQuality = EFrameQuality.PVC,
                Range = couleur
            };

            Model localProCrea = new Model
            {
                Name = "Local Professionnel (Créatif)",
                BasePricePercentage = 12.00,
                Filling = EFillingKind.WoodenWool,
                IntFinishing = EIntFinishing.Wood,
                ExtFinishing = EExtFinishing.Paint,
                FrameQuality = EFrameQuality.PVC,
                Range = couleur
            };

            Model localProDesign = new Model
            {
                Name = "Local Professionnel (Design)",
                BasePricePercentage = 15.00,
                Filling = EFillingKind.Hemp,
                IntFinishing = EIntFinishing.Wood,
                ExtFinishing = EExtFinishing.Roughcast,
                FrameQuality = EFrameQuality.PVC,
                Range = couleur
            };

            context.Models.AddOrUpdate(maison3ChSdb, villaAvecTerrasse, maison2ChJardin, chalet2Ch, abrisMontagnard, villaAvecPiscine, creche, localProCrea, localProDesign);

            Module porteRenforcee = new Module { Id=1, Name="Porte Renforcee", Price=4, Components = new List<Component>{ sectionDroite } };
            Module fenetreRonde = new Module { Id =2,  Name = "Fenêtre Ronde", Price = 4, Components = new List<Component> { sectionDroite } };
            Module fenetreOuverte = new Module { Id =3, Name = "Fenêtre Ouverte", Price = 4, Components = new List<Component> { sectionDroite } };
            Module palissade = new Module { Id = 4, Name = "Palissade", Price = 4, Components = new List<Component> { sectionDroite } };
            Module decorationInt = new Module { Id =5, Name = "Decoration Int 1", Price = 4, Components = new List<Component> { sectionDroite } };
            Module archeVoute = new Module { Id =6, Name = "Arche Voute", Price = 4, Components = new List<Component> { sectionDroite } };

            context.Modules.AddRange(new List<Module> { porteRenforcee, fenetreRonde, fenetreOuverte, palissade, decorationInt, archeVoute } );

            maison3ChSdb.Modules = new List<Module>();
            maison2ChJardin.Modules = new List<Module>();
            villaAvecTerrasse.Modules = new List<Module>();

            maison3ChSdb.Modules.AddRange(new Module[] { fenetreRonde, fenetreOuverte, palissade });
            maison2ChJardin.Modules.AddRange(new Module[] { decorationInt, archeVoute });
            villaAvecTerrasse.Modules.AddRange(new Module[] {  porteRenforcee });

            
            
        }

    }
}
