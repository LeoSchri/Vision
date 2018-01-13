namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class relevantInformation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Information", "Location_ID", "dbo.Locations");
            DropForeignKey("dbo.Information", "Scene_ID", "dbo.Scenes");
            DropIndex("dbo.Information", new[] { "Location_ID" });
            DropIndex("dbo.Information", new[] { "Scene_ID" });
            CreateTable(
                "dbo.LocationInformation",
                c => new
                    {
                        Location_ID = c.Int(nullable: false),
                        Information_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Location_ID, t.Information_ID })
                .ForeignKey("dbo.Locations", t => t.Location_ID)
                .ForeignKey("dbo.Information", t => t.Information_ID)
                .Index(t => t.Location_ID)
                .Index(t => t.Information_ID);
            
            CreateTable(
                "dbo.SceneInformation",
                c => new
                    {
                        Scene_ID = c.Int(nullable: false),
                        Information_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Scene_ID, t.Information_ID })
                .ForeignKey("dbo.Scenes", t => t.Scene_ID)
                .ForeignKey("dbo.Information", t => t.Information_ID)
                .Index(t => t.Scene_ID)
                .Index(t => t.Information_ID);
            
            CreateTable(
                "dbo.CharacterInformation",
                c => new
                    {
                        Character_ID = c.Int(nullable: false),
                        Information_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Character_ID, t.Information_ID })
                .ForeignKey("dbo.Characters", t => t.Character_ID)
                .ForeignKey("dbo.Information", t => t.Information_ID)
                .Index(t => t.Character_ID)
                .Index(t => t.Information_ID);
            
            CreateTable(
                "dbo.PlotlineInformation",
                c => new
                    {
                        Plotline_ID = c.Int(nullable: false),
                        Information_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Plotline_ID, t.Information_ID })
                .ForeignKey("dbo.Plotlines", t => t.Plotline_ID)
                .ForeignKey("dbo.Information", t => t.Information_ID)
                .Index(t => t.Plotline_ID)
                .Index(t => t.Information_ID);
            
            DropColumn("dbo.Information", "Location_ID");
            DropColumn("dbo.Information", "Scene_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Information", "Scene_ID", c => c.Int());
            AddColumn("dbo.Information", "Location_ID", c => c.Int());
            DropForeignKey("dbo.PlotlineInformation", "Information_ID", "dbo.Information");
            DropForeignKey("dbo.PlotlineInformation", "Plotline_ID", "dbo.Plotlines");
            DropForeignKey("dbo.CharacterInformation", "Information_ID", "dbo.Information");
            DropForeignKey("dbo.CharacterInformation", "Character_ID", "dbo.Characters");
            DropForeignKey("dbo.SceneInformation", "Information_ID", "dbo.Information");
            DropForeignKey("dbo.SceneInformation", "Scene_ID", "dbo.Scenes");
            DropForeignKey("dbo.LocationInformation", "Information_ID", "dbo.Information");
            DropForeignKey("dbo.LocationInformation", "Location_ID", "dbo.Locations");
            DropIndex("dbo.PlotlineInformation", new[] { "Information_ID" });
            DropIndex("dbo.PlotlineInformation", new[] { "Plotline_ID" });
            DropIndex("dbo.CharacterInformation", new[] { "Information_ID" });
            DropIndex("dbo.CharacterInformation", new[] { "Character_ID" });
            DropIndex("dbo.SceneInformation", new[] { "Information_ID" });
            DropIndex("dbo.SceneInformation", new[] { "Scene_ID" });
            DropIndex("dbo.LocationInformation", new[] { "Information_ID" });
            DropIndex("dbo.LocationInformation", new[] { "Location_ID" });
            DropTable("dbo.PlotlineInformation");
            DropTable("dbo.CharacterInformation");
            DropTable("dbo.SceneInformation");
            DropTable("dbo.LocationInformation");
            CreateIndex("dbo.Information", "Scene_ID");
            CreateIndex("dbo.Information", "Location_ID");
            AddForeignKey("dbo.Information", "Scene_ID", "dbo.Scenes", "ID");
            AddForeignKey("dbo.Information", "Location_ID", "dbo.Locations", "ID");
        }
    }
}
