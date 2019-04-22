namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedCropAndFarmForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FarmCrops", "Farm_FarmId", "dbo.Farms");
            DropForeignKey("dbo.FarmCrops", "Crop_CropId", "dbo.Crops");
            DropIndex("dbo.FarmCrops", new[] { "Farm_FarmId" });
            DropIndex("dbo.FarmCrops", new[] { "Crop_CropId" });
            AddColumn("dbo.Crops", "Farms_FarmId", c => c.Int());
            CreateIndex("dbo.Crops", "Farms_FarmId");
            AddForeignKey("dbo.Crops", "Farms_FarmId", "dbo.Farms", "FarmId");
            DropTable("dbo.FarmCrops");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FarmCrops",
                c => new
                    {
                        Farm_FarmId = c.Int(nullable: false),
                        Crop_CropId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Farm_FarmId, t.Crop_CropId });
            
            DropForeignKey("dbo.Crops", "Farms_FarmId", "dbo.Farms");
            DropIndex("dbo.Crops", new[] { "Farms_FarmId" });
            DropColumn("dbo.Crops", "Farms_FarmId");
            CreateIndex("dbo.FarmCrops", "Crop_CropId");
            CreateIndex("dbo.FarmCrops", "Farm_FarmId");
            AddForeignKey("dbo.FarmCrops", "Crop_CropId", "dbo.Crops", "CropId", cascadeDelete: true);
            AddForeignKey("dbo.FarmCrops", "Farm_FarmId", "dbo.Farms", "FarmId", cascadeDelete: true);
        }
    }
}
