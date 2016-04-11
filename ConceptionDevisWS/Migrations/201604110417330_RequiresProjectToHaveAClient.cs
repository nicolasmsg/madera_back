namespace ConceptionDevisWS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiresProjectToHaveAClient : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projets", "Client_Id", "dbo.Clients");
            DropIndex("dbo.Projets", new[] { "Client_Id" });
            AlterColumn("dbo.Projets", "Client_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Projets", "Client_Id");
            AddForeignKey("dbo.Projets", "Client_Id", "dbo.Clients", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projets", "Client_Id", "dbo.Clients");
            DropIndex("dbo.Projets", new[] { "Client_Id" });
            AlterColumn("dbo.Projets", "Client_Id", c => c.Int());
            CreateIndex("dbo.Projets", "Client_Id");
            AddForeignKey("dbo.Projets", "Client_Id", "dbo.Clients", "Id");
        }
    }
}
