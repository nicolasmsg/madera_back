namespace ConceptionDevisWS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class MadeFrameQualityAndExtFinishingsAsFlags : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gamme", "FinitionsExterieures", c => c.Int(nullable: false));
            AddColumn("dbo.Gamme", "QualiteHuisserie", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gamme", "QualiteHuisserie");
            DropColumn("dbo.Gamme", "FinitionsExterieures");
        }
    }
}
