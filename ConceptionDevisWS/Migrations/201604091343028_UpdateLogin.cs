namespace ConceptionDevisWS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateLogin : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Utilisateurs");
            CreateTable(
                "dbo.Utilisateurs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 20),
                        Droits = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Utilisateurs");
            CreateTable(
                "dbo.Utilisateurs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        MotDePasse = c.String(),
                        Droits = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            
        }
    }
}
