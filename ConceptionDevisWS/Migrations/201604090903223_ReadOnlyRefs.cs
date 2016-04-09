namespace ConceptionDevisWS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReadOnlyRefs : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Clients", new[] { "Referrence" });
            DropIndex("dbo.Projets", new[] { "Reference" });
            DropIndex("dbo.Composants", new[] { "Referrence" });
            DropIndex("dbo.Modules", new[] { "Reference" });
            DropColumn("dbo.Clients", "Referrence");
            DropColumn("dbo.Projets", "Reference");
            DropColumn("dbo.Composants", "Referrence");
            DropColumn("dbo.Modules", "Reference");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Modules", "Reference", c => c.String(maxLength: 50));
            AddColumn("dbo.Composants", "Referrence", c => c.String(maxLength: 50));
            AddColumn("dbo.Projets", "Reference", c => c.String(maxLength: 50));
            AddColumn("dbo.Clients", "Referrence", c => c.String(maxLength: 50));
            CreateIndex("dbo.Modules", "Reference", unique: true);
            CreateIndex("dbo.Composants", "Referrence", unique: true);
            CreateIndex("dbo.Projets", "Reference", unique: true);
            CreateIndex("dbo.Clients", "Referrence", unique: true);
        }
    }
}
