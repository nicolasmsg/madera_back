namespace ConceptionDevisWS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Composants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Referrence = c.String(maxLength: 50),
                        Nom = c.String(maxLength: 50),
                        Longueur = c.Double(nullable: false),
                        Angle = c.String(),
                        Module_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.Module_Id)
                .Index(t => t.Module_Id);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reference = c.String(),
                        Nom = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Composants", "Module_Id", "dbo.Modules");
            DropIndex("dbo.Composants", new[] { "Module_Id" });
            DropTable("dbo.Modules");
            DropTable("dbo.Composants");
        }
    }
}
