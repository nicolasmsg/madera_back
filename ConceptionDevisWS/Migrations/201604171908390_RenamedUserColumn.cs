namespace ConceptionDevisWS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RenamedUserColumn : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Clients", new[] { "user_Id" });
            CreateIndex("dbo.Clients", "User_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Clients", new[] { "User_Id" });
            CreateIndex("dbo.Clients", "user_Id");
        }
    }
}
