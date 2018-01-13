namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class baseElement_Change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "LastModified", c => c.DateTime(nullable: false));
            DropColumn("dbo.Projects", "LastEdited");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "LastEdited", c => c.DateTime(nullable: false));
            DropColumn("dbo.Projects", "LastModified");
        }
    }
}
