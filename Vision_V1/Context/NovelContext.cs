using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Vision_V1.Models;

namespace Vision_V1.Context
{
    public class NovelContext : DbContext
    {
        public NovelContext() : base()
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Arc> Arcs { get; set; }
        public DbSet<Plotline> Plotlines { get; set; }
        public DbSet<Scene> Scenes { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterEvolvement> CharacterEvolvements { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<RelationshipPhase> RelationshipPhases { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Information> Information { get; set; }
        public DbSet<InformationContent> InformationContents { get; set; }

        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            builder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            builder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });


            builder.Entity<Setting>()
            .HasRequired(n => n.ApplicationUser)
            .WithMany(a => a.Settings)
            .HasForeignKey(n => n.ApplicationUserId)
            .WillCascadeOnDelete(true);

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
            .HasRequired(s => s.Forerunner)
            .WithMany(s => s.DependentScenes)
            .HasForeignKey(n => n.ForerunnerID)
            .WillCascadeOnDelete(false);

            builder.Entity<Scene>()
            .HasMany(s => s.AttendantCharacters)
            .WithMany(c => c.Scenes)
            .Map(cs =>
             {
                 cs.MapLeftKey("Scene_ID");
                 cs.MapRightKey("Character_ID");
                 cs.ToTable("SceneCharacters");
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

            builder.Entity<Character>()
            .HasRequired(s => s.Project)
            .WithMany(s => s.Characters)
            .HasForeignKey(n => n.ProjectId)
            .WillCascadeOnDelete(true);

            builder.Entity<Information>()
            .HasRequired(s => s.Project)
            .WithMany(s => s.Information)
            .HasForeignKey(n => n.ProjectId)
            .WillCascadeOnDelete(true);

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

            builder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        public DbSet<Vision_V1.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}