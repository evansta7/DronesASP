namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedLinkingTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Farmers",
                c => new
                    {
                        FarmerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                    })
                .PrimaryKey(t => t.FarmerId);
            
            CreateTable(
                "dbo.FarmCrops",
                c => new
                    {
                        Farm_FarmId = c.Int(nullable: false),
                        Crop_CropId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Farm_FarmId, t.Crop_CropId })
                .ForeignKey("dbo.Farms", t => t.Farm_FarmId, cascadeDelete: true)
                .ForeignKey("dbo.Crops", t => t.Crop_CropId, cascadeDelete: true)
                .Index(t => t.Farm_FarmId)
                .Index(t => t.Crop_CropId);
            
            AddColumn("dbo.Farms", "UAV_DroneId", c => c.Int());
            AddColumn("dbo.UAVs", "Crop_CropId", c => c.Int());
            CreateIndex("dbo.Farms", "UAV_DroneId");
            CreateIndex("dbo.UAVs", "Crop_CropId");
            AddForeignKey("dbo.Farms", "UAV_DroneId", "dbo.UAVs", "DroneId");
            AddForeignKey("dbo.UAVs", "Crop_CropId", "dbo.Crops", "CropId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UAVs", "Crop_CropId", "dbo.Crops");
            DropForeignKey("dbo.Farms", "UAV_DroneId", "dbo.UAVs");
            DropForeignKey("dbo.FarmCrops", "Crop_CropId", "dbo.Crops");
            DropForeignKey("dbo.FarmCrops", "Farm_FarmId", "dbo.Farms");
            DropIndex("dbo.FarmCrops", new[] { "Crop_CropId" });
            DropIndex("dbo.FarmCrops", new[] { "Farm_FarmId" });
            DropIndex("dbo.UAVs", new[] { "Crop_CropId" });
            DropIndex("dbo.Farms", new[] { "UAV_DroneId" });
            DropColumn("dbo.UAVs", "Crop_CropId");
            DropColumn("dbo.Farms", "UAV_DroneId");
            DropTable("dbo.FarmCrops");
            DropTable("dbo.Farmers");
        }
    }
}
