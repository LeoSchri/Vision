namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class propertyProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Arcs", "ProjectId", c => c.Int());
            AddColumn("dbo.Notes", "ProjectId", c => c.Int());
            AddColumn("dbo.Chapters", "ProjectId", c => c.Int());
            AddColumn("dbo.CharacterEvolvements", "ProjectId", c => c.Int());
            AddColumn("dbo.RelationshipPhases", "ProjectId", c => c.Int());
            AddColumn("dbo.Relationships", "ProjectId", c => c.Int());
            CreateIndex("dbo.Arcs", "ProjectId");
            CreateIndex("dbo.Notes", "ProjectId");
            CreateIndex("dbo.Chapters", "ProjectId");
            CreateIndex("dbo.CharacterEvolvements", "ProjectId");
            CreateIndex("dbo.RelationshipPhases", "ProjectId");
            CreateIndex("dbo.Relationships", "ProjectId");
            AddForeignKey("dbo.Notes", "ProjectId", "dbo.Projects", "ID");
            AddForeignKey("dbo.Chapters", "ProjectId", "dbo.Projects", "ID");
            AddForeignKey("dbo.CharacterEvolvements", "ProjectId", "dbo.Projects", "ID");
            AddForeignKey("dbo.RelationshipPhases", "ProjectId", "dbo.Projects", "ID");
            AddForeignKey("dbo.Relationships", "ProjectId", "dbo.Projects", "ID");
            AddForeignKey("dbo.Arcs", "ProjectId", "dbo.Projects", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Arcs", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Relationships", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.RelationshipPhases", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.CharacterEvolvements", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Chapters", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Notes", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Relationships", new[] { "ProjectId" });
            DropIndex("dbo.RelationshipPhases", new[] { "ProjectId" });
            DropIndex("dbo.CharacterEvolvements", new[] { "ProjectId" });
            DropIndex("dbo.Chapters", new[] { "ProjectId" });
            DropIndex("dbo.Notes", new[] { "ProjectId" });
            DropIndex("dbo.Arcs", new[] { "ProjectId" });
            DropColumn("dbo.Relationships", "ProjectId");
            DropColumn("dbo.RelationshipPhases", "ProjectId");
            DropColumn("dbo.CharacterEvolvements", "ProjectId");
            DropColumn("dbo.Chapters", "ProjectId");
            DropColumn("dbo.Notes", "ProjectId");
            DropColumn("dbo.Arcs", "ProjectId");
        }
    }
}
