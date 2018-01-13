using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vision_V1.Models;
using Vision_V1.ViewModels;
using static Vision_V1.Models.Types;

namespace Vision_V1.Controllers
{
    public class CharacterEvolvementsController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Index()
        {
            var characterEvolvements = db.CharacterEvolvements.Where(b => b.CharacterId == PageManager.CurrentCharacter.ID).ToList();
            PageManager.CharacterEvolvements = characterEvolvements;

            return pc.AddIndexPage("CharacterEvolvements", characterEvolvements);
        }

        public ActionResult IndexToList()
        {
            var characterEvolvements = db.CharacterEvolvements.Where(b => b.CharacterId == PageManager.CurrentCharacter.ID).ToList();
            PageManager.CharacterEvolvements = characterEvolvements;
            pc.ExchangePages("Details", "Characters", "List", "CharacterEvolvements", characterEvolvements);

            return View("PageView");
        }

        public ActionResult List()
        {
            var characterEvolvements = db.CharacterEvolvements.Where(b => b.CharacterId == PageManager.CurrentCharacter.ID).ToList();
            PageManager.CharacterEvolvements = characterEvolvements;

            return pc.AddListPage("CharacterEvolvements", characterEvolvements);
        }

        public ActionResult Details(int? id)
        {
            CharacterEvolvement characterEvolvement = null;
            if (id == null)
            {
                characterEvolvement = db.CharacterEvolvements.Find(PageManager.CurrentCharacterEvolvement.ID);
            }
            else
            {
                characterEvolvement = db.CharacterEvolvements.Find(id);
            }
            if (characterEvolvement == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentCharacterEvolvement = characterEvolvement;

            return pc.AddDetailsPage("CharacterEvolvements", characterEvolvement,2);
        }

        public ActionResult Create()
        {
            return pc.AddCreatePage("CharacterEvolvements", new CharacterEvolvement(),2, new List<int>() { 2, 3 });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CharacterEvolvement characterEvolvement = db.CharacterEvolvements.Find(id);
            if (characterEvolvement == null)
            {
                return HttpNotFound();
            }

            Session["destinedMethod"] = "Details";
            if (PageManager.CurrentArc == null)
            {
                Session["destinedController"] = "Characters";
            }
            else
            {
                Session["destinedController"] = "CharacterEvolvements";
            }

            PageManager.CurrentCharacterEvolvement = characterEvolvement;

            return pc.AddEditPage("CharacterEvolvements", characterEvolvement,2, new List<int>() { 2, 3 });
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CharacterEvolvement characterEvolvement = db.CharacterEvolvements.Find(id);
            if (characterEvolvement == null)
            {
                return HttpNotFound();
            }

            Session["destinedMethod"] = "Details";
            if (PageManager.CurrentArc == null)
            {
                Session["destinedController"] = "Characters";
            }
            else
            {
                Session["destinedController"] = "CharacterEvolvements";
            }

            PageManager.CurrentCharacterEvolvement = characterEvolvement;

            return pc.AddDeletePage("CharacterEvolvements", characterEvolvement,2);
        }

        public ActionResult PickForCD()
        {
            var scene = PageManager.CurrentScene;
            var characters = db.Characters.Where(c => c.ProjectId == PageManager.CurrentProject.ID).ToList();
            var characterEvolvements = new List<CharacterEvolvement>();
            
            characters.ForEach(c =>
            {
                c.EvolvementSteps.ToList().ForEach(es =>
                {
                    if (es.SceneId == null && c.EvolvementSteps.ToList().IndexOf(es) != 0)
                        characterEvolvements.Add(es);
                });
            });

            Session["destinedController"] = "Scenes";
            Session["destinedMethod"] = "Details";
            Session["method"] = "BindEvolvementToScene";

            return pc.AddPickManyPage("CharacterEvolvements", characterEvolvements);
        }

        // POST: CharacterEvolvements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CharacterDescription,Name")] CharacterEvolvement characterEvolvement)
        {
            pc.RemovePage("Create", "CharacterEvolvements");

            if (ModelState.IsValid)
            {
                characterEvolvement.LastModified = DateTime.Now;
                var character = PageManager.CurrentCharacter;
                if(character != null)
                {
                    characterEvolvement.CharacterId = character.ID;

                    db.CharacterEvolvements.Add(characterEvolvement);
                    db.SaveChanges();
                }
                return RedirectToAction("Details", new { id = characterEvolvement.ID });
            }

            return RedirectToAction("Create");
        }

        // POST: CharacterEvolvements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CharacterDescription,Name,CharacterId")] CharacterEvolvement characterEvolvement)
        {
            pc.RemovePage("Edit", "CharacterEvolvements");

            if (ModelState.IsValid)
            {
                db.Entry(characterEvolvement).State = EntityState.Modified;
                characterEvolvement.LastModified = DateTime.Now;
                characterEvolvement.Character = db.Characters.Find(characterEvolvement.CharacterId);
                db.SaveChanges();
                if ((string)Session["destinedController"] == "CharacterEvolvements")
                {
                    return RedirectToAction("Details", new { id = characterEvolvement.ID });
                }
                else
                {
                    return RedirectToAction((string)Session["destinedMethod"], (string)Session["destinedController"]);
                }
            }
            return RedirectToAction("Edit");
        }

        // POST: CharacterEvolvements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pc.RemovePage("Delete", "CharacterEvolvements");

            CharacterEvolvement characterEvolvement = db.CharacterEvolvements.Find(id);
            db.CharacterEvolvements.Remove(characterEvolvement);
            db.SaveChanges();
            return RedirectToAction("Details","Characters",new { id = characterEvolvement.CharacterId });
        }

        public ActionResult BindEvolvementToScene(int? id)
        {
            var scene = db.Scenes.Find(PageManager.CurrentScene.ID);
            CharacterEvolvement evolvement = null;
            if (id != null)
            {
                evolvement = db.CharacterEvolvements.Find(id);
                if (scene == null || evolvement == null)
                {
                    return HttpNotFound();
                }

                scene.CharacterEvolvements.Add(evolvement);
                evolvement.SceneId = scene.ID;
                db.SaveChanges();
            }

            pc.RemovePage("PickMany", "CharacterEvolvements");
            var characters = db.Characters.Where(c => c.ProjectId == PageManager.CurrentProject.ID).ToList();
            var characterEvolvements = new List<CharacterEvolvement>();
            characters.ForEach(c =>
            {
                c.EvolvementSteps.ToList().ForEach(es =>
                {
                    if (es.SceneId == null && c.EvolvementSteps.ToList().IndexOf(es) != 0)
                        characterEvolvements.Add(es);
                });
            });
            if (characterEvolvements == null || !characterEvolvements.Any())
            {
                return RedirectToAction("Details", "Scenes", new { id = scene.ID });
            }
            else
            {
                return RedirectToAction("PickForCD");
            }
        }

        public ActionResult RemoveEvolvementFromScene(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var evolvement = db.CharacterEvolvements.Find(id);
            var scene = db.Scenes.Find(PageManager.CurrentScene.ID);
            if (evolvement == null || scene == null)
            {
                return HttpNotFound();
            }

            evolvement.SceneId = null;
            evolvement.Scene = null;
            scene.CharacterEvolvements.Remove(evolvement);
            db.SaveChanges();

            return RedirectToAction("Details", "Scenes", new { id = scene.ID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
