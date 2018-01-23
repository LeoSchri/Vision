namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Scene_PointInTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Scenes", "PointInTime", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Scenes", "PointInTime");
        }
    }
}
