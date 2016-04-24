namespace ConceptionDevisWS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdjustUserPasswordLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Utilisateurs", "Password", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Utilisateurs", "Password", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
