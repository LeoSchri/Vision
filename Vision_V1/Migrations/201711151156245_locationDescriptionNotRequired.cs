namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class locationDescriptionNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Locations", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Locations", "Description", c => c.String(nullable: false));
        }
    }
}
