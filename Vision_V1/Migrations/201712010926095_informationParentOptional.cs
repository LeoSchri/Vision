namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class informationParentOptional : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Information", new[] { "ParentId" });
            AlterColumn("dbo.Information", "ParentId", c => c.Int());
            CreateIndex("dbo.Information", "ParentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Information", new[] { "ParentId" });
            AlterColumn("dbo.Information", "ParentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Information", "ParentId");
        }
    }
}
