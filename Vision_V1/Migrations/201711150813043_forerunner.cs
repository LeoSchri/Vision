namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forerunner : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Scenes", "ForerunnerID", "dbo.Scenes");
            DropIndex("dbo.Scenes", new[] { "ForerunnerID" });
            CreateTable(
                "dbo.SceneScenes",
                c => new
                    {
                        Scene_1_ID = c.Int(nullable: false),
                        Scene_2_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Scene_1_ID, t.Scene_2_ID })
                .ForeignKey("dbo.Scenes", t => t.Scene_1_ID)
                .ForeignKey("dbo.Scenes", t => t.Scene_2_ID)
                .Index(t => t.Scene_1_ID)
                .Index(t => t.Scene_2_ID);
            
            DropColumn("dbo.Scenes", "ForerunnerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Scenes", "ForerunnerID", c => c.Int(nullable: false));
            DropForeignKey("dbo.SceneScenes", "Scene_2_ID", "dbo.Scenes");
            DropForeignKey("dbo.SceneScenes", "Scene_1_ID", "dbo.Scenes");
            DropIndex("dbo.SceneScenes", new[] { "Scene_2_ID" });
            DropIndex("dbo.SceneScenes", new[] { "Scene_1_ID" });
            DropTable("dbo.SceneScenes");
            CreateIndex("dbo.Scenes", "ForerunnerID");
            AddForeignKey("dbo.Scenes", "ForerunnerID", "dbo.Scenes", "ID");
        }
    }
}
