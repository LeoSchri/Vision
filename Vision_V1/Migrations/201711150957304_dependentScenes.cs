namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dependentScenes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SceneScenes", "Scene_1_ID", "dbo.Scenes");
            DropForeignKey("dbo.SceneScenes", "Scene_2_ID", "dbo.Scenes");
            DropIndex("dbo.SceneScenes", new[] { "Scene_1_ID" });
            DropIndex("dbo.SceneScenes", new[] { "Scene_2_ID" });
            AddColumn("dbo.Scenes", "Scene_ID", c => c.Int());
            CreateIndex("dbo.Scenes", "Scene_ID");
            AddForeignKey("dbo.Scenes", "Scene_ID", "dbo.Scenes", "ID");
            DropTable("dbo.SceneScenes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SceneScenes",
                c => new
                    {
                        Scene_1_ID = c.Int(nullable: false),
                        Scene_2_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Scene_1_ID, t.Scene_2_ID });
            
            DropForeignKey("dbo.Scenes", "Scene_ID", "dbo.Scenes");
            DropIndex("dbo.Scenes", new[] { "Scene_ID" });
            DropColumn("dbo.Scenes", "Scene_ID");
            CreateIndex("dbo.SceneScenes", "Scene_2_ID");
            CreateIndex("dbo.SceneScenes", "Scene_1_ID");
            AddForeignKey("dbo.SceneScenes", "Scene_2_ID", "dbo.Scenes", "ID");
            AddForeignKey("dbo.SceneScenes", "Scene_1_ID", "dbo.Scenes", "ID");
        }
    }
}
