namespace ConceptionDevisWS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddMiniClient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Birdthdate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Clients", "Birthdate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "Birthdate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Clients", "Birdthdate");
        }
    }
}
