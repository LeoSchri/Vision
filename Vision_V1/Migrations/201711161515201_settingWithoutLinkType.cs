namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class settingWithoutLinkType : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Settings", "LinkType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Settings", "LinkType", c => c.Int(nullable: false));
        }
    }
}
