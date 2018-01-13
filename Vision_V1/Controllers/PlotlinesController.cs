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
    public class PlotlinesController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        private PageController pc = new PageController();

        public ActionResult Home()
        {
            var plotlines = db.Plotlines.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Plotlines = plotlines;

            return pc.AddHomePage("Plotlines", plotlines);
        }

        public ActionResult Index()
        {
            var plotlines = db.Plotlines.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Plotlines = plotlines;

            return pc.AddIndexPage("Plotlines", plotlines);
        }

        public ActionResult List()
        {
            var plotlines = db.Plotlines.Where(b => b.ProjectId == PageManager.CurrentProject.ID).ToList();
            PageManager.Plotlines = plotlines;

            return pc.AddListPage("Plotlines", plotlines);
        }

        public ActionResult Details(int? id)
        {
            Plotline plotline = null;
            if (id == null)
            {
                plotline = db.Plotlines.Find(PageManager.CurrentPlotline.ID);
            }
            else
            {
                plotline = db.Plotlines.Find(id);
            }
            if (plotline == null)
            {
                return HttpNotFound();
            }

            PageManager.CurrentPlotline = plotline;
            pc.RemoveAllPages("Scenes");
            pc.RemoveAllPages("Characters");
            return pc.AddDetailsPage("Plotlines", plotline,4);
        }

        public ActionResult Create()
        {
            return pc.AddCreatePage("Plotlines", new Plotline(),2, new List<int>() { 2, 3 });
        }

        public ActionResult CreateChild()
        {
            return pc.AddCreateChildPage("Plotlines", new Plotline(),2, new List<int>() { 2, 3 });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plotline plotline = db.Plotlines.Find(id);
            if (plotline == null)
            {
                return HttpNotFound();
            }

            Session["destinedController"] = "Plotlines";
            if (PageManager.CurrentPlotline == null)
            {
                Session["destinedMethod"] = "Home";
            }
            else
            {
                Session["destinedMethod"] = "Details";
            }

            PageManager.CurrentPlotline = plotline;

            return pc.AddEditPage("Plotlines", plotline,2, new List<int>() { 2, 3 });
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plotline plotline = db.Plotlines.Find(id);
            if (plotline == null)
            {
                return HttpNotFound();
            }

            Session["destinedController"] = "Plotlines";
            if (PageManager.CurrentPlotline == null)
            {
                Session["destinedMethod"] = "Home";
            }
            else
            {
                Session["destinedMethod"] = "Details";
            }

            PageManager.CurrentPlotline = plotline;

            return pc.AddDeletePage("Plotlines", plotline,4);
        }

        public ActionResult PickForMPB()
        {
            var plotlines = db.Plotlines.Where(c => c.ProjectId == PageManager.CurrentProject.ID).ToList();
            var pickablePlotlines = RemoveUsedPlotlinesBA(plotlines);

            Session["destinedController"] = "Books";
            Session["destinedMethod"] = "Details";
            Session["method"] = "BindPlotlineToBook";

            return pc.AddPickOnePage("Plotlines", pickablePlotlines);
        }

        public ActionResult PickForMPA()
        {
            var plotlines = db.Plotlines.Where(c => c.ProjectId == PageManager.CurrentProject.ID).ToList();
            var pickablePlotlines = RemoveUsedPlotlinesBA(plotlines);

            Session["destinedController"] = "Arcs";
            Session["destinedMethod"] = "Details";
            Session["method"] = "BindPlotlineToArc";

            return pc.AddPickOnePage("Plotlines", pickablePlotlines);
        }

        public ActionResult PickForDP()
        {
            var plotlines = db.Plotlines.Where(c => c.ProjectId == PageManager.CurrentProject.ID).ToList();
            var plotline = PageManager.CurrentPlotline;
            var pickablePlotlines = RemoveUsedPlotlinesP(plotlines,plotline);

            Session["destinedController"] = "Plotlines";
            Session["destinedMethod"] = "Details";
            Session["method"] = "BindPlotlineToPlotline";

            return pc.AddPickManyPage("Plotlines", pickablePlotlines);
        }

        public ActionResult PickForC()
        {
            var character = PageManager.CurrentCharacter;
            var plotlines = db.Plotlines.Where(p => p.ProjectId == PageManager.CurrentProject.ID).ToList();
            var pickablePlotlines = new List<Plotline>();
            plotlines.ForEach(p =>
            {
                if(p.Characters == null || !p.Characters.Any() || !p.Characters.Contains(character))
                {
                    pickablePlotlines.Add(p);
                }
            });
            var plotline = PageManager.CurrentPlotline;

            Session["destinedController"] = "Characters";
            Session["destinedMethod"] = "Details";
            Session["method"] = "BindPlotlineToCharacter";

            return pc.AddPickManyPage("Plotlines", pickablePlotlines);
        }

        // POST: Plotlines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Summary")] Plotline plotline)
        {
            pc.RemovePage("Create","Plotlines");

            if (ModelState.IsValid)
            {
                plotline.ProjectId = PageManager.CurrentProject.ID;
                plotline.LastModified = DateTime.Now;
                db.Plotlines.Add(plotline);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = plotline.ID });
            }

            return RedirectToAction("Create");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateChild([Bind(Include = "Name,Summary")] Plotline plotline)
        {
            pc.RemovePage("CreateChild","Plotlines");

            if (ModelState.IsValid)
            {
                plotline.ProjectId = PageManager.CurrentProject.ID;
                plotline.LastModified = DateTime.Now;

                var character = db.Characters.Find(Session["currentCharacterId"]);
                plotline.Characters.Add(character);
                db.Plotlines.Add(plotline);
                db.SaveChanges();

                db.Entry(character).State = EntityState.Modified;
                character.Plotlines.Add(plotline);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = plotline.ID });
            }

            return RedirectToAction("CreateC");
        }

        // POST: Plotlines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Summary")] Plotline plotline)
        {
            pc.RemovePage("Edit","Plotlines");

            if (ModelState.IsValid)
            {
                db.Entry(plotline).State = EntityState.Modified;
                plotline.ProjectId = PageManager.CurrentProject.ID;
                plotline.LastModified = DateTime.Now;
                db.SaveChanges();
                if ((string)Session["destinedMethod"] == "Details")
                {
                    return RedirectToAction("Details", new { id = plotline.ID });
                }
                else
                {
                    return RedirectToAction((string)Session["destinedMethod"]);
                }
            }
            return RedirectToAction("Edit", new { id = plotline.ID });
        }

        // POST: Plotlines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pc.RemovePage("Delete","Plotlines");

            Plotline plotline = db.Plotlines.Find(id);

            Helper.RemovePlotReferences(plotline);

            db.Plotlines.Remove(plotline);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult BindPlotlineToBook(int? id)
        {
            var book = db.Books.Find(PageManager.CurrentBook.ID);
            Plotline plotline = null;
            if (id != null)
            {
                plotline = db.Plotlines.Find(id);
                if (book == null || plotline == null)
                {
                    return HttpNotFound();
                }
            }

            book.MainPlotline = plotline;
            if (id != null)
                book.PlotlineId = plotline.ID;
            else book.PlotlineId = null;
            db.SaveChanges();

            pc.RemovePage("PickOne", "Plotlines");

            return RedirectToAction("Details", "Books");
        }

        public ActionResult BindPlotlineToArc(int? id)
        {
            var arc = db.Arcs.Find(PageManager.CurrentArc.ID);
            Plotline plotline = null;
            if (id != null)
            {
                plotline = db.Plotlines.Find(id);
                if (arc == null || plotline == null)
                {
                    return HttpNotFound();
                }
            }

            arc.MainPlotline = plotline;
            if (id != null)
                arc.PlotlineId = plotline.ID;
            else arc.PlotlineId = null;
            db.SaveChanges();

            pc.RemovePage("PickOne", "Plotlines");

            return RedirectToAction("Details", "Arcs");
        }

        public ActionResult BindPlotlineToCharacter(int? id)
        {
            var character = db.Characters.Find(PageManager.CurrentCharacter.ID);
            Plotline plotline = null;
            if (id != null)
            {
                plotline = db.Plotlines.Find(id);
                if (character == null || plotline == null)
                {
                    return HttpNotFound();
                }
            }

            character.Plotlines.Add(plotline);
            plotline.Characters.Add(character);
            db.SaveChanges();

            pc.RemovePage("PickMany", "Plotlines");

            return RedirectToAction("PickForC");
        }
       
        public ActionResult BindPlotlineToPlotline(int? id)
        {
            var plotlineOrg = db.Plotlines.Find(PageManager.CurrentPlotline.ID);
            Plotline plotline = null;
            if (id != null)
            {
                plotline = db.Plotlines.Find(id);
                if (plotlineOrg == null || plotline == null)
                {
                    return HttpNotFound();
                }
            }

            plotlineOrg.DependentPlotlinesF.Add(plotline);
            plotline.DependentPlotlinesA.Add(plotlineOrg);
            db.SaveChanges();

            pc.RemovePage("PickMany", "Plotlines");

            var plotlines = db.Plotlines.Where(c => c.ProjectId == PageManager.CurrentProject.ID).ToList();
            var pickablePlotlines = RemoveUsedPlotlinesP(plotlines, plotline);
            if(pickablePlotlines != null && pickablePlotlines.Any())
            {
                return RedirectToAction("PickForDP");
            }
            else
            {
                return RedirectToAction("Details", new { id = plotlineOrg.ID });
            }

        }

        public ActionResult RemoveFromPlotline(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var plotlineOrg = db.Plotlines.Find(PageManager.CurrentPlotline.ID);
            var plotline = db.Plotlines.Find(id);
            if (plotlineOrg == null || plotline == null)
            {
                return HttpNotFound();
            }

            plotlineOrg.DependentPlotlinesF.Remove(plotline);
            plotline.DependentPlotlinesA.Remove(plotlineOrg);
            db.SaveChanges();

            return RedirectToAction("Details");
        }
        
        public ActionResult RemoveFromCharacter(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var character = db.Characters.Find(PageManager.CurrentCharacter.ID);
            var plotline = db.Plotlines.Find(id);
            if (character == null || plotline == null)
            {
                return HttpNotFound();
            }

            character.Plotlines.Remove(plotline);
            plotline.Characters.Remove(character);
            db.SaveChanges();

            return RedirectToAction("Details", "Characters", new { id = character.ID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<Plotline> RemoveUsedPlotlinesBA(List<Plotline> plotlines)
        {
            var books = db.Books.Where(x => x.ProjectId == PageManager.CurrentProject.ID).ToList();
            var arcs = db.Arcs.ToList();
            var pickablePlotlines = new List<Plotline>();
            if (plotlines.Count != 0)
            {
                for (int i = 0; i < plotlines.Count; i++)
                {
                    var contains = false;
                    books.ForEach(b =>
                    {
                        if (b.PlotlineId == plotlines[i].ID)
                        {
                            contains = true;
                        }
                    });
                    arcs.ForEach(a =>
                    {
                        if (a.PlotlineId == plotlines[i].ID)
                        {
                            contains = true;
                        }
                    });

                    if (!contains)
                    {
                        pickablePlotlines.Add(plotlines[i]);
                    }
                }
            }

            return pickablePlotlines;
        }

        private List<Plotline> RemoveUsedPlotlinesP(List<Plotline> plotlines, Plotline plotline)
        {
            var pickablePlotlines = new List<Plotline>();
            if (plotlines != null && plotlines.Any())
            {
                plotlines.ForEach(p =>
                {
                    if (plotline.ID != p.ID && (plotline.DependentPlotlinesF== null || !plotline.DependentPlotlinesF.Any() || !plotline.DependentPlotlinesF.Contains(p)) && (plotline.DependentPlotlinesA == null || !plotline.DependentPlotlinesA.Any() || !plotline.DependentPlotlinesA.Contains(p)))
                    {
                        pickablePlotlines.Add(p);
                    }
                });
            }

            return pickablePlotlines;
        }

    }
}
