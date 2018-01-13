namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mainCharacter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MainCharacters",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    ProjectId = c.Int(nullable: false),
                    LastModified = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);

            AddColumn("dbo.Characters", "MainCharacterId", c => c.Int());
            AddColumn("dbo.Notes", "MainCharacter_ID", c => c.Int());
            CreateIndex("dbo.Characters", "MainCharacterId");
            CreateIndex("dbo.Notes", "MainCharacter_ID");
            AddForeignKey("dbo.Notes", "MainCharacter_ID", "dbo.MainCharacters", "ID");
            AddForeignKey("dbo.Characters", "MainCharacterId", "dbo.MainCharacters", "ID");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Characters", "MainCharacterId", "dbo.MainCharacters");
            DropForeignKey("dbo.MainCharacters", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Notes", "MainCharacter_ID", "dbo.MainCharacters");
            DropIndex("dbo.MainCharacters", new[] { "ProjectId" });
            DropIndex("dbo.Notes", new[] { "MainCharacter_ID" });
            DropIndex("dbo.Characters", new[] { "MainCharacterId" });
            DropColumn("dbo.Notes", "MainCharacter_ID");
            DropColumn("dbo.Characters", "MainCharacterId");
            DropTable("dbo.MainCharacters");
        }
    }
}
