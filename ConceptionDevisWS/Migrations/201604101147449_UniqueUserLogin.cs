namespace ConceptionDevisWS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UniqueUserLogin : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Utilisateurs", "Login", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Utilisateurs", new[] { "Login" });
        }
    }
}
