namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attributeOrderID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attributes", "OrderID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attributes", "OrderID");
        }
    }
}
