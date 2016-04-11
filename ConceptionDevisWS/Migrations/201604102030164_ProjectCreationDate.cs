namespace ConceptionDevisWS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectCreationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projets", "DateCreation", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projets", "DateCreation");
        }
    }
}
