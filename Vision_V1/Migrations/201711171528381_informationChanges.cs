namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class informationChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notes", "InformationContent_ID", "dbo.InformationContents");
            DropForeignKey("dbo.InformationContents", "InformationContent_ID", "dbo.InformationContents");
            DropIndex("dbo.Notes", new[] { "InformationContent_ID" });
            DropIndex("dbo.InformationContents", new[] { "InformationContent_ID" });
            AddColumn("dbo.Information", "Content", c => c.String());
            AddColumn("dbo.Information", "ParentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Information", "ParentId");
            AddForeignKey("dbo.Information", "ParentId", "dbo.Information", "ID");
            DropColumn("dbo.Notes", "InformationContent_ID");
            DropTable("dbo.InformationContents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InformationContents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        InformationContent_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Notes", "InformationContent_ID", c => c.Int());
            DropForeignKey("dbo.Information", "ParentId", "dbo.Information");
            DropIndex("dbo.Information", new[] { "ParentId" });
            DropColumn("dbo.Information", "ParentId");
            DropColumn("dbo.Information", "Content");
            CreateIndex("dbo.InformationContents", "InformationContent_ID");
            CreateIndex("dbo.Notes", "InformationContent_ID");
            AddForeignKey("dbo.InformationContents", "InformationContent_ID", "dbo.InformationContents", "ID");
            AddForeignKey("dbo.Notes", "InformationContent_ID", "dbo.InformationContents", "ID");
        }
    }
}
