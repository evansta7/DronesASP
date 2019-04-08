namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixedLinkingTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Farms", "UAV_DroneId", "dbo.UAVs");
            DropForeignKey("dbo.UAVs", "Crop_CropId", "dbo.Crops");
            DropIndex("dbo.Farms", new[] { "UAV_DroneId" });
            DropIndex("dbo.UAVs", new[] { "Crop_CropId" });
            CreateTable(
                "dbo.UAVFarms",
                c => new
                    {
                        UAV_DroneId = c.Int(nullable: false),
                        Farm_FarmId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UAV_DroneId, t.Farm_FarmId })
                .ForeignKey("dbo.UAVs", t => t.UAV_DroneId, cascadeDelete: true)
                .ForeignKey("dbo.Farms", t => t.Farm_FarmId, cascadeDelete: true)
                .Index(t => t.UAV_DroneId)
                .Index(t => t.Farm_FarmId);
            
            DropColumn("dbo.Farms", "UAV_DroneId");
            DropColumn("dbo.UAVs", "Crop_CropId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UAVs", "Crop_CropId", c => c.Int());
            AddColumn("dbo.Farms", "UAV_DroneId", c => c.Int());
            DropForeignKey("dbo.UAVFarms", "Farm_FarmId", "dbo.Farms");
            DropForeignKey("dbo.UAVFarms", "UAV_DroneId", "dbo.UAVs");
            DropIndex("dbo.UAVFarms", new[] { "Farm_FarmId" });
            DropIndex("dbo.UAVFarms", new[] { "UAV_DroneId" });
            DropTable("dbo.UAVFarms");
            CreateIndex("dbo.UAVs", "Crop_CropId");
            CreateIndex("dbo.Farms", "UAV_DroneId");
            AddForeignKey("dbo.UAVs", "Crop_CropId", "dbo.Crops", "CropId");
            AddForeignKey("dbo.Farms", "UAV_DroneId", "dbo.UAVs", "DroneId");
        }
    }
}
