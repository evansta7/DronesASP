namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addforeignkey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FarmerFarms",
                c => new
                    {
                        Farmer_FarmerId = c.Int(nullable: false),
                        Farm_FarmId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Farmer_FarmerId, t.Farm_FarmId })
                .ForeignKey("dbo.Farmers", t => t.Farmer_FarmerId, cascadeDelete: true)
                .ForeignKey("dbo.Farms", t => t.Farm_FarmId, cascadeDelete: true)
                .Index(t => t.Farmer_FarmerId)
                .Index(t => t.Farm_FarmId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FarmerFarms", "Farm_FarmId", "dbo.Farms");
            DropForeignKey("dbo.FarmerFarms", "Farmer_FarmerId", "dbo.Farmers");
            DropIndex("dbo.FarmerFarms", new[] { "Farm_FarmId" });
            DropIndex("dbo.FarmerFarms", new[] { "Farmer_FarmerId" });
            DropTable("dbo.FarmerFarms");
        }
    }
}
