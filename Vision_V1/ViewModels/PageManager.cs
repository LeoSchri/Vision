using System;
using System.Collections.Generic;
using System.Linq;
using Vision_V1.Models;

namespace Vision_V1.ViewModels
{
    public class PageManager
    {
        private static Chapter currentChapter;

        public static List<Project> Projects { get; set; }
        public static Project CurrentProject { get; set; }

        public static List<Book> Books { get; set; }
        public static Book CurrentBook { get; set; }

        public static List<Arc> Arcs { get; set; }
        public static Arc CurrentArc { get; set; }

        public static List<Chapter> Chapters { get; set; }
        public static Chapter CurrentChapter {
            get
            {
                return currentChapter;
            }
            set
            {
                currentChapter = value;
                if(currentChapter != null)
                {
                    CheckIfPreviousChapterExists();
                    CheckIfNextChapterExists();
                }
            }
        }

        public static bool HasPreviousChapter { get; set; } = false;
        public static bool HasNextChapter { get; set; } = false;

        public static List<Scene> Scenes { get; set; }
        public static Scene CurrentScene { get; set; }

        public static List<Plotline> Plotlines { get; set; }
        public static Plotline CurrentPlotline { get; set; }

        public static List<Character> Characters { get; set; }
        public static Character CurrentCharacter { get; set; }

        public static List<MainCharacter> MainCharacters { get; set; }
        public static MainCharacter CurrentMainCharacter { get; set; }

        public static List<CharacterEvolvement> CharacterEvolvements { get; set; }
        public static CharacterEvolvement CurrentCharacterEvolvement { get; set; }

        public static List<Relationship> Relationships { get; set; }
        public static Relationship CurrentRelationship { get; set; }

        public static List<RelationshipPhase> RelationshipPhases { get; set; }
        public static RelationshipPhase CurrentRelationshipPhase { get; set; }

        public static List<Location> Locations { get; set; }
        public static Location CurrentLocation { get; set; }

        public static List<Information> Information { get; set; }
        public static Information CurrentInformation { get; set; }

        public static List<Models.Attribute> Attributes { get; set; }
        public static Models.Attribute CurrentAttribute { get; set; }

        //public static List<CharacterAttribute> CharacterAttributes { get; set; }
        //public static CharacterAttribute CurrentCharacterAttribute { get; set; }

        public static List<Page> Pages { get; set; } = new List<Page>();
        public static int StructureId { get; set; }

        private static ApplicationDbContext db = new ApplicationDbContext();

        private static void CheckIfNextChapterExists()
        {
            var chapters = new List<Chapter>();
            db.Books.Where(b => b.ProjectId == CurrentProject.ID).ToList().ForEach(b => b.Arcs.ToList().ForEach(a => chapters.AddRange(a.Chapters)));

            var currentChapter = chapters.Find(c => c.ID == CurrentChapter.ID);
            int? index = chapters.IndexOf(currentChapter);
            if(index != null)
            {
                if(chapters.Count > index+1)
                {
                    HasNextChapter = true;
                }
                else
                {
                    HasNextChapter = false;
                }
            }
        }

        private static void CheckIfPreviousChapterExists()
        {
            var chapters = new List<Chapter>();
            db.Books.Where(b => b.ProjectId == CurrentProject.ID).ToList().ForEach(b => b.Arcs.ToList().ForEach(a => chapters.AddRange(a.Chapters)));

            var currentChapter = chapters.Find(c => c.ID == CurrentChapter.ID);
            int? index = chapters.IndexOf(currentChapter);
            if (index != null)
            {
                if (index != 0)
                {
                    HasPreviousChapter = true;
                }
                else
                {
                    HasPreviousChapter = false;
                }
            }
        }

        public static void Reset()
        {
            var pc = new PageManager();
            var props = pc.GetType().GetProperties();
            foreach(var prop in props)
            {
                if(prop.Name != "CurrentProject" && prop.Name != "Attributes")
                {
                    prop.SetValue(pc,null);
                }
            }
        }
    }

    public class Page
    {
        public string Name { get; private set; }
        public string Controller { get; set; }
        public object Model { get; set; }

        public PageSize Size { get; set; }

        public int PropertyCount { get; set; }
        public List<int> PropertySizes { get; set; }

        public Page(string name, string controller, object model, int propertyCount=0, List<int> propertySizes = null, int height = 1, int width = 1)
        {
            Name = name;
            Controller = controller;
            Model = model;
            PropertyCount = propertyCount;
            PropertySizes = propertySizes;
            Size = new PageSize(height, width);
        }
    }

    public class PageSize
    {
        public int Height { get; set; }
        public int Width { get; set; }

        public PageSize(int height, int width)
        {
            Height = height;
            Width = width;
        }
    }
}