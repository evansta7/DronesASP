namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class relationship_farm_farmer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FarmerFarms", "Farmer_FarmerId", "dbo.Farmers");
            DropForeignKey("dbo.FarmerFarms", "Farm_FarmId", "dbo.Farms");
            DropIndex("dbo.FarmerFarms", new[] { "Farmer_FarmerId" });
            DropIndex("dbo.FarmerFarms", new[] { "Farm_FarmId" });
            AddColumn("dbo.Farmers", "Farm_FarmId", c => c.Int());
            CreateIndex("dbo.Farmers", "Farm_FarmId");
            AddForeignKey("dbo.Farmers", "Farm_FarmId", "dbo.Farms", "FarmId");
            DropTable("dbo.FarmerFarms");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FarmerFarms",
                c => new
                    {
                        Farmer_FarmerId = c.Int(nullable: false),
                        Farm_FarmId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Farmer_FarmerId, t.Farm_FarmId });
            
            DropForeignKey("dbo.Farmers", "Farm_FarmId", "dbo.Farms");
            DropIndex("dbo.Farmers", new[] { "Farm_FarmId" });
            DropColumn("dbo.Farmers", "Farm_FarmId");
            CreateIndex("dbo.FarmerFarms", "Farm_FarmId");
            CreateIndex("dbo.FarmerFarms", "Farmer_FarmerId");
            AddForeignKey("dbo.FarmerFarms", "Farm_FarmId", "dbo.Farms", "FarmId", cascadeDelete: true);
            AddForeignKey("dbo.FarmerFarms", "Farmer_FarmerId", "dbo.Farmers", "FarmerId", cascadeDelete: true);
        }
    }
}
