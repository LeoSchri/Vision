using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vision_V1.ViewModels;

namespace Vision_V1.Models
{
    public class Helper
    {
        private static ApplicationDbContext db = ApplicationDbContext.Create();

        internal static void SetStructureId()
        {
            switch (PageManager.Pages.Count)
            {
                case 1: PageManager.StructureId = 1; break;
                case 2: PageManager.StructureId = 2; break;
                case 3: PageManager.StructureId = 8; break;
                case 4: PageManager.StructureId = 11; break;
                case 5: PageManager.StructureId = 13; break;
                case 6: PageManager.StructureId = 17; break;
            }
        }

        public static void RemoveBinding(ModelStateDictionary modelState, params string[] properties)
        {
            var props = properties.ToList();
            var indexList = new List<int>();

            for(int i = 0; i< props.Count; i++)
            {
                modelState.Keys.ToList().ForEach(k =>
                {
                    if (k.Contains(props[i]))
                    {
                        indexList.Add(modelState.Keys.ToList().IndexOf(k));
                    }
                });
            }

            if (indexList.Any())
            {
                indexList.ForEach(i =>
                {
                    modelState.Remove(modelState.ElementAt(i));
                });
            }
        }

        public static int? RemoveIfExists(Page page)
        {
            var duplicate = PageManager.Pages.Find(p => p.Name == page.Name && p.Controller == page.Controller);
            if (duplicate != null)
            {
                var index = PageManager.Pages.IndexOf(duplicate);
                PageManager.Pages.Remove(duplicate);
                return index;
            }
            return null;
        }

        public static void RemoveReferences(Project project)
        {
            project.Plotlines.ToList().ForEach(p =>RemovePlotReferences(p));
            project.Scenes.ToList().ForEach(s =>RemoveSceneReferences(s));
            project.Characters.ToList().ForEach(c =>RemoveCharacterReferences(c));
            project.MainCharacters.ToList().ForEach(c =>RemoveMainCharacterReferences(c));
            project.Locations.ToList().ForEach(l =>RemoveLocationReferences(l));
            project.Attributes.ToList().ForEach(a => RemoveAttributeReferences(a));
        }

        public static void RemovePlotReferences(Plotline plotline)
        {
            plotline.DependentPlotlinesA.ToList().RemoveRange(0, plotline.DependentPlotlinesA.Count);
            plotline.DependentPlotlinesF.ToList().RemoveRange(0, plotline.DependentPlotlinesF.Count);

            plotline.Scenes.ToList().RemoveRange(0, plotline.Scenes.Count);

            plotline.Characters.ToList().RemoveRange(0, plotline.Characters.Count);

            plotline.Notes.ToList().RemoveRange(0, plotline.Notes.Count);
            plotline.RelevantInformation.ToList().RemoveRange(0, plotline.RelevantInformation.Count);

            db.SaveChanges();
        }

        public static void RemoveSceneReferences(Scene scene)
        {
            scene.DependentScenesA.ToList().RemoveRange(0, scene.DependentScenesA.Count);
            scene.DependentScenesF.ToList().RemoveRange(0, scene.DependentScenesF.Count);

            scene.AttendantCharacters.ToList().RemoveRange(0, scene.AttendantCharacters.Count);

            scene.Plotlines.ToList().RemoveRange(0, scene.Plotlines.Count);

            scene.Notes.ToList().RemoveRange(0, scene.Notes.Count);
            scene.RelevantInformation.ToList().RemoveRange(0, scene.RelevantInformation.Count);

            db.SaveChanges();
        }

        public static void RemoveCharacterReferences(Character character)
        {
            character.Plotlines.ToList().RemoveRange(0, character.Plotlines.Count);

            character.Notes.ToList().RemoveRange(0, character.Notes.Count);
            character.RelevantInformation.ToList().RemoveRange(0, character.RelevantInformation.Count);
            character.CharacterAttributes.ToList().RemoveRange(0, character.CharacterAttributes.Count);

            db.SaveChanges();
        }

        public static void RemoveMainCharacterReferences(MainCharacter character)
        {
            character.POVScenes.ToList().RemoveRange(0, character.POVScenes.Count);
            character.Notes.ToList().RemoveRange(0, character.Notes.Count);

            db.SaveChanges();
        }

        public static void RemoveLocationReferences(Location location)
        {
            location.Notes.ToList().RemoveRange(0, location.Notes.Count);
            location.RelevantInformation.ToList().RemoveRange(0, location.RelevantInformation.Count);

            db.SaveChanges();
        }

        public static void RemoveAttributeReferences(Attribute attribute)
        {
            attribute.CharacterAttributes.ToList().RemoveRange(0, attribute.CharacterAttributes.Count);

            db.SaveChanges();
        }

        public static void RemoveInformationReferences(Information information)
        {
            information.Notes.ToList().RemoveRange(0, information.Notes.Count);
            information.Scenes.ToList().RemoveRange(0, information.Scenes.Count);
            information.Plotlines.ToList().RemoveRange(0, information.Plotlines.Count);
            information.Characters.ToList().RemoveRange(0, information.Characters.Count);
            information.Locations.ToList().RemoveRange(0, information.Locations.Count);

            db.SaveChanges();
        }
    }
}