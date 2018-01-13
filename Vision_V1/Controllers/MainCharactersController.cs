using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vision_V1.Models;
using Vision_V1.ViewModels;

namespace Vision_V1.Controllers
{
    public class MainCharactersController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Home()
        {
            var mainCharacters = db.MainCharacters.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.MainCharacters = mainCharacters;

            return pc.AddHomePage("MainCharacters", mainCharacters);
        }

        public ActionResult Index()
        {
            var mainCharacters = db.MainCharacters.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.MainCharacters = mainCharacters;

            return pc.AddIndexPage("MainCharacters", mainCharacters);
        }

        public ActionResult List()
        {
            var mainCharacters = db.MainCharacters.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.MainCharacters = mainCharacters;

            return pc.AddListPage("MainCharacters", mainCharacters);
        }

        public ActionResult Details(int? id)
        {
            MainCharacter mainCharacter = null;
            if (id == null)
            {
                mainCharacter = db.MainCharacters.Find(PageManager.CurrentMainCharacter.ID);
            }
            else
            {
                mainCharacter = db.MainCharacters.Find(id);
            }
            if (mainCharacter == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentMainCharacter = mainCharacter;
            var propertyCount = 4;

            return pc.AddDetailsPage("MainCharacters", mainCharacter, propertyCount);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainCharacter mainCharacter = db.MainCharacters.Find(id);
            if (mainCharacter == null)
            {
                return HttpNotFound();
            }

            Session["destinedController"] = "MainCharacters";
            if (PageManager.CurrentMainCharacter == null)
            {
                Session["destinedMethod"] = "Home";
            }
            else
            {
                Session["destinedMethod"] = "Details";
            }

            PageManager.CurrentMainCharacter = mainCharacter;
            var propertyCount = 4;

            return pc.AddDeletePage("MainCharacters", mainCharacter, propertyCount);
        }

        public ActionResult PickForPOVC()
        {
            var scene = PageManager.CurrentScene;
            var mainCharacters = db.MainCharacters.Where(c => c.ProjectId == PageManager.CurrentProject.ID && c.ID != scene.POVCharacterId).ToList();
            Session["destinedController"] = "Scenes";
            Session["destinedMethod"] = "Details";
            Session["method"] = "BindPOVCharacterToScene";

            return pc.AddPickOnePage("MainCharacters", mainCharacters);
        }

        public void Create(MainCharacter mainCharacter, int CharacterId)
        {
            mainCharacter.ProjectId = PageManager.CurrentProject.ID;
            mainCharacter.LastModified = DateTime.Now;
            db.MainCharacters.Add(mainCharacter);
            db.SaveChanges();

            var character = db.Characters.Find(CharacterId);
            db.Entry(character).State = EntityState.Modified;
            character.MainCharacterId = mainCharacter.ID;
            db.SaveChanges();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pc.RemovePage("Delete", "MainCharacters");

            MainCharacter mainCharacter = db.MainCharacters.Find(id);

            Helper.RemoveMainCharacterReferences(mainCharacter);

            var character = db.Characters.ToList().Find(c => c.MainCharacterId == mainCharacter.ID);
            db.Entry(character).State = EntityState.Modified;
            character.MainCharacterId = null;
            character.MainCharacter = null;
            character.IsMainCharacter = false;
            db.SaveChanges();

            db.MainCharacters.Remove(mainCharacter);
            db.SaveChanges();

            return RedirectToAction("Home");
        }

        public ActionResult BindPOVCharacterToScene(int? id)
        {
            var scene = db.Scenes.Find(PageManager.CurrentScene.ID);
            MainCharacter mainCharacter = null;
            if (id != null)
            {
                mainCharacter = db.MainCharacters.Find(id);
                if (scene == null || mainCharacter == null)
                {
                    return HttpNotFound();
                }

                scene.POVCharacterId = mainCharacter.ID;
                mainCharacter.POVScenes.Add(scene);
            }
            else scene.POVCharacterId = null;
            db.SaveChanges();

            pc.RemovePage("PickOne", "MainCharacters");

            return RedirectToAction("Details", "Scenes", new { id = scene.ID });
        }
    }
}