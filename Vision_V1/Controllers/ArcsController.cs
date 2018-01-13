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
    public class ArcsController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Index()
        {
            var arcs = db.Arcs.Where(b => b.BookId == PageManager.CurrentBook.ID).ToList();
            PageManager.Arcs = arcs;

            return pc.AddIndexPage("Arcs", arcs);
        }

        public ActionResult IndexToList()
        {
            var arcs = db.Arcs.Where(b => b.BookId == PageManager.CurrentBook.ID).ToList();
            PageManager.Arcs = arcs;
            pc.ExchangePages("Details", "Books", "List", "Arcs", arcs);

            return View("PageView");
        }

        public ActionResult List()
        {
            var arcs = db.Arcs.Where(b => b.BookId == PageManager.CurrentBook.ID).ToList();
            PageManager.Arcs = arcs;

            return pc.AddListPage("Arcs", arcs);
        }

        public ActionResult Details(int? id)
        {
            Arc arc = null;
            if (id == null)
            {
                arc = db.Arcs.Find(PageManager.CurrentArc.ID);
            }
            else
            {
                arc = db.Arcs.Find(id);
            }
            if (arc == null)
            {
                return HttpNotFound();
            }

            pc.RemoveAllPages("Chapters");
            PageManager.CurrentArc = arc;
            return pc.AddDetailsPage("Arcs", arc, 5);
        }

        public ActionResult Create()
        {
            return pc.AddCreatePage("Arcs", new Arc(), 3, new List<int>() { 2, 3, 1 });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arc arc = db.Arcs.Find(id);
            if (arc == null)
            {
                return HttpNotFound();
            }

            Session["destinedMethod"] = "Details";
            if (PageManager.CurrentArc == null)
            {
                Session["destinedController"] = "Books";
            }
            else
            {
                Session["destinedController"] = "Arcs";
            }

            PageManager.CurrentArc = arc;
            return pc.AddEditPage("Arcs", arc, 4, new List<int>() { 2, 3, 3, 1 });
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arc arc = db.Arcs.Find(id);
            if (arc == null)
            {
                return HttpNotFound();
            }

            Session["destinedMethod"] = "Details";
            if (PageManager.CurrentArc == null)
            {
                Session["destinedController"] = "Books";
            }
            else
            {
                Session["destinedController"] = "Arcs";
            }

            PageManager.CurrentArc = arc;
            return pc.AddDeletePage("Arcs", arc, 5);
        }

        // POST: Arcs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,EstimatedDuration")] Arc arc)
        {
            pc.RemovePage("Create","Arcs");

            if (ModelState.IsValid)
            {
                var book = PageManager.CurrentBook;
                arc.BookId = book.ID;
                db.Arcs.Add(arc);
                arc.LastModified = DateTime.Now;
                db.SaveChanges();

                PageManager.CurrentArc = arc;

                return RedirectToAction("Details", new { id = arc.ID });
            }

            return RedirectToAction("Create", new { id=arc.BookId});
        }

        // POST: Arcs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,EstimatedDuration,BookId")] Arc arc)
        {
            pc.RemovePage("Edit","Arcs");

            if (ModelState.IsValid)
            {
                db.Entry(arc).State = EntityState.Modified;
                arc.LastModified = DateTime.Now;
                db.SaveChanges();

                if ((string)Session["destinedController"] == "Arcs")
                {
                    return RedirectToAction("Details", new { id = arc.ID });
                }
                else
                {
                    return RedirectToAction((string)Session["destinedMethod"], (string)Session["destinedController"]);
                }
            }
            return RedirectToAction("Edit", new { id = arc.ID });
        }

        // POST: Arcs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pc.RemovePage("Delete","Arcs");
            pc.RemovePage("Details", "Arcs");

            Arc arc = db.Arcs.Find(id);
            db.Arcs.Remove(arc);
            db.SaveChanges();
            return RedirectToAction("Details","Books",new { id=arc.BookId});
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
