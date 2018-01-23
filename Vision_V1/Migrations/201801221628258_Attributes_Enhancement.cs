namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attributes_Enhancement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attributes", "ShowInTable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attributes", "ShowInTable");
        }
    }
}
