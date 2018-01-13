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
    public class RelationshipPhasesController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Index()
        {
            var relationshipPhases = db.RelationshipPhases.Where(b => b.RelationshipId == PageManager.CurrentRelationship.ID).ToList();
            PageManager.RelationshipPhases = relationshipPhases;

            return pc.AddIndexPage("RelationshipPhases", relationshipPhases);
        }

        public ActionResult IndexToList()
        {
            var relationshipPhases = db.RelationshipPhases.Where(b => b.RelationshipId == PageManager.CurrentRelationship.ID).ToList();
            PageManager.RelationshipPhases = relationshipPhases;
            pc.ExchangePages("Details", "Relationships", "List", "RelationshipPhases", relationshipPhases);

            return View("PageView");
        }

        public ActionResult List()
        {
            var relationshipPhases = db.RelationshipPhases.Where(b => b.RelationshipId == PageManager.CurrentRelationship.ID).ToList();
            PageManager.RelationshipPhases = relationshipPhases;

            return pc.AddListPage("RelationshipPhases", relationshipPhases);
        }

        public ActionResult Details(int? id)
        {
            RelationshipPhase relationshipPhase = null;
            if (id == null)
            {
                relationshipPhase = db.RelationshipPhases.Find(PageManager.CurrentRelationshipPhase.ID);
            }
            else
            {
                relationshipPhase = db.RelationshipPhases.Find(id);
            }
            if (relationshipPhase == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentRelationshipPhase = relationshipPhase;

            return pc.AddDetailsPage("RelationshipPhases", relationshipPhase, 4);
        }

        public ActionResult Create(int? id)
        {
            Relationship relation = null;
            if (id == null)
            {
                relation = db.Relationships.Find(PageManager.CurrentRelationship.ID);
            }
            else
            {
                relation = db.Relationships.Find(id);
            }
            if (relation == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentRelationship = relation;

            return pc.AddCreatePage("RelationshipPhases", new RelationshipPhase(), 4, new List<int>() { 2, 1, 1, 3 });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RelationshipPhase relationshipPhase = db.RelationshipPhases.Find(id);
            if (relationshipPhase == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentRelationshipPhase = relationshipPhase;

            return pc.AddEditPage("RelationshipPhases", relationshipPhase, 4, new List<int>() { 2, 1, 1, 3 });
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RelationshipPhase relationshipPhase = db.RelationshipPhases.Find(id);
            if (relationshipPhase == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentRelationshipPhase = relationshipPhase;

            return pc.AddDeletePage("RelationshipPhases", relationshipPhase, 4);
        }

        public ActionResult PickForRC()
        {
            var scene = PageManager.CurrentScene;
            var characters = db.Characters.Where(c => c.ProjectId == PageManager.CurrentProject.ID).ToList();
            var relationshipPhases = new List<RelationshipPhase>();

            characters.ForEach(c =>
            {
                c.Relationships.ToList().ForEach(r =>
                {
                    r.Phases.ToList().ForEach(p =>
                    {
                        if(p.SceneId == null)
                        {
                            relationshipPhases.Add(p);
                        }
                    });
                });
            });

            Session["destinedController"] = "Scenes";
            Session["destinedMethod"] = "Details";
            Session["method"] = "BindRelationshipPhaseToScene";

            return pc.AddPickManyPage("RelationshipPhases", relationshipPhases);
        }

        // POST: RelationshipPhases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Relation,Closeness,ThoughtsOnCounterpart,Name")] RelationshipPhase relationshipPhase)
        {
            pc.RemovePage("Create", "RelationshipPhases");

            if (ModelState.IsValid)
            {
                relationshipPhase.LastModified = DateTime.Now;
                var relationship = db.Relationships.Find(PageManager.CurrentRelationship.ID);
                if (relationship != null)
                {
                    db.Entry(relationship).State = EntityState.Modified;
                    relationship.LastModified = DateTime.Now;
                    db.SaveChanges();

                    relationshipPhase.Relationship = relationship;
                    relationshipPhase.RelationshipId = relationship.ID;

                    db.RelationshipPhases.Add(relationshipPhase);
                    db.SaveChanges();
                }
                return RedirectToAction("Details", new { id = relationshipPhase.ID });
            }

            return RedirectToAction("Create");
        }

        // POST: RelationshipPhases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Relation,Closeness,ThoughtsOnCounterpart,Name")] RelationshipPhase relationshipPhase)
        {
            pc.RemovePage("Edit", "RelationshipPhases");

            if (ModelState.IsValid)
            {
                db.Entry(relationshipPhase).State = EntityState.Modified;
                relationshipPhase.LastModified = DateTime.Now;
                db.SaveChanges();

                return RedirectToAction("Details", new { id = relationshipPhase.ID });
            }
            return RedirectToAction("Edit");
        }

        // POST: RelationshipPhases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pc.RemovePage("Delete", "RelationshipPhases");

            RelationshipPhase relationshipPhase = db.RelationshipPhases.Find(id);
            db.RelationshipPhases.Remove(relationshipPhase);
            db.SaveChanges();
            return RedirectToAction("Details","Relationships",new { id = relationshipPhase.RelationshipId });
        }

        public ActionResult BindRelationshipPhaseToScene(int? id)
        {
            var scene = db.Scenes.Find(PageManager.CurrentScene.ID);
            RelationshipPhase relationChange = null;
            if (id != null)
            {
                relationChange = db.RelationshipPhases.Find(id);
                if (scene == null || relationChange == null)
                {
                    return HttpNotFound();
                }

                scene.RelationshipChanges.Add(relationChange);
                relationChange.SceneId = scene.ID;
                db.SaveChanges();
            }

            pc.RemovePage("PickMany", "RelationshipPhases");
            var characters = db.Characters.Where(c => c.ProjectId == PageManager.CurrentProject.ID).ToList();
            var relationshipPhases = new List<RelationshipPhase>();
            characters.ForEach(c =>
            {
                c.Relationships.ToList().ForEach(r =>
                {
                    r.Phases.ToList().ForEach(p =>
                    {
                        if (p.SceneId == null)
                        {
                            relationshipPhases.Add(p);
                        }
                    });
                });
            });
            if (relationshipPhases == null || !relationshipPhases.Any())
            {
                return RedirectToAction("Details", "Scenes", new { id = scene.ID });
            }
            else
            {
                return RedirectToAction("PickForRC");
            }
        }

        public ActionResult RemoveRelationshipPhaseFromScene(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var relationChange = db.RelationshipPhases.Find(id);
            var scene = db.Scenes.Find(PageManager.CurrentScene.ID);
            if (relationChange == null || scene == null)
            {
                return HttpNotFound();
            }

            relationChange.SceneId = null;
            relationChange.Scene = null;
            scene.RelationshipChanges.Remove(relationChange);
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
