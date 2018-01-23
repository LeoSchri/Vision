namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Duration_string : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Arcs", "EstimatedDuration", c => c.String());
            AlterColumn("dbo.Books", "EstimatedDuration", c => c.String());
            AlterColumn("dbo.Plotlines", "EstimatedDuration", c => c.String());
            AlterColumn("dbo.Scenes", "EstimatedDuration", c => c.String());
            AlterColumn("dbo.Chapters", "EstimatedDuration", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Chapters", "EstimatedDuration", c => c.Int(nullable: false));
            AlterColumn("dbo.Scenes", "EstimatedDuration", c => c.Int(nullable: false));
            AlterColumn("dbo.Plotlines", "EstimatedDuration", c => c.Int(nullable: false));
            AlterColumn("dbo.Books", "EstimatedDuration", c => c.Int(nullable: false));
            AlterColumn("dbo.Arcs", "EstimatedDuration", c => c.Int(nullable: false));
        }
    }
}
