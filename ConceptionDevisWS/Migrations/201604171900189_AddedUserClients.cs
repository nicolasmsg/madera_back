namespace ConceptionDevisWS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserClients : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "user_Id", c => c.Int());
            CreateIndex("dbo.Clients", "user_Id");
            AddForeignKey("dbo.Clients", "user_Id", "dbo.Utilisateurs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "user_Id", "dbo.Utilisateurs");
            DropIndex("dbo.Clients", new[] { "user_Id" });
            DropColumn("dbo.Clients", "user_Id");
        }
    }
}
