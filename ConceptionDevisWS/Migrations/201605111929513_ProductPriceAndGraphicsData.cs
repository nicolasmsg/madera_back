namespace ConceptionDevisWS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductPriceAndGraphicsData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "DonneesGraphiques", c => c.String(maxLength: 4000));
            AddColumn("dbo.Products", "Prix", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Prix");
            DropColumn("dbo.Products", "DonneesGraphiques");
        }
    }
}
