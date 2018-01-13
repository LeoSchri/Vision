namespace Vision_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Arcs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        PlotlineId = c.Int(),
                        EstimatedDuration = c.Int(nullable: false),
                        Summary = c.String(),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Plotlines", t => t.PlotlineId)
                .Index(t => t.BookId)
                .Index(t => t.PlotlineId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        PlotlineId = c.Int(),
                        EstimatedDuration = c.Int(nullable: false),
                        Summary = c.String(),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Plotlines", t => t.PlotlineId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.PlotlineId);
            
            CreateTable(
                "dbo.Plotlines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        EstimatedDuration = c.Int(nullable: false),
                        Summary = c.String(),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        GeneralInformation = c.String(),
                        Appearance = c.String(),
                        Surname = c.String(),
                        Gender = c.String(),
                        Race = c.String(),
                        Status = c.String(),
                        Birthday = c.String(),
                        Birthplace = c.String(),
                        Reference = c.String(),
                        Profession = c.String(),
                        SpecialFeatures = c.String(),
                        History = c.String(),
                        IsMainCharacter = c.Boolean(nullable: false),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.CharacterEvolvements",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CharacterDescription = c.String(nullable: false),
                        CharacterId = c.Int(nullable: false),
                        SceneId = c.Int(),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Characters", t => t.CharacterId, cascadeDelete: true)
                .ForeignKey("dbo.Scenes", t => t.SceneId)
                .Index(t => t.CharacterId)
                .Index(t => t.SceneId);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        CharacterEvolvement_ID = c.Int(),
                        Chapter_ID = c.Int(),
                        Location_ID = c.Int(),
                        Information_ID = c.Int(),
                        Scene_ID = c.Int(),
                        RelationshipPhase_ID = c.Int(),
                        Relationship_ID = c.Int(),
                        Character_ID = c.Int(),
                        Plotline_ID = c.Int(),
                        Book_ID = c.Int(),
                        Arc_ID = c.Int(),
                        InformationContent_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CharacterEvolvements", t => t.CharacterEvolvement_ID)
                .ForeignKey("dbo.Chapters", t => t.Chapter_ID)
                .ForeignKey("dbo.Locations", t => t.Location_ID)
                .ForeignKey("dbo.Information", t => t.Information_ID)
                .ForeignKey("dbo.Scenes", t => t.Scene_ID)
                .ForeignKey("dbo.RelationshipPhases", t => t.RelationshipPhase_ID)
                .ForeignKey("dbo.Relationships", t => t.Relationship_ID)
                .ForeignKey("dbo.Characters", t => t.Character_ID)
                .ForeignKey("dbo.Plotlines", t => t.Plotline_ID)
                .ForeignKey("dbo.Books", t => t.Book_ID)
                .ForeignKey("dbo.Arcs", t => t.Arc_ID)
                .ForeignKey("dbo.Information", t => t.InformationContent_ID)
                .Index(t => t.CharacterEvolvement_ID)
                .Index(t => t.Chapter_ID)
                .Index(t => t.Location_ID)
                .Index(t => t.Information_ID)
                .Index(t => t.Scene_ID)
                .Index(t => t.RelationshipPhase_ID)
                .Index(t => t.Relationship_ID)
                .Index(t => t.Character_ID)
                .Index(t => t.Plotline_ID)
                .Index(t => t.Book_ID)
                .Index(t => t.Arc_ID)
                .Index(t => t.InformationContent_ID);
            
            CreateTable(
                "dbo.Scenes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(),
                        ProjectId = c.Int(nullable: false),
                        Mood = c.Int(nullable: false),
                        Weather = c.Int(nullable: false),
                        ChapterId = c.Int(),
                        LocationId = c.Int(),
                        POVCharacterId = c.Int(),
                        ForerunnerID = c.Int(nullable: false),
                        EstimatedDuration = c.Int(nullable: false),
                        Summary = c.String(),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Chapters", t => t.ChapterId)
                .ForeignKey("dbo.Scenes", t => t.ForerunnerID)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.Characters", t => t.POVCharacterId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.ChapterId)
                .Index(t => t.LocationId)
                .Index(t => t.POVCharacterId)
                .Index(t => t.ForerunnerID);
            
            CreateTable(
                "dbo.Chapters",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArcId = c.Int(nullable: false),
                        EstimatedDuration = c.Int(nullable: false),
                        Summary = c.String(),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Arcs", t => t.ArcId, cascadeDelete: true)
                .Index(t => t.ArcId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        ParentId = c.Int(),
                        Description = c.String(nullable: false),
                        Orientation = c.Int(nullable: false),
                        Climate = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Locations", t => t.ParentId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        LastEdited = c.DateTime(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LinkType = c.Int(nullable: false),
                        EstimatedDuration = c.Boolean(nullable: false),
                        SeparatedCharacterProperties = c.Boolean(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Information",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        Location_ID = c.Int(),
                        Scene_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.Location_ID)
                .ForeignKey("dbo.Scenes", t => t.Scene_ID)
                .Index(t => t.ProjectId)
                .Index(t => t.Location_ID)
                .Index(t => t.Scene_ID);
            
            CreateTable(
                "dbo.RelationshipPhases",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SceneId = c.Int(),
                        Relation = c.Int(nullable: false),
                        RelationshipId = c.Int(nullable: false),
                        Closeness = c.Int(nullable: false),
                        ThoughtsOnCounterpart = c.String(),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Relationships", t => t.RelationshipId, cascadeDelete: true)
                .ForeignKey("dbo.Scenes", t => t.SceneId)
                .Index(t => t.SceneId)
                .Index(t => t.RelationshipId);
            
            CreateTable(
                "dbo.Relationships",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CharacterId = c.Int(nullable: false),
                        CounterpartCharacterId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Characters", t => t.CharacterId, cascadeDelete: true)
                .ForeignKey("dbo.Characters", t => t.CounterpartCharacterId)
                .Index(t => t.CharacterId)
                .Index(t => t.CounterpartCharacterId);
            
            CreateTable(
                "dbo.Information",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Name = c.String(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        InformationContent_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Information", t => t.InformationContent_ID)
                .Index(t => t.InformationContent_ID);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SceneCharacters",
                c => new
                    {
                        Scene_ID = c.Int(nullable: false),
                        Character_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Scene_ID, t.Character_ID })
                .ForeignKey("dbo.Scenes", t => t.Scene_ID)
                .ForeignKey("dbo.Characters", t => t.Character_ID)
                .Index(t => t.Scene_ID)
                .Index(t => t.Character_ID);
            
            CreateTable(
                "dbo.ScenePlotlines",
                c => new
                    {
                        Scene_ID = c.Int(nullable: false),
                        Plotline_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Scene_ID, t.Plotline_ID })
                .ForeignKey("dbo.Scenes", t => t.Scene_ID)
                .ForeignKey("dbo.Plotlines", t => t.Plotline_ID)
                .Index(t => t.Scene_ID)
                .Index(t => t.Plotline_ID);
            
            CreateTable(
                "dbo.PlotlineCharacters",
                c => new
                    {
                        Plotline_ID = c.Int(nullable: false),
                        Character_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Plotline_ID, t.Character_ID })
                .ForeignKey("dbo.Plotlines", t => t.Plotline_ID)
                .ForeignKey("dbo.Characters", t => t.Character_ID)
                .Index(t => t.Plotline_ID)
                .Index(t => t.Character_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.Information", "InformationContent_ID", "dbo.Information");
            DropForeignKey("dbo.Notes", "InformationContent_ID", "dbo.Information");
            DropForeignKey("dbo.Notes", "Arc_ID", "dbo.Arcs");
            DropForeignKey("dbo.Arcs", "PlotlineId", "dbo.Plotlines");
            DropForeignKey("dbo.Arcs", "BookId", "dbo.Books");
            DropForeignKey("dbo.Books", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Notes", "Book_ID", "dbo.Books");
            DropForeignKey("dbo.Books", "PlotlineId", "dbo.Plotlines");
            DropForeignKey("dbo.Plotlines", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Notes", "Plotline_ID", "dbo.Plotlines");
            DropForeignKey("dbo.PlotlineCharacters", "Character_ID", "dbo.Characters");
            DropForeignKey("dbo.PlotlineCharacters", "Plotline_ID", "dbo.Plotlines");
            DropForeignKey("dbo.Characters", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Notes", "Character_ID", "dbo.Characters");
            DropForeignKey("dbo.CharacterEvolvements", "SceneId", "dbo.Scenes");
            DropForeignKey("dbo.Information", "Scene_ID", "dbo.Scenes");
            DropForeignKey("dbo.RelationshipPhases", "SceneId", "dbo.Scenes");
            DropForeignKey("dbo.RelationshipPhases", "RelationshipId", "dbo.Relationships");
            DropForeignKey("dbo.Notes", "Relationship_ID", "dbo.Relationships");
            DropForeignKey("dbo.Relationships", "CounterpartCharacterId", "dbo.Characters");
            DropForeignKey("dbo.Relationships", "CharacterId", "dbo.Characters");
            DropForeignKey("dbo.Notes", "RelationshipPhase_ID", "dbo.RelationshipPhases");
            DropForeignKey("dbo.Scenes", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Scenes", "POVCharacterId", "dbo.Characters");
            DropForeignKey("dbo.ScenePlotlines", "Plotline_ID", "dbo.Plotlines");
            DropForeignKey("dbo.ScenePlotlines", "Scene_ID", "dbo.Scenes");
            DropForeignKey("dbo.Notes", "Scene_ID", "dbo.Scenes");
            DropForeignKey("dbo.Scenes", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Information", "Location_ID", "dbo.Locations");
            DropForeignKey("dbo.Locations", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Information", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Notes", "Information_ID", "dbo.Information");
            DropForeignKey("dbo.Projects", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Settings", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Locations", "ParentId", "dbo.Locations");
            DropForeignKey("dbo.Notes", "Location_ID", "dbo.Locations");
            DropForeignKey("dbo.Scenes", "ForerunnerID", "dbo.Scenes");
            DropForeignKey("dbo.Scenes", "ChapterId", "dbo.Chapters");
            DropForeignKey("dbo.Notes", "Chapter_ID", "dbo.Chapters");
            DropForeignKey("dbo.Chapters", "ArcId", "dbo.Arcs");
            DropForeignKey("dbo.SceneCharacters", "Character_ID", "dbo.Characters");
            DropForeignKey("dbo.SceneCharacters", "Scene_ID", "dbo.Scenes");
            DropForeignKey("dbo.Notes", "CharacterEvolvement_ID", "dbo.CharacterEvolvements");
            DropForeignKey("dbo.CharacterEvolvements", "CharacterId", "dbo.Characters");
            DropIndex("dbo.PlotlineCharacters", new[] { "Character_ID" });
            DropIndex("dbo.PlotlineCharacters", new[] { "Plotline_ID" });
            DropIndex("dbo.ScenePlotlines", new[] { "Plotline_ID" });
            DropIndex("dbo.ScenePlotlines", new[] { "Scene_ID" });
            DropIndex("dbo.SceneCharacters", new[] { "Character_ID" });
            DropIndex("dbo.SceneCharacters", new[] { "Scene_ID" });
            DropIndex("dbo.Information", new[] { "InformationContent_ID" });
            DropIndex("dbo.Relationships", new[] { "CounterpartCharacterId" });
            DropIndex("dbo.Relationships", new[] { "CharacterId" });
            DropIndex("dbo.RelationshipPhases", new[] { "RelationshipId" });
            DropIndex("dbo.RelationshipPhases", new[] { "SceneId" });
            DropIndex("dbo.Information", new[] { "Scene_ID" });
            DropIndex("dbo.Information", new[] { "Location_ID" });
            DropIndex("dbo.Information", new[] { "ProjectId" });
            DropIndex("dbo.Settings", new[] { "ApplicationUserId" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Projects", new[] { "ApplicationUserId" });
            DropIndex("dbo.Locations", new[] { "ParentId" });
            DropIndex("dbo.Locations", new[] { "ProjectId" });
            DropIndex("dbo.Chapters", new[] { "ArcId" });
            DropIndex("dbo.Scenes", new[] { "ForerunnerID" });
            DropIndex("dbo.Scenes", new[] { "POVCharacterId" });
            DropIndex("dbo.Scenes", new[] { "LocationId" });
            DropIndex("dbo.Scenes", new[] { "ChapterId" });
            DropIndex("dbo.Scenes", new[] { "ProjectId" });
            DropIndex("dbo.Notes", new[] { "InformationContent_ID" });
            DropIndex("dbo.Notes", new[] { "Arc_ID" });
            DropIndex("dbo.Notes", new[] { "Book_ID" });
            DropIndex("dbo.Notes", new[] { "Plotline_ID" });
            DropIndex("dbo.Notes", new[] { "Character_ID" });
            DropIndex("dbo.Notes", new[] { "Relationship_ID" });
            DropIndex("dbo.Notes", new[] { "RelationshipPhase_ID" });
            DropIndex("dbo.Notes", new[] { "Scene_ID" });
            DropIndex("dbo.Notes", new[] { "Information_ID" });
            DropIndex("dbo.Notes", new[] { "Location_ID" });
            DropIndex("dbo.Notes", new[] { "Chapter_ID" });
            DropIndex("dbo.Notes", new[] { "CharacterEvolvement_ID" });
            DropIndex("dbo.CharacterEvolvements", new[] { "SceneId" });
            DropIndex("dbo.CharacterEvolvements", new[] { "CharacterId" });
            DropIndex("dbo.Characters", new[] { "ProjectId" });
            DropIndex("dbo.Plotlines", new[] { "ProjectId" });
            DropIndex("dbo.Books", new[] { "PlotlineId" });
            DropIndex("dbo.Books", new[] { "ProjectId" });
            DropIndex("dbo.Arcs", new[] { "PlotlineId" });
            DropIndex("dbo.Arcs", new[] { "BookId" });
            DropTable("dbo.PlotlineCharacters");
            DropTable("dbo.ScenePlotlines");
            DropTable("dbo.SceneCharacters");
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.Information");
            DropTable("dbo.Relationships");
            DropTable("dbo.RelationshipPhases");
            DropTable("dbo.Information");
            DropTable("dbo.Settings");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.Projects");
            DropTable("dbo.Locations");
            DropTable("dbo.Chapters");
            DropTable("dbo.Scenes");
            DropTable("dbo.Notes");
            DropTable("dbo.CharacterEvolvements");
            DropTable("dbo.Characters");
            DropTable("dbo.Plotlines");
            DropTable("dbo.Books");
            DropTable("dbo.Arcs");
        }
    }
}
