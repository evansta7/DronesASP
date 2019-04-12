namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Crops",
                c => new
                    {
                        CropId = c.Int(nullable: false, identity: true),
                        CropDescription = c.String(),
                        CropName = c.String(),
                        IdealClimateLowerRange = c.Int(nullable: false),
                        IdealClimateUpperRange = c.Int(nullable: false),
                        IdealSoil = c.String(),
                        MostCommonPest = c.String(),
                        SoilDescription = c.String(),
                    })
                .PrimaryKey(t => t.CropId);
            
            CreateTable(
                "dbo.Farms",
                c => new
                    {
                        FarmId = c.Int(nullable: false, identity: true),
                        FarmSize = c.Int(nullable: false),
                        PostalCode = c.String(),
                        StreetAddress = c.String(),
                        Suburb = c.String(),
                    })
                .PrimaryKey(t => t.FarmId);
            
            CreateTable(
                "dbo.UAVs",
                c => new
                    {
                        DroneId = c.Int(nullable: false, identity: true),
                        DroneStatus = c.String(),
                        DroneType = c.String(),
                    })
                .PrimaryKey(t => t.DroneId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UAVFarms", "Farm_FarmId", "dbo.Farms");
            DropForeignKey("dbo.UAVFarms", "UAV_DroneId", "dbo.UAVs");
            DropForeignKey("dbo.FarmCrops", "Crop_CropId", "dbo.Crops");
            DropForeignKey("dbo.FarmCrops", "Farm_FarmId", "dbo.Farms");
            DropIndex("dbo.UAVFarms", new[] { "Farm_FarmId" });
            DropIndex("dbo.UAVFarms", new[] { "UAV_DroneId" });
            DropIndex("dbo.FarmCrops", new[] { "Crop_CropId" });
            DropIndex("dbo.FarmCrops", new[] { "Farm_FarmId" });
            DropTable("dbo.UAVFarms");
            DropTable("dbo.FarmCrops");
            DropTable("dbo.Farmers");
            DropTable("dbo.UAVs");
            DropTable("dbo.Farms");
            DropTable("dbo.Crops");
        }
    }
}
