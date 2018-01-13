namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dependentPlotlines : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlotlinePlotlines",
                c => new
                    {
                        Plotline_1_ID = c.Int(nullable: false),
                        Plotline_2_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Plotline_1_ID, t.Plotline_2_ID })
                .ForeignKey("dbo.Plotlines", t => t.Plotline_1_ID)
                .ForeignKey("dbo.Plotlines", t => t.Plotline_2_ID)
                .Index(t => t.Plotline_1_ID)
                .Index(t => t.Plotline_2_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlotlinePlotlines", "Plotline_2_ID", "dbo.Plotlines");
            DropForeignKey("dbo.PlotlinePlotlines", "Plotline_1_ID", "dbo.Plotlines");
            DropIndex("dbo.PlotlinePlotlines", new[] { "Plotline_2_ID" });
            DropIndex("dbo.PlotlinePlotlines", new[] { "Plotline_1_ID" });
            DropTable("dbo.PlotlinePlotlines");
        }
    }
}
