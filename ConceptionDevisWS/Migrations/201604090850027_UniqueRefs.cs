namespace ConceptionDevisWS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueRefs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Referrence", c => c.String(maxLength: 50));
            AlterColumn("dbo.Modules", "Reference", c => c.String(maxLength: 50));
            CreateIndex("dbo.Clients", "Referrence", unique: true);
            CreateIndex("dbo.Projets", "Reference", unique: true);
            CreateIndex("dbo.Composants", "Referrence", unique: true);
            CreateIndex("dbo.Modules", "Reference", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Modules", new[] { "Reference" });
            DropIndex("dbo.Composants", new[] { "Referrence" });
            DropIndex("dbo.Projets", new[] { "Reference" });
            DropIndex("dbo.Clients", new[] { "Referrence" });
            AlterColumn("dbo.Modules", "Reference", c => c.String());
            DropColumn("dbo.Clients", "Referrence");
        }
    }
}
