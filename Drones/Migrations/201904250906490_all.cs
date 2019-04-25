namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class all : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UAVFarms", "UAV_DroneId", "dbo.UAVs");
            DropForeignKey("dbo.UAVFarms", "Farm_FarmId", "dbo.Farms");
            DropIndex("dbo.UAVFarms", new[] { "UAV_DroneId" });
            DropIndex("dbo.UAVFarms", new[] { "Farm_FarmId" });
            AddColumn("dbo.Farms", "UAV_DroneId", c => c.Int());
            AddColumn("dbo.UAVs", "Farms_FarmId", c => c.Int());
            AddColumn("dbo.UAVs", "Farm_FarmId", c => c.Int());
            CreateIndex("dbo.Farms", "UAV_DroneId");
            CreateIndex("dbo.UAVs", "Farms_FarmId");
            CreateIndex("dbo.UAVs", "Farm_FarmId");
            AddForeignKey("dbo.Farms", "UAV_DroneId", "dbo.UAVs", "DroneId");
            AddForeignKey("dbo.UAVs", "Farms_FarmId", "dbo.Farms", "FarmId");
            AddForeignKey("dbo.UAVs", "Farm_FarmId", "dbo.Farms", "FarmId");
            DropTable("dbo.UAVFarms");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UAVFarms",
                c => new
                    {
                        UAV_DroneId = c.Int(nullable: false),
                        Farm_FarmId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UAV_DroneId, t.Farm_FarmId });
            
            DropForeignKey("dbo.UAVs", "Farm_FarmId", "dbo.Farms");
            DropForeignKey("dbo.UAVs", "Farms_FarmId", "dbo.Farms");
            DropForeignKey("dbo.Farms", "UAV_DroneId", "dbo.UAVs");
            DropIndex("dbo.UAVs", new[] { "Farm_FarmId" });
            DropIndex("dbo.UAVs", new[] { "Farms_FarmId" });
            DropIndex("dbo.Farms", new[] { "UAV_DroneId" });
            DropColumn("dbo.UAVs", "Farm_FarmId");
            DropColumn("dbo.UAVs", "Farms_FarmId");
            DropColumn("dbo.Farms", "UAV_DroneId");
            CreateIndex("dbo.UAVFarms", "Farm_FarmId");
            CreateIndex("dbo.UAVFarms", "UAV_DroneId");
            AddForeignKey("dbo.UAVFarms", "Farm_FarmId", "dbo.Farms", "FarmId", cascadeDelete: true);
            AddForeignKey("dbo.UAVFarms", "UAV_DroneId", "dbo.UAVs", "DroneId", cascadeDelete: true);
        }
    }
}
