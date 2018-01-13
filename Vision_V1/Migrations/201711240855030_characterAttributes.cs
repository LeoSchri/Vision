namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class characterAttributes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CharacterAttributes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CharacterId = c.Int(nullable: false),
                        AttributeId = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Attributes", t => t.AttributeId)
                .ForeignKey("dbo.Characters", t => t.CharacterId)
                .Index(t => t.CharacterId)
                .Index(t => t.AttributeId);
            
            CreateTable(
                "dbo.Attributes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Size = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CharacterAttributes", "CharacterId", "dbo.Characters");
            DropForeignKey("dbo.CharacterAttributes", "AttributeId", "dbo.Attributes");
            DropIndex("dbo.CharacterAttributes", new[] { "AttributeId" });
            DropIndex("dbo.CharacterAttributes", new[] { "CharacterId" });
            DropTable("dbo.Attributes");
            DropTable("dbo.CharacterAttributes");
        }
    }
}
