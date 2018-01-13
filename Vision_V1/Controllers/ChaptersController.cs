using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Vision_V1.Models;
using Vision_V1.ViewModels;

namespace Vision_V1.Controllers
{
    public class ChaptersController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Index()
        {
            var chapters = db.Chapters.Where(b => b.ArcId == PageManager.CurrentArc.ID).ToList();
            PageManager.Chapters = chapters;

            return pc.AddIndexPage("Chapters", chapters);
        }

        public ActionResult IndexToList()
        {
            var chapters = db.Chapters.Where(b => b.ArcId == PageManager.CurrentArc.ID).ToList();
            PageManager.Chapters = chapters;
            pc.ExchangePages("Details", "Arcs", "List", "Chapters", chapters);

            return View("PageView");
        }

        public ActionResult List()
        {
            var chapters = db.Chapters.Where(b => b.ArcId == PageManager.CurrentArc.ID).ToList();
            PageManager.Chapters = chapters;

            return pc.AddListPage("Chapters", chapters);
        }

        public ActionResult Details(int? id)
        {
            Chapter chapter = null;
            if (id == null)
            {
                chapter = db.Chapters.Find(PageManager.CurrentChapter.ID);
            }
            else
            {
                chapter = db.Chapters.Find(id);
            }
            if (chapter == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentChapter = chapter;
            pc.RemoveAllPages("Scenes");
            return pc.AddDetailsPage("Chapters", chapter, 3);
        }

        public ActionResult Create()
        {
            return pc.AddCreatePage("Chapters", new Chapter(), 2, new List<int>() { 2, 1 });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }

            Session["destinedMethod"] = "Details";
            if (PageManager.CurrentChapter == null)
            {
                Session["destinedController"] = "Arcs";
            }
            else
            {
                Session["destinedController"] = "Chapters";
            }

            PageManager.CurrentChapter = chapter;

            return pc.AddEditPage("Chapters", chapter, 2, new List<int>() { 2, 1 });
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }

            Session["destinedMethod"] = "Details";
            if (PageManager.CurrentChapter == null)
            {
                Session["destinedController"] = "Arcs";
            }
            else
            {
                Session["destinedController"] = "Chapters";
            }

            PageManager.CurrentChapter = chapter;

            return pc.AddDeletePage("Chapters", chapter, 3);
        }

        // POST: Chapters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,EstimatedDuration")] Chapter chapter)
        {
            pc.RemovePage("Create","Chapters");

            if (ModelState.IsValid)
            {
                chapter.LastModified = DateTime.Now;
                var arc = PageManager.CurrentArc;
                chapter.ArcId = arc.ID;
                db.Chapters.Add(chapter);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = chapter.ID });
            }

            return RedirectToAction("Create", new { id = chapter.ArcId });
        }

        // POST: Chapters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,EstimatedDuration,ArcId")] Chapter chapter)
        {
            pc.RemovePage("Edit","Chapters");

            if (ModelState.IsValid)
            {
                db.Entry(chapter).State = EntityState.Modified;
                chapter.LastModified = DateTime.Now;
                db.SaveChanges();
                if ((string)Session["destinedController"] == "Chapters")
                {
                    return RedirectToAction("Details", new { id = chapter.ID });
                }
                else
                {
                    return RedirectToAction((string)Session["destinedMethod"], (string)Session["destinedController"]);
                }
            }
            return RedirectToAction("Edit", new { id = chapter.ID });
        }

        // POST: Chapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pc.RemovePage("Delete","Chapters");

            Chapter chapter = db.Chapters.Find(id);
            db.Chapters.Remove(chapter);
            db.SaveChanges();
            return RedirectToAction("Details", "Arcs", new { id = chapter.ArcId });
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
