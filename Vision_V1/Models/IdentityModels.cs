using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Vision_V1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            return userIdentity;
        }

        public virtual ICollection<Project> Projects { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("NovelContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Arc> Arcs { get; set; }
        public DbSet<Plotline> Plotlines { get; set; }
        public DbSet<Scene> Scenes { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<MainCharacter> MainCharacters { get; set; }
        public DbSet<CharacterEvolvement> CharacterEvolvements { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<RelationshipPhase> RelationshipPhases { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Information> Information { get; set; }

        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<CharacterAttribute> CharacterAttributes { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            builder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            builder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            builder.Entity<Project>()
            .HasRequired(n => n.ApplicationUser)
            .WithMany(a => a.Projects)
            .HasForeignKey(n => n.ApplicationUserId)
            .WillCascadeOnDelete(true);

            builder.Entity<Book>()
            .HasRequired(s => s.Project)
            .WithMany(s => s.Books)
            .HasForeignKey(n => n.ProjectId)
            .WillCascadeOnDelete(true);

            builder.Entity<Arc>()
            .HasRequired(s => s.Book)
            .WithMany(s => s.Arcs)
            .HasForeignKey(n => n.BookId)
            .WillCascadeOnDelete(true);

            builder.Entity<Chapter>()
            .HasRequired(s => s.Arc)
            .WithMany(s => s.Chapters)
            .HasForeignKey(n => n.ArcId)
            .WillCascadeOnDelete(true);

            builder.Entity<Scene>()
            .HasOptional(s => s.Chapter)
            .WithMany(s => s.Scenes)
            .HasForeignKey(n => n.ChapterId)
            .WillCascadeOnDelete(false);

            builder.Entity<Scene>()
            .HasOptional(s => s.Location)
            .WithMany(s => s.Scenes)
            .HasForeignKey(n => n.LocationId)
            .WillCascadeOnDelete(false);

            builder.Entity<Scene>()
            .HasOptional(s => s.POVCharacter)
            .WithMany(s => s.POVScenes)
            .HasForeignKey(n => n.POVCharacterId)
            .WillCascadeOnDelete(false);

            builder.Entity<Scene>()
            .HasRequired(s => s.Project)
            .WithMany(s => s.Scenes)
            .HasForeignKey(n => n.ProjectId)
            .WillCascadeOnDelete(true);

            builder.Entity<Scene>()
            .HasMany(s => s.DependentScenesF)
            .WithMany(c => c.DependentScenesA)
            .Map(cs =>
            {
                cs.MapLeftKey("Scene_1_ID");
                cs.MapRightKey("Scene_2_ID");
                cs.ToTable("SceneScenes");
            });

            builder.Entity<Scene>()
            .HasMany(s => s.AttendantCharacters)
            .WithMany(c => c.Scenes)
            .Map(cs =>
            {
                cs.MapLeftKey("Scene_ID");
                cs.MapRightKey("Character_ID");
                cs.ToTable("SceneCharacters");
            });

            builder.Entity<Scene>()
            .HasMany(s => s.Plotlines)
            .WithMany(c => c.Scenes)
            .Map(cs =>
            {
                cs.MapLeftKey("Scene_ID");
                cs.MapRightKey("Plotline_ID");
                cs.ToTable("ScenePlotlines");
            });

            builder.Entity<Scene>()
            .HasMany(s => s.RelevantInformation)
            .WithMany(c => c.Scenes)
            .Map(cs =>
            {
                cs.MapLeftKey("Scene_ID");
                cs.MapRightKey("Information_ID");
                cs.ToTable("SceneInformation");
            });

            builder.Entity<RelationshipPhase>()
            .HasOptional(s => s.Scene)
            .WithMany(s => s.RelationshipChanges)
            .HasForeignKey(n => n.SceneId)
            .WillCascadeOnDelete(false);

            builder.Entity<RelationshipPhase>()
            .HasRequired(s => s.Relationship)
            .WithMany(s => s.Phases)
            .HasForeignKey(n => n.RelationshipId)
            .WillCascadeOnDelete(true);

            builder.Entity<Relationship>()
            .HasRequired(s => s.Character)
            .WithMany(s => s.Relationships)
            .HasForeignKey(n => n.CharacterId)
            .WillCascadeOnDelete(true);

            builder.Entity<Relationship>()
            .HasRequired(s => s.CounterpartCharacter)
            .WithMany(s => s.RelationshipCounterparts)
            .HasForeignKey(n => n.CounterpartCharacterId)
            .WillCascadeOnDelete(false);

            builder.Entity<CharacterEvolvement>()
            .HasRequired(s => s.Character)
            .WithMany(s => s.EvolvementSteps)
            .HasForeignKey(n => n.CharacterId)
            .WillCascadeOnDelete(true);

            builder.Entity<CharacterEvolvement>()
            .HasOptional(s => s.Scene)
            .WithMany(s => s.CharacterEvolvements)
            .HasForeignKey(n => n.SceneId)
            .WillCascadeOnDelete(false);

            builder.Entity<Plotline>()
            .HasRequired(s => s.Project)
            .WithMany(s => s.Plotlines)
            .HasForeignKey(n => n.ProjectId)
            .WillCascadeOnDelete(true);

            builder.Entity<Plotline>()
            .HasMany(s => s.Characters)
            .WithMany(c => c.Plotlines)
            .Map(cs =>
            {
                cs.MapLeftKey("Plotline_ID");
                cs.MapRightKey("Character_ID");
                cs.ToTable("PlotlineCharacters");
            });

            builder.Entity<Plotline>()
            .HasMany(s => s.DependentPlotlinesF)
            .WithMany(c => c.DependentPlotlinesA)
            .Map(cs =>
            {
                cs.MapLeftKey("Plotline_1_ID");
                cs.MapRightKey("Plotline_2_ID");
                cs.ToTable("PlotlinePlotlines");
            });

            builder.Entity<Plotline>()
            .HasMany(s => s.RelevantInformation)
            .WithMany(c => c.Plotlines)
            .Map(cs =>
            {
                cs.MapLeftKey("Plotline_ID");
                cs.MapRightKey("Information_ID");
                cs.ToTable("PlotlineInformation");
            });

            builder.Entity<Character>()
            .HasRequired(s => s.Project)
            .WithMany(s => s.Characters)
            .HasForeignKey(n => n.ProjectId)
            .WillCascadeOnDelete(true);

            builder.Entity<MainCharacter>()
            .HasRequired(s => s.Project)
            .WithMany(s => s.MainCharacters)
            .HasForeignKey(n => n.ProjectId)
            .WillCascadeOnDelete(true);

            builder.Entity<Character>()
            .HasMany(s => s.RelevantInformation)
            .WithMany(c => c.Characters)
            .Map(cs =>
            {
                cs.MapLeftKey("Character_ID");
                cs.MapRightKey("Information_ID");
                cs.ToTable("CharacterInformation");
            });

            builder.Entity<Information>()
            .HasRequired(s => s.Project)
            .WithMany(s => s.Information)
            .HasForeignKey(n => n.ProjectId)
            .WillCascadeOnDelete(true);

            builder.Entity<Information>()
            .HasOptional(s => s.Parent)
            .WithMany(s => s.SubContents)
            .HasForeignKey(n => n.ParentId)
            .WillCascadeOnDelete(false);

            builder.Entity<Location>()
            .HasRequired(s => s.Project)
            .WithMany(s => s.Locations)
            .HasForeignKey(n => n.ProjectId)
            .WillCascadeOnDelete(true);

            builder.Entity<Location>()
            .HasOptional(s => s.Parent)
            .WithMany(s => s.SubLocations)
            .HasForeignKey(n => n.ParentId)
            .WillCascadeOnDelete(false);

            builder.Entity<Location>()
            .HasMany(s => s.RelevantInformation)
            .WithMany(c => c.Locations)
            .Map(cs =>
            {
                cs.MapLeftKey("Location_ID");
                cs.MapRightKey("Information_ID");
                cs.ToTable("LocationInformation");
            });

            builder.Entity<Attribute>()
            .HasRequired(s => s.Project)
            .WithMany(s => s.Attributes)
            .HasForeignKey(n => n.ProjectId)
            .WillCascadeOnDelete(true);

            builder.Entity<CharacterAttribute>()
            .HasOptional(s => s.Attribute)
            .WithMany(s => s.CharacterAttributes)
            .HasForeignKey(n => n.AttributeId)
            .WillCascadeOnDelete(false);

            builder.Entity<CharacterAttribute>()
            .HasOptional(s => s.Character)
            .WithMany(s => s.CharacterAttributes)
            .HasForeignKey(n => n.CharacterId)
            .WillCascadeOnDelete(false);

            builder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        //public DbSet<Vision_V1.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}