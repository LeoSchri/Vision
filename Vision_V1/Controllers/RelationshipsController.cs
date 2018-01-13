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
    public class RelationshipsController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Index()
        {
            var relationships = db.Relationships.Where(b => b.CharacterId == PageManager.CurrentCharacter.ID).ToList();
            PageManager.Relationships = relationships;

            return pc.AddIndexPage("Relationships", relationships);
        }

        public ActionResult IndexToList()
        {
            var relationships = db.Relationships.Where(b => b.CharacterId == PageManager.CurrentCharacter.ID).ToList();
            PageManager.Relationships = relationships;
            pc.ExchangePages("Details", "Characters", "List", "Relationships", relationships);

            return View("PageView");
        }

        public ActionResult List()
        {
            var relationships = db.Relationships.Where(b => b.CharacterId == PageManager.CurrentCharacter.ID).ToList();
            PageManager.Relationships = relationships;

            return pc.AddListPage("Relationships", relationships);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Relationship relationship = db.Relationships.Find(id);
            if (relationship == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentRelationship = relationship;

            return pc.AddDetailsPage("Relationships", relationship, 0);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Relationship relationship = db.Relationships.Find(id);
            if (relationship == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentRelationship = relationship;

            return pc.AddDeletePage("Relationships", relationship,5);
        }

        // POST: Relationships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pc.RemovePage("Delete", "Relationships");

            Relationship relationship = db.Relationships.Find(id);
            db.Relationships.Remove(relationship);
            db.SaveChanges();
            return RedirectToAction("Details","Characters",new { id = relationship.CharacterId });
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
