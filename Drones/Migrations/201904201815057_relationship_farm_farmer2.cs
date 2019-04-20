namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class relationship_farm_farmer2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Farmers", "Farm_FarmId", "dbo.Farms");
            DropIndex("dbo.Farmers", new[] { "Farm_FarmId" });
            AddColumn("dbo.Farms", "Farmer_FarmerId", c => c.Int());
            CreateIndex("dbo.Farms", "Farmer_FarmerId");
            AddForeignKey("dbo.Farms", "Farmer_FarmerId", "dbo.Farmers", "FarmerId");
            DropColumn("dbo.Farmers", "Farm_FarmId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Farmers", "Farm_FarmId", c => c.Int());
            DropForeignKey("dbo.Farms", "Farmer_FarmerId", "dbo.Farmers");
            DropIndex("dbo.Farms", new[] { "Farmer_FarmerId" });
            DropColumn("dbo.Farms", "Farmer_FarmerId");
            CreateIndex("dbo.Farmers", "Farm_FarmId");
            AddForeignKey("dbo.Farmers", "Farm_FarmId", "dbo.Farms", "FarmId");
        }
    }
}
