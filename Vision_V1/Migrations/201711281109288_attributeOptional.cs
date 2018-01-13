namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attributeOptional : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CharacterAttributes", new[] { "CharacterId" });
            DropIndex("dbo.CharacterAttributes", new[] { "AttributeId" });
            AlterColumn("dbo.CharacterAttributes", "CharacterId", c => c.Int());
            AlterColumn("dbo.CharacterAttributes", "AttributeId", c => c.Int());
            CreateIndex("dbo.CharacterAttributes", "CharacterId");
            CreateIndex("dbo.CharacterAttributes", "AttributeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CharacterAttributes", new[] { "AttributeId" });
            DropIndex("dbo.CharacterAttributes", new[] { "CharacterId" });
            AlterColumn("dbo.CharacterAttributes", "AttributeId", c => c.Int(nullable: false));
            AlterColumn("dbo.CharacterAttributes", "CharacterId", c => c.Int(nullable: false));
            CreateIndex("dbo.CharacterAttributes", "AttributeId");
            CreateIndex("dbo.CharacterAttributes", "CharacterId");
        }
    }
}
