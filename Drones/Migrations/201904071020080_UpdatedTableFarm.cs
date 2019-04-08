namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTableFarm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Farms", "FarmSize", c => c.Int(nullable: false));
            AddColumn("dbo.Farms", "PostalCode", c => c.String());
            AddColumn("dbo.Farms", "StreetAddress", c => c.String());
            AddColumn("dbo.Farms", "Suburb", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Farms", "Suburb");
            DropColumn("dbo.Farms", "StreetAddress");
            DropColumn("dbo.Farms", "PostalCode");
            DropColumn("dbo.Farms", "FarmSize");
        }
    }
}
