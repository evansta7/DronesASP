namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCropsTable : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Crops");
        }
    }
}
