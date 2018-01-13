namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class characterPropsRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Characters", "GeneralInformation");
            DropColumn("dbo.Characters", "Appearance");
            DropColumn("dbo.Characters", "Surname");
            DropColumn("dbo.Characters", "Gender");
            DropColumn("dbo.Characters", "Race");
            DropColumn("dbo.Characters", "Status");
            DropColumn("dbo.Characters", "Birthday");
            DropColumn("dbo.Characters", "Birthplace");
            DropColumn("dbo.Characters", "Reference");
            DropColumn("dbo.Characters", "Profession");
            DropColumn("dbo.Characters", "SpecialFeatures");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Characters", "SpecialFeatures", c => c.String());
            AddColumn("dbo.Characters", "Profession", c => c.String());
            AddColumn("dbo.Characters", "Reference", c => c.String());
            AddColumn("dbo.Characters", "Birthplace", c => c.String());
            AddColumn("dbo.Characters", "Birthday", c => c.String());
            AddColumn("dbo.Characters", "Status", c => c.String());
            AddColumn("dbo.Characters", "Race", c => c.String());
            AddColumn("dbo.Characters", "Gender", c => c.String());
            AddColumn("dbo.Characters", "Surname", c => c.String());
            AddColumn("dbo.Characters", "Appearance", c => c.String());
            AddColumn("dbo.Characters", "GeneralInformation", c => c.String());
        }
    }
}
