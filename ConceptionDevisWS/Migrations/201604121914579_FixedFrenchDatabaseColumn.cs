namespace ConceptionDevisWS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FixedFrenchDatabaseColumn : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Clients", name: "FirstName", newName: "Prenom");
            RenameColumn(table: "dbo.Clients", name: "LastName", newName: "Nom");
            RenameColumn(table: "dbo.Clients", name: "Address", newName: "Adresse");
            RenameColumn(table: "dbo.Clients", name: "City", newName: "Ville");
            RenameColumn(table: "dbo.Clients", name: "ZipCode", newName: "CodePostal");
            RenameColumn(table: "dbo.Clients", name: "Phone", newName: "Telephone");
            RenameColumn(table: "dbo.Clients", name: "Birdthdate", newName: "DateDeNaissance");
            RenameColumn(table: "dbo.Utilisateurs", name: "Login", newName: "Nom");
            RenameColumn(table: "dbo.Utilisateurs", name: "Password", newName: "MotDePasse");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Utilisateurs", name: "MotDePasse", newName: "Password");
            RenameColumn(table: "dbo.Utilisateurs", name: "Nom", newName: "Login");
            RenameColumn(table: "dbo.Clients", name: "DateDeNaissance", newName: "Birdthdate");
            RenameColumn(table: "dbo.Clients", name: "Telephone", newName: "Phone");
            RenameColumn(table: "dbo.Clients", name: "CodePostal", newName: "ZipCode");
            RenameColumn(table: "dbo.Clients", name: "Ville", newName: "City");
            RenameColumn(table: "dbo.Clients", name: "Adresse", newName: "Address");
            RenameColumn(table: "dbo.Clients", name: "Nom", newName: "LastName");
            RenameColumn(table: "dbo.Clients", name: "Prenom", newName: "FirstName");
        }
    }
}
