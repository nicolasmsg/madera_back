namespace ConceptionDevisWS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTechnicalSheetPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projets", "CheminDossierTechnique", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projets", "CheminDossierTechnique");
        }
    }
}
