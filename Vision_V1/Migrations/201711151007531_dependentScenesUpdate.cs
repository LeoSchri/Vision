namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dependentScenesUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Scenes", "Scene_ID", "dbo.Scenes");
            DropIndex("dbo.Scenes", new[] { "Scene_ID" });
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
            
            DropColumn("dbo.Scenes", "Scene_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Scenes", "Scene_ID", c => c.Int());
            DropForeignKey("dbo.SceneScenes", "Scene_2_ID", "dbo.Scenes");
            DropForeignKey("dbo.SceneScenes", "Scene_1_ID", "dbo.Scenes");
            DropIndex("dbo.SceneScenes", new[] { "Scene_2_ID" });
            DropIndex("dbo.SceneScenes", new[] { "Scene_1_ID" });
            DropTable("dbo.SceneScenes");
            CreateIndex("dbo.Scenes", "Scene_ID");
            AddForeignKey("dbo.Scenes", "Scene_ID", "dbo.Scenes", "ID");
        }
    }
}
