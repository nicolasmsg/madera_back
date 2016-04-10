namespace ConceptionDevisWS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ManyToManyRel_Module_Composant : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Composants", "Module_Id", "dbo.Modules");
            DropIndex("dbo.Composants", new[] { "Module_Id" });
            CreateTable(
                "dbo.ModuleComposants",
                c => new
                    {
                        Module_Id = c.Int(nullable: false),
                        Composant_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Module_Id, t.Composant_Id })
                .ForeignKey("dbo.Modules", t => t.Module_Id, cascadeDelete: true)
                .ForeignKey("dbo.Composants", t => t.Composant_Id, cascadeDelete: true)
                .Index(t => t.Module_Id)
                .Index(t => t.Composant_Id);
            
            DropColumn("dbo.Composants", "Module_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Composants", "Module_Id", c => c.Int());
            DropForeignKey("dbo.ModuleComposants", "Composant_Id", "dbo.Composants");
            DropForeignKey("dbo.ModuleComposants", "Module_Id", "dbo.Modules");
            DropIndex("dbo.ModuleComposants", new[] { "Composant_Id" });
            DropIndex("dbo.ModuleComposants", new[] { "Module_Id" });
            DropTable("dbo.ModuleComposants");
            CreateIndex("dbo.Composants", "Module_Id");
            AddForeignKey("dbo.Composants", "Module_Id", "dbo.Modules", "Id");
        }
    }
}
