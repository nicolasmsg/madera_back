namespace ConceptionDevisWS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPricesAndAddedModulesToModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Composants", "Prix", c => c.Double(nullable: false));
            AddColumn("dbo.Modules", "Prix", c => c.Double(nullable: false));
            AddColumn("dbo.Modules", "Model_Id", c => c.Int());
            CreateIndex("dbo.Modules", "Model_Id");
            AddForeignKey("dbo.Modules", "Model_Id", "dbo.Modele", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modules", "Model_Id", "dbo.Modele");
            DropIndex("dbo.Modules", new[] { "Model_Id" });
            DropColumn("dbo.Modules", "Model_Id");
            DropColumn("dbo.Modules", "Prix");
            DropColumn("dbo.Composants", "Prix");
        }
    }
}
