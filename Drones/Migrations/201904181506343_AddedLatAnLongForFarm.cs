namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLatAnLongForFarm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Farms", "Latitude", c => c.String());
            AddColumn("dbo.Farms", "Longitude", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Farms", "Longitude");
            DropColumn("dbo.Farms", "Latitude");
        }
    }
}
