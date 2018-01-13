namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Scene_Story : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Scenes", "Story", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Scenes", "Story");
        }
    }
}
