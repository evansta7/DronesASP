namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedUAVTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UAVs",
                c => new
                    {
                        DroneId = c.Int(nullable: false, identity: true),
                        DroneStatus = c.String(),
                        DroneType = c.String(),
                    })
                .PrimaryKey(t => t.DroneId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UAVs");
        }
    }
}
