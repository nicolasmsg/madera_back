namespace ConceptionDevisWS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class MadeModelsRangeMandatory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Modele", "Range_Id", "dbo.Gamme");
            DropIndex("dbo.Modele", new[] { "Range_Id" });
            AlterColumn("dbo.Modele", "Range_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Modele", "Range_Id");
            AddForeignKey("dbo.Modele", "Range_Id", "dbo.Gamme", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modele", "Range_Id", "dbo.Gamme");
            DropIndex("dbo.Modele", new[] { "Range_Id" });
            AlterColumn("dbo.Modele", "Range_Id", c => c.Int());
            CreateIndex("dbo.Modele", "Range_Id");
            AddForeignKey("dbo.Modele", "Range_Id", "dbo.Gamme", "Id");
        }
    }
}
