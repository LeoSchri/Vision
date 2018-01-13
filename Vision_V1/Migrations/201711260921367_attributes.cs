namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attributes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attributes", "ProjectId", c => c.Int(nullable: false));
            CreateIndex("dbo.Attributes", "ProjectId");
            AddForeignKey("dbo.Attributes", "ProjectId", "dbo.Projects", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attributes", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Attributes", new[] { "ProjectId" });
            DropColumn("dbo.Attributes", "ProjectId");
        }
    }
}
