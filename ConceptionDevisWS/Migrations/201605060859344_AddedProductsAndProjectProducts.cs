namespace ConceptionDevisWS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProductsAndProjectProducts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 50),
                        CheminPlan = c.String(),
                        Model_Id = c.Int(),
                        Project_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modele", t => t.Model_Id)
                .ForeignKey("dbo.Projets", t => t.Project_Id)
                .Index(t => t.Model_Id)
                .Index(t => t.Project_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Project_Id", "dbo.Projets");
            DropForeignKey("dbo.Products", "Model_Id", "dbo.Modele");
            DropIndex("dbo.Products", new[] { "Project_Id" });
            DropIndex("dbo.Products", new[] { "Model_Id" });
            DropTable("dbo.Products");
        }
    }
}
