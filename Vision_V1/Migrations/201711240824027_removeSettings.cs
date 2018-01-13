namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeSettings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Settings", "ApplicationUserId", "dbo.ApplicationUsers");
            DropIndex("dbo.Settings", new[] { "ApplicationUserId" });
            DropTable("dbo.Settings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EstimatedDuration = c.Boolean(nullable: false),
                        SeparatedCharacterProperties = c.Boolean(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.Settings", "ApplicationUserId");
            AddForeignKey("dbo.Settings", "ApplicationUserId", "dbo.ApplicationUsers", "Id", cascadeDelete: true);
        }
    }
}
