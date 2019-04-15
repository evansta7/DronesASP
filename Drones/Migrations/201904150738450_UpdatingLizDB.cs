namespace Drones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingLizDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Farmers", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Farmers", "UserId");
        }
    }
}
