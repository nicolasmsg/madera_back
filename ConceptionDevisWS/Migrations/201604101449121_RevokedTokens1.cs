namespace ConceptionDevisWS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RevokedTokens1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RevokedTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RevokedTokens", new[] { "Name" });
            DropTable("dbo.RevokedTokens");
        }
    }
}
