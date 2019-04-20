namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class relationship_farm_farmer3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Farms", "Farmer_FarmerId", "dbo.Farmers");
            DropIndex("dbo.Farms", new[] { "Farmer_FarmerId" });
            AddColumn("dbo.Farmers", "Farms_FarmId", c => c.Int());
            CreateIndex("dbo.Farmers", "Farms_FarmId");
            AddForeignKey("dbo.Farmers", "Farms_FarmId", "dbo.Farms", "FarmId");
            DropColumn("dbo.Farms", "Farmer_FarmerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Farms", "Farmer_FarmerId", c => c.Int());
            DropForeignKey("dbo.Farmers", "Farms_FarmId", "dbo.Farms");
            DropIndex("dbo.Farmers", new[] { "Farms_FarmId" });
            DropColumn("dbo.Farmers", "Farms_FarmId");
            CreateIndex("dbo.Farms", "Farmer_FarmerId");
            AddForeignKey("dbo.Farms", "Farmer_FarmerId", "dbo.Farmers", "FarmerId");
        }
    }
}
