namespace ConceptionDevisWS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedClient_Project_User : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 30),
                        LastName = c.String(maxLength: 30),
                        Address = c.String(maxLength: 100),
                        City = c.String(maxLength: 30),
                        ZipCode = c.Int(nullable: false),
                        Phone = c.String(maxLength: 10),
                        Email = c.String(maxLength: 50),
                        Birthdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reference = c.String(maxLength: 50),
                        Nom = c.String(maxLength: 30),
                        Etat = c.String(),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.Utilisateurs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        MotDePasse = c.String(),
                        Droits = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projets", "Client_Id", "dbo.Clients");
            DropIndex("dbo.Projets", new[] { "Client_Id" });
            DropTable("dbo.Utilisateurs");
            DropTable("dbo.Projets");
            DropTable("dbo.Clients");
        }
    }
}
