namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scenePOVCharacterIdForeignKeyConstraint : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Scenes", "POVCharacterId", "dbo.Characters");
        }

        public override void Down()
        {
            AddForeignKey("dbo.Scenes", "POVCharacterId", "dbo.Characters", "ID");

        }
    }
}
