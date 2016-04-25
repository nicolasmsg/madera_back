namespace ConceptionDevisWS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRangeAndModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Modele",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 50),
                        Remplissage = c.Int(nullable: false),
                        FinitionsExterieures = c.Int(nullable: false),
                        FinitionsInterieures = c.Int(nullable: false),
                        QualiteHuisserie = c.Int(nullable: false),
                        CheminImage = c.String(),
                        PourcentagePrixBase = c.Double(nullable: false),
                        Range_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gamme", t => t.Range_Id)
                .Index(t => t.Range_Id);
            
            CreateTable(
                "dbo.Gamme",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 50),
                        TypeIsolant = c.Int(nullable: false),
                        ConceptionOssature = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modele", "Range_Id", "dbo.Gamme");
            DropIndex("dbo.Modele", new[] { "Range_Id" });
            DropTable("dbo.Gamme");
            DropTable("dbo.Modele");
        }
    }
}
